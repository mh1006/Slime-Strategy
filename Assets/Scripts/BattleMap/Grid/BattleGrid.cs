using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using JetBrains.Annotations;
using SlimeStrategy.BattleMap.Grid.Pathfinding;
using SlimeStrategy.BattleMap.Units;
using UnityEngine;
using UnityEngine.InputSystem;
using Graph = SlimeStrategy.BattleMap.Grid.Pathfinding.Graph;


namespace SlimeStrategy.BattleMap.Grid
{
    public class BattleGrid : MonoBehaviour
    {
        #region SerializeFields

        [SerializeField] private BattleManager _battleManager;

        [Tooltip("Amount of grid cells in X+ direction from origin")] [SerializeField]
        private int width;

        public int Width => width;

        [Tooltip("Amount of grid cells in Z+ direction from origin")] [SerializeField]
        private int depth;

        public int Depth => depth;

        [Tooltip("Corner marking the origin (0, 0)")] [SerializeField]
        private Transform cornerO;

        [Tooltip("Corner marking the X+ point (width - 1, 0)")] [SerializeField]
        private Transform cornerX;

        [Tooltip("Corner marking the Z+ point (0, depth - 1)")] [SerializeField]
        private Transform cornerZ;

        [Tooltip("Corner marking the XZ+ point (width - 1, depth - 1)")] [SerializeField]
        private Transform cornerXZ;

        [SerializeField] private GameObject overlayParentObject;
        [SerializeField] private Transform mouseCellOverlay;
        [SerializeField] private Transform selectCellOverlay;
        [SerializeField] private GameObject cellHighlightPrefab;

        private Vector3 _corner0Pos;
        private Vector3 _corner1Pos;
        private Vector3 _corner2Pos;
        private Vector3 _corner3Pos;

        private float _gridCellWidth;
        private float _gridCellDepth;
        private Camera _mainCamera;

        #endregion

        private Dictionary<GridCoordinate, GridSpace> _gridSpaces;
        private List<BattleUnit> _units;

        public List<BattleUnit> Units => _units;

        public Dictionary<GridCoordinate, GridSpace> GridSpaces => _gridSpaces;

        private Dijkstra _distances;

        public Dijkstra Distances => _distances;

        private List<GameObject> _movementOverlayHighlights;
        private AudioSource _damageSound;

        public GridSpace Register(GridObject obj)
        {
            GridCoordinate coord = WorldPosToGridCoordinate(obj.transform.position);
            _gridSpaces[coord].Add(obj);
            // Debug.Log($"GridObject registered @ {coord}");

            // Check if this is a unit
            var unit = obj.GetComponent<BattleUnit>();
            if (!ReferenceEquals(unit, null)) _units.Add(unit);

            return _gridSpaces[coord];
        }

        public void Unregister(GridObject obj)
        {
            var objs = _gridSpaces[obj.Coordinate];
            if (objs.Contains(obj))
            {
                // Debug.Log($"GridObject unregistered @ {obj.Coordinate}");
                objs.Remove(obj);
                UpdateGridPathfinding();
                var unit = obj.GetComponent<BattleUnit>();
                if (!ReferenceEquals(unit, null)) _units.Remove(unit);
            }
            else
            {
                Debug.LogWarning("GridObject unregister call, but it was not found");
            }
        }

        [CanBeNull]
        public BattleUnit GetBattleUnit(GridCoordinate coordinate)
        {
            return _gridSpaces[coordinate].BattleUnit;
        }

        public bool HasBattleUnit(GridCoordinate coordinate)
        {
            return _gridSpaces[coordinate].HasUnit();
        }

        public List<BattleUnit> GetBattleUnits(UnitFaction faction)
        {
            var result = new List<BattleUnit>();
            foreach (var unit in _units)
            {
                if (unit.UnitFaction == faction) result.Add(unit);
            }

            return result;
        }
        
        public bool MoveUnit(BattleUnit unit, GridSpace to)
        {
            if (!CanMoveTo(unit, to)) return false;

            var from = unit.Space;
            var distance = Distance(from, to);

            from.Remove(unit.GetComponent<GridObject>());
            to.Add(unit.GetComponent<GridObject>());
            var moveSuccess = unit.Move(to, distance);
            return moveSuccess;
        }

        public bool CanMoveTo(BattleUnit unit, GridSpace toSpace)
        {
            // Can't move after attacking
            if (unit.HasAttacked) return false;

            var fromSpace = _gridSpaces[unit.Coordinate];
            if (toSpace.HasUnit()) return false;
            var distance = Distance(fromSpace, toSpace);
            // Debug.Log($"Want move: {distance}");
            // Debug.Log($"Can move : {unit.RemainingMovement}");
            if (distance > unit.RemainingMovement) return false;
            return true;
        }

        public bool AttackUnit(BattleUnit source, BattleUnit target)
        {
            if (!CanAttackUnit(source, target)) return false;
            var success = source.Attack(target);
            if (success) _damageSound.Play();
            return success;
        }

        public bool CanAttackUnit(BattleUnit source, BattleUnit target)
        {
            if (source.HasAttacked) return false;
            var distance = Distance(source.Coordinate, target.Coordinate);
            if (distance > source.UnitStats.ActRange) return false;
            return true;
        }

        // TODO All this overlay code should go to another class
        public void OverlaySelect(GridSpace space)
        {
            if (ReferenceEquals(space, null))
            {
                selectCellOverlay.gameObject.SetActive(false);
                Overlay_DestroyMovementHighlights();
                return;
            }

            selectCellOverlay.position = GridCoordinateToWorldPos(space.Coordinate);
            selectCellOverlay.gameObject.SetActive(true);
            Overlay_CreateMovementHighlights(space);
        }

        public void Overlay_CreateMovementHighlights(GridSpace space)
        {
            if (!space.HasUnit()) return;
            var unit = space.BattleUnit;
            
            // Create list if it doesn't exist
            _movementOverlayHighlights ??= new List<GameObject>();
            
            if (_movementOverlayHighlights.Count != 0) Overlay_DestroyMovementHighlights();
            
            Distances.Calculate(space);
            var (dist, _) = Distances.Paths[space];

            for (int i = 0; i < dist.Length; i++)
            {
                if (dist[i] > unit.RemainingMovement) continue;
                // Else, the unit can move here, so make a cell highlight object
                
                // Get coordinate from gridspace ID
                var coord = new GridCoordinate(i % width, i / width);
                var pos = GridCoordinateToWorldPos(coord);

                Debug.Log(pos);
                var obj = Instantiate(cellHighlightPrefab, pos, cellHighlightPrefab.transform.rotation);
                obj.transform.parent = overlayParentObject.transform;
                _movementOverlayHighlights.Add(obj);
            }

        }
        
        public void Overlay_DestroyMovementHighlights()
        {
            if (ReferenceEquals(_movementOverlayHighlights, null))
            {
                _movementOverlayHighlights = new List<GameObject>();
                return;
            }

            if (_movementOverlayHighlights.Count == 0) return;
            
            _movementOverlayHighlights.RemoveAll(item =>
            {
                Destroy(item);
                return true;
            });
        }

        public int Distance(GridSpace from, GridSpace to)
        {
            return _distances.Distance(from, to);
        }

        public int Distance(GridCoordinate from, GridCoordinate to)
        {
            var fromSpace = _gridSpaces[from];
            var toSpace = _gridSpaces[to];
            return Distance(fromSpace, toSpace);
        }

        public int Distance(BattleUnit from, GridCoordinate to)
        {
            var toSpace = _gridSpaces[to];
            return Distance(from.Space, toSpace);
        }

        public int Distance(BattleUnit from, GridSpace to)
        {
            return Distance(from.Space, to);
        }

        public int Distance(BattleUnit from, BattleUnit to)
        {
            return Distance(from.Space, to.Space);
        }

        public void UpdateGridPathfinding()
        {
            UnitFaction turnFaction;
            if (_battleManager.State == BattleState.EnemyTurn)
            {
                turnFaction = UnitFaction.EnemyFaction;
            }
            else
            {
                turnFaction = UnitFaction.PlayerFaction;
            }

            var graph = GenerateGraph(turnFaction);
            _distances = new Dijkstra(graph);
            
            var preCalculatePaths = false;
            if (!preCalculatePaths) return;
            
            var units = GetBattleUnits(turnFaction);
            foreach (var unit in units)
            {
                // If unit has attacked, no need to pre-calculate its pathfinding, it's probably not going to be used
                if (unit.HasAttacked) continue;

                _distances.Calculate(unit.Space);
            }
        }

        public void SetupNextTurn()
        {
            for (int x = 0; x < width; x++)
            {
                for (int z = 0; z < depth; z++)
                {
                    var coord = new GridCoordinate(x, z);
                    var unit = GetBattleUnit(coord);
                    if (!ReferenceEquals(unit, null))
                    {
                        unit.ResetForNewTurn();
                    }
                }
            }

            UpdateGridPathfinding();
        }

        #region Context menu actions
        
        [ContextMenu("Debug - Print grid")]
        private void PrettyPrintGrid()
        {
            var elements = new List<string>();

            for (var z = depth - 1; z >= 0; z--)
            {
                for (var x = 0; x < width; x++)
                {
                    var coord = new GridCoordinate(x, z);
                    var space = _gridSpaces[coord];
                    if (space.HasUnit())
                    {
                        if (space.BattleUnit.UnitFaction == UnitFaction.PlayerFaction)
                        {
                            // elements.Add("<color=cyan>O</color>");
                            elements.Add("S");
                        }
                        else
                        {
                            // elements.Add("<color=red>X</color>");
                            elements.Add("E");
                        }
                    }
                    else if (space.IsTerrainBlocked())
                    {
                        elements.Add("T");
                    }
                    else
                    {
                        elements.Add(" ");
                    }
                }
            }

            var prettyPrintMatrix = "BattleGrid:\n" + PrettyPrintMatrix(elements.ToArray(), width, 3);
            Debug.Log(prettyPrintMatrix);
        }
        
        [ContextMenu("Debug - Print current friendly distances")]
        private void Context_PrintCurrentFriendlyDijkstraDistances()
        {
            GridSpace space = GetBattleUnits(UnitFaction.PlayerFaction)[0].Space;

            var dijkstraData = _distances.Paths[space];
            var dist = dijkstraData.Item1;
            var prev = dijkstraData.Item2;

            var distStrings = new List<string>();

            for (var z = depth - 1; z >= 0; z--)
            {
                for (var x = 0; x < width; x++)
                {
                    var index = z * width + x;
                    var d = dist[index];
                    if (d == int.MaxValue)
                    {
                        distStrings.Add("X");
                    }
                    else
                    {
                        distStrings.Add(d.ToString());
                    }
                }
            }

            var prettyPrintMatrix = "BattleGrid, current friendly distances:\n" + PrettyPrintMatrix(distStrings.ToArray(), width, 3);
            Debug.Log(prettyPrintMatrix);
        }
        
        [ContextMenu("Debug - Print current enemy distances")]
        private void Context_PrintCurrentEnemyDijkstraDistances()
        {
            GridSpace space = GetBattleUnits(UnitFaction.EnemyFaction)[0].Space;

            var dijkstraData = _distances.Paths[space];
            var dist = dijkstraData.Item1;
            var prev = dijkstraData.Item2;

            var distStrings = new List<string>();

            for (var z = depth - 1; z >= 0; z--)
            {
                for (var x = 0; x < width; x++)
                {
                    var index = z * width + x;
                    var d = dist[index];
                    if (d == int.MaxValue)
                    {
                        distStrings.Add("X");
                    }
                    else
                    {
                        distStrings.Add(d.ToString());
                    }
                }
            }

            var prettyPrintMatrix = "BattleGrid, current enemy distances:\n" + PrettyPrintMatrix(distStrings.ToArray(), width, 3);
            Debug.Log(prettyPrintMatrix);
        }

        [ContextMenu("Debug - Print new friendly distances")]
        private void Context_PrintNewFriendlyDijkstraDistances()
        {
            // Stopwatch sw = new Stopwatch();
            // sw.Start();

            GridSpace space = GetBattleUnits(UnitFaction.PlayerFaction)[0].Space;
            Graph graph = GenerateGraph(space.BattleUnit);
            Dijkstra dijkstra = new Dijkstra(graph);

            // sw.Stop();

            // Debug.Log(string.Format("Time elapsed (s): {0}", sw.Elapsed.TotalSeconds));
            // Debug.Log(string.Format("Time elapsed (ms): {0}", sw.Elapsed.TotalMilliseconds));
            // Debug.Log(string.Format("Time elapsed (μs): {0}",sw.Elapsed.TotalMilliseconds * 1000));

            // sw = new Stopwatch();
            // sw.Start();

            dijkstra.Calculate(space);

            // sw.Stop();
            //
            // Debug.Log(string.Format("Time elapsed (s): {0}", sw.Elapsed.TotalSeconds));
            // Debug.Log(string.Format("Time elapsed (ms): {0}", sw.Elapsed.TotalMilliseconds));
            // Debug.Log(string.Format("Time elapsed (μs): {0}",sw.Elapsed.TotalMilliseconds * 1000));

            var dijkstraData = dijkstra.Paths[space];
            var dist = dijkstraData.Item1;
            var prev = dijkstraData.Item2;

            var distStrings = new List<string>();

            for (var z = depth - 1; z >= 0; z--)
            {
                for (var x = 0; x < width; x++)
                {
                    var index = z * width + x;
                    var d = dist[index];
                    if (d == int.MaxValue)
                    {
                        distStrings.Add("X");
                    }
                    else
                    {
                        distStrings.Add(d.ToString());
                    }
                }
            }

            var prettyPrintMatrix = "BattleGrid:\n" + PrettyPrintMatrix(distStrings.ToArray(), width, 3);
            Debug.Log(prettyPrintMatrix);
        }

        [ContextMenu("Debug - Print new enemy distances")]
        private void Context_PrintNewEnemyDijkstraDistances()
        {
            // Stopwatch sw = new Stopwatch();
            // sw.Start();

            GridSpace space = GetBattleUnits(UnitFaction.EnemyFaction)[0].Space;
            Graph graph = GenerateGraph(space.BattleUnit);
            Dijkstra dijkstra = new Dijkstra(graph);

            // sw.Stop();

            // Debug.Log(string.Format("Time elapsed (s): {0}", sw.Elapsed.TotalSeconds));
            // Debug.Log(string.Format("Time elapsed (ms): {0}", sw.Elapsed.TotalMilliseconds));
            // Debug.Log(string.Format("Time elapsed (μs): {0}",sw.Elapsed.TotalMilliseconds * 1000));

            // sw = new Stopwatch();
            // sw.Start();

            dijkstra.Calculate(space);

            // sw.Stop();
            //
            // Debug.Log(string.Format("Time elapsed (s): {0}", sw.Elapsed.TotalSeconds));
            // Debug.Log(string.Format("Time elapsed (ms): {0}", sw.Elapsed.TotalMilliseconds));
            // Debug.Log(string.Format("Time elapsed (μs): {0}",sw.Elapsed.TotalMilliseconds * 1000));

            var dijkstraData = dijkstra.Paths[space];
            var dist = dijkstraData.Item1;
            var prev = dijkstraData.Item2;

            var distStrings = new List<string>();

            for (var z = depth - 1; z >= 0; z--)
            {
                for (var x = 0; x < width; x++)
                {
                    var index = z * width + x;
                    var d = dist[index];
                    if (d == int.MaxValue)
                    {
                        distStrings.Add("X");
                    }
                    else
                    {
                        distStrings.Add(d.ToString());
                    }
                }
            }

            var prettyPrintMatrix = "BattleGrid:\n" + PrettyPrintMatrix(distStrings.ToArray(), width, 3);
            Debug.Log(prettyPrintMatrix);
        }

        private String PrettyPrintMatrix(string[] array, int width, int elementWidth)
        {
            var topSep = "╤";
            var botSep = "╧";
            var lSep = "╟";
            var rSep = "╢";
            var horLine = "─";
            var verLine = "│";
            var crossLine = "┼";
            var outHorLine = "═";
            var outVerLine = "║";

            var sb = new StringBuilder();

            // Make top line
            sb.Append("╔");
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < elementWidth; j++)
                {
                    sb.Append(outHorLine);
                }

                sb.Append(topSep);
            }

            sb.Remove(sb.Length - 1, 1);
            sb.Append("╗");
            sb.AppendLine();

            // Make middle lines
            for (int line = 0; line < array.Length / width; line++)
            {
                sb.Append(outVerLine);
                for (int i = 0; i < width; i++)
                {
                    int idx = i + line * width;
                    for (int j = 0; j < (elementWidth / 2) + (elementWidth % 2) - array[idx].Length; j++)
                    {
                        sb.Append(" ");
                    }

                    sb.Append(array[idx]);
                    for (int j = 0; j < (elementWidth / 2); j++)
                    {
                        sb.Append(" ");
                    }

                    sb.Append(verLine);
                }

                sb.Remove(sb.Length - 1, 1);
                sb.Append(outVerLine);
                sb.AppendLine();

                if (line != (array.Length / width) - 1)
                {
                    sb.Append(lSep);
                    for (int i = 0; i < width; i++)
                    {
                        for (int j = 0; j < elementWidth; j++)
                        {
                            sb.Append(horLine);
                        }

                        sb.Append(crossLine);
                    }

                    sb.Remove(sb.Length - 1, 1);
                    sb.Append(rSep);
                    sb.AppendLine();
                }
            }


            // Make bot line
            sb.Append("╚");
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < elementWidth; j++)
                {
                    sb.Append(outHorLine);
                }

                sb.Append(botSep);
            }

            sb.Remove(sb.Length - 1, 1);
            sb.Append("╝");
            return sb.ToString();
        }

        #endregion

        #region Coordinate conversion math

        private bool CoordinateInBounds(GridCoordinate coordinate)
        {
            var x = coordinate.X;
            var z = coordinate.Z;
            return 0 <= x && x < width &&
                   0 <= z && z < depth;
        }

        public Vector3 GridCoordinateToWorldPos(GridCoordinate c)
        {
            if (!CoordinateInBounds(c))
                throw new UnityException("Grid position out of bounds");

            var x = c.X;
            var z = c.Z;

            float xOut = Mathf.Lerp(_corner0Pos.x, _corner1Pos.x, (float)x / (width - 1));
            float zOut = Mathf.Lerp(_corner0Pos.z, _corner2Pos.z, (float)z / (depth - 1));
            float yOut = _corner0Pos.y;

            return new Vector3(xOut, yOut, zOut);
        }

        public Vector3 GridCoordinateToWorldPos(int x, int z)
        {
            return GridCoordinateToWorldPos(new GridCoordinate(x, z));
        }

        public Vector3 SnapWorldPosToGrid(Vector3 position)
        {
            // World pos_x / cellWidth -> gridCell_x
            // Round gridCell_x to whole numbers, then mult cellWidth to get snapped position near original position
            // Clamp to corners

            float x = (float)Math.Clamp(Math.Round(position.x / _gridCellWidth) * _gridCellWidth, _corner0Pos.x,
                _corner1Pos.x);
            float z = (float)Math.Clamp(Math.Round(position.z / _gridCellDepth) * _gridCellDepth, _corner0Pos.z,
                _corner2Pos.z);
            float y = _corner0Pos.y;

            return new Vector3(x, y, z);
        }

        public GridCoordinate WorldPosToGridCoordinate(Vector3 position)
        {
            int x = Math.Clamp((int)Math.Round(position.x / _gridCellWidth), 0, width - 1);
            int z = Math.Clamp((int)Math.Round(position.z / _gridCellDepth), 0, depth - 1);

            return new GridCoordinate(x, z);
        }

        #endregion

        #region Graph generation

        public Graph GenerateGraph(BattleUnit unit)
        {
            return GenerateGraph(unit.UnitFaction);
        }

        public Graph GenerateGraph(UnitFaction unitFaction)
        {
            Debug.Log($"[BattleGrid] <color=red>Generating pathfinding graph</color> with {_gridSpaces.Keys.Count} nodes");
            var graph = new Graph(_gridSpaces.Keys.Count);
            foreach (var space in _gridSpaces.Values)
            {
                graph.AssociateSpace(space);
            }
            foreach (var coordinate in _gridSpaces.Keys)
            {
                // Check X+
                MakeGraphEdgeWithBlockCheck(graph, unitFaction, coordinate,
                    new GridCoordinate(coordinate.X + 1, coordinate.Z));
                // Check X-
                MakeGraphEdgeWithBlockCheck(graph, unitFaction, coordinate,
                    new GridCoordinate(coordinate.X - 1, coordinate.Z));
                // Check Z+
                MakeGraphEdgeWithBlockCheck(graph, unitFaction, coordinate,
                    new GridCoordinate(coordinate.X, coordinate.Z + 1));
                // Check Z-
                MakeGraphEdgeWithBlockCheck(graph, unitFaction, coordinate,
                    new GridCoordinate(coordinate.X, coordinate.Z - 1));
            }

            return graph;
        }

        private void MakeGraphEdgeWithBlockCheck(Graph graph, UnitFaction unitFaction, GridCoordinate from,
            GridCoordinate to)
        {
            if (!CoordinateInBounds(to)) return;
            var fromSpace = _gridSpaces[from];
            var toSpace = _gridSpaces[to];

            if (fromSpace.IsBlocked_MoveOut(unitFaction)) return;
            if (toSpace.IsBlocked_MoveIn(unitFaction)) return;
            graph.MakeEdge(fromSpace.ID, toSpace.ID);
        }

        #endregion

        #region Plumbing

        private void Update()
        {
            Ray ray = _mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());
            if (!Physics.Raycast(ray, out var hit)) return;
            // If we didn't hit a collider, or the collider isn't the grid, deactivate mouse overlay
            if (!hit.collider || hit.collider.gameObject != this.gameObject)
            {
                mouseCellOverlay.gameObject.SetActive(false);
                _battleManager.HoveredUnit = null;
            }
            // Else the mouse is over the grid, activate overlay and update position
            else
            {
                var mouseCellOverlayObject = this.mouseCellOverlay.gameObject;
                mouseCellOverlayObject.SetActive(true);
                mouseCellOverlayObject.transform.position = SnapWorldPosToGrid(hit.point);
                var hitCoord = WorldPosToGridCoordinate(hit.point);
                _battleManager.HoveredUnit = _gridSpaces[hitCoord].HasUnit() ? _gridSpaces[hitCoord].BattleUnit : null;
                // if (Time.frameCount % 60 == 0)
                // {
                //     var log = $"hit.point = {hit.point}\n";
                //     log += $"snapped point = {SnapWorldPosToGrid(hit.point)}\n";
                //     log += $"mouseCellOverlay = {mouseCellOverlayObject.transform.position}\n";
                //     log += $"hitCoord = {hitCoord}";
                //     
                //     Debug.Log(log);
                // }
            }
        }

        private void Awake()
        {
            _mainCamera = Camera.main;
            _corner0Pos = cornerO.position;
            _corner1Pos = cornerX.position;
            _corner2Pos = cornerZ.position;
            _corner3Pos = cornerXZ.position;

            _gridCellWidth = Mathf.Abs(_corner1Pos.x - _corner0Pos.x) / (width - 1);
            _gridCellDepth = Mathf.Abs(_corner2Pos.z - _corner0Pos.z) / (depth - 1);

            // Create a box collider that covers the grid
            var boxCollider = gameObject.AddComponent<BoxCollider>();
            float colliderWidth = _gridCellWidth * width;
            float colliderDepth = _gridCellDepth * depth;
            float colliderHeight = 0.1f;

            boxCollider.size = new Vector3(colliderWidth, colliderHeight, colliderDepth);
            // float worldE2ECenterX = (_corner1_pos.x - _corner0_pos.x) / width;
            // float worldE2ECenterZ = (_corner1_pos.z - _corner0_pos.z) / width;
            boxCollider.center = new Vector3(_gridCellWidth * ((width - 1) / 2f), -(colliderHeight / 2),
                _gridCellDepth * ((depth - 1) / 2f));

            _gridSpaces = new Dictionary<GridCoordinate, GridSpace>(new GridCoordinateEqualityComparer());
            _units = new List<BattleUnit>();

            for (int z = 0; z < depth; z++)
            {
                for (int x = 0; x < width; x++)
                {
                    var coord = new GridCoordinate(x, z);
                    var space = new GridSpace(x + z * width, coord);
                    _gridSpaces.Add(coord, space);
                }
            }

            Debug.Log(_gridSpaces[new GridCoordinate(0, 0)].Count);
            _damageSound = gameObject.GetComponent<AudioSource>();
        }

        private void Start()
        {
            StartCoroutine(LateStart());
        }

        private IEnumerator LateStart()
        {
            yield return new WaitForEndOfFrame();
            UpdateGridPathfinding();
        }

        #endregion
    }
}