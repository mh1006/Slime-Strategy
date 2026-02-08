using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SlimeStrategy.BattleMap.Grid;
using SlimeStrategy.BattleMap.Units;
using Unity.VisualScripting;
using UnityEngine;

namespace SlimeStrategy.BattleMap
{
    public class AIController : MonoBehaviour
    {
        [SerializeField] private BattleManager battleManager;

        private BattleState _previousState = BattleState.None;
        private State _state;

        private Queue<BattleUnit> _units;
        private Coroutine _handleTurnsCoroutine;

        private IEnumerator HandleTurns()
        {
            while (_units.Count > 0)
            {
                var unit = _units.Dequeue();
                yield return StartCoroutine(HandleUnitTurn(unit));
            }

            battleManager.EnemyEndTurnAction();
            _state = State.WaitingForTurn;
        }

        private IEnumerator HandleUnitTurn(BattleUnit unit)
        {
            Debug.Log($"[AIController] Handling unit {unit.UnitClass} at {unit.Coordinate}");

            var attackTargetUnit = FindTarget(unit);
            var targetSpace = FindTargetSpace(unit, attackTargetUnit);
            var moveSuccess = battleManager.BattleGrid.MoveUnit(unit, targetSpace);
            if (!moveSuccess) Debug.LogError($"[AIController] AI Unit move failed for some reason");

            // Wait for movement to complete
            yield return new WaitForSeconds(0.5f);

            battleManager.BattleGrid.AttackUnit(unit, attackTargetUnit);

            // Wait for dramatic effect
            yield return new WaitForSeconds(1);
            Debug.Log($"[AIController] Done handling unit {unit.UnitClass} at {unit.Coordinate}");
        }

        /// <summary>
        /// Will find the unit in its range it can deal the most damage to this turn, else the closest unit.
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        private BattleUnit FindTarget(BattleUnit source)
        {
            var playerUnits = battleManager.BattleGrid.GetBattleUnits(UnitFaction.PlayerFaction);
            
            // If list size is 0 or 1, the list is sorted.
            if (playerUnits.Count == 1) return playerUnits[0];
            if (playerUnits.Count == 0) return null;

            var unitData = new List<UnitData>();
            foreach (var unit in playerUnits)
            {
                unitData.Add(new UnitData
                {
                    Unit = unit, Distance = int.MaxValue, CalculatedDamage = int.MinValue
                });
            }
            
            for (int i = 0; i < unitData.Count; i++)
            {
                var targetUnit = unitData[i];
                
                targetUnit.Distance = battleManager.BattleGrid.Distance(source, targetUnit.Unit);
                targetUnit.Unit.CalculateDamage(source.UnitStats.AD, source.DamageType);

                unitData[i] = targetUnit;
            }

            // Sort by descending damage
            unitData.Sort((x, y) => x.CalculatedDamage.CompareTo(y.CalculatedDamage));

            var inRange = unitData.Where((unit) =>
            {
                return unit.Distance <= source.RemainingMovement + source.UnitStats.ActRange;
            }).ToList();

            if (inRange.Count > 1)
            {
                inRange.Sort((x, y) => x.Distance.CompareTo(y.Distance));
                return inRange[0].Unit;
            } else if (inRange.Count == 1)
            {
                return inRange[0].Unit;
            }
            
            unitData.Sort((x, y) => x.Distance.CompareTo(y.Distance));
            return unitData[0].Unit;
        }
        
        private GridSpace FindTargetSpace(BattleUnit sourceUnit, BattleUnit targetUnit)
        {
            var grid = battleManager.BattleGrid;
            var path = grid.Distances.Path(sourceUnit.Space, targetUnit.Space);
            var targetSpace = sourceUnit.Space;
            
            // TODO This is not the best algorithm. There is possibility for future improvement
            // For example: this might get stuck behind units when there's a perfectly valid space to stand
            // Try finding all GridSpaces within attack range, discard all occupied, sort by distance from source unit
            // Move to the closest
            // Possible to bias this to closest to allies as well.
            // After sorting by distance, discard all unreachable, then calculate min distance to allied unit
            foreach (var pathSpace in path)
            {
                if (grid.Distance(sourceUnit, pathSpace) > sourceUnit.UnitStats.MovRange) break;
                if (!pathSpace.HasUnit()) targetSpace = pathSpace;
                
                // If we have found a target space within attack range: stop, this is the space to go to.
                if (grid.Distance(targetSpace, targetUnit.Space) < sourceUnit.UnitStats.ActRange) break;
            }

            return targetSpace;
        }

        private List<BattleUnit> FilterUnitsByClass(List<BattleUnit> units, StatClass statClass)
        {
            var result = new List<BattleUnit>();
            foreach (var unit in units)
            {
                if (unit.UnitClass == statClass) result.Add(unit);
            }

            return result;
        }

        private void EnqueueAll<T>(Queue<T> queue, List<T> list)
        {
            if (list.Count == 0) return;
            foreach (var item in list)
            {
                queue.Enqueue(item);
            }
        }

        /// <summary>
        /// Sorts a list of BattleUnits by ascending distance to the closest player unit
        /// </summary>
        /// <param name="units"></param>
        private List<BattleUnit> SortByDistanceToClosestPlayerUnit(List<BattleUnit> units)
        {
            // If list size is 0 or 1, the list is sorted.
            if (units.Count < 2) return units;

            var unitDistances = new List<UnitData>();
            foreach (var unit in units)
            {
                unitDistances.Add(new UnitData
                {
                    Unit = unit, Distance = int.MaxValue
                });
            }

            var playerUnits = battleManager.BattleGrid.GetBattleUnits(UnitFaction.PlayerFaction);

            for (int i = 0; i < unitDistances.Count; i++)
            {
                var unitDistance = unitDistances[i];
                foreach (var playerUnit in playerUnits)
                {
                    var distance = battleManager.BattleGrid.Distance(unitDistance.Unit, playerUnit);
                    // Debug.Log($"Distance from {unit.Coordinate} to {playerUnit.Coordinate} is {distance}");
                    unitDistance.Distance = System.Math.Min(unitDistance.Distance, distance);
                }

                unitDistances[i] = unitDistance;
            }

            // Sort by ascending distance
            unitDistances.Sort((x, y) => x.Distance.CompareTo(y.Distance));

            var result = new List<BattleUnit>();
            foreach (var unit in unitDistances)
            {
                result.Add(unit.Unit);
                Debug.Log($"[AIController] Enemy turn order: {unit.Unit.Coordinate} - {unit.Distance}");
            }

            return result;
        }

        #region Plumbing

        private void Update()
        {
            CheckForBattleStateChange();
        }

        private void CheckForBattleStateChange()
        {
            if (_previousState == battleManager.State) return;
            var currentState = battleManager.State;

            Debug.Log($"AI Controller detected state change to {currentState}");

            // If the state has changed since last time, and the current state is EnemyTurn, start the turn
            if (currentState == BattleState.EnemyTurn)
                StartEnemyTurn();

            _previousState = currentState;
        }

        private void StartEnemyTurn()
        {
            if (!ReferenceEquals(_handleTurnsCoroutine, null)) StopCoroutine(_handleTurnsCoroutine);

            _units = new Queue<BattleUnit>();
            var units = battleManager.BattleGrid.GetBattleUnits(UnitFaction.EnemyFaction);

            var warriors = FilterUnitsByClass(units, StatClass.Warrior);
            var mages = FilterUnitsByClass(units, StatClass.Mage);
            var archers = FilterUnitsByClass(units, StatClass.Archer);
            var knights = FilterUnitsByClass(units, StatClass.Knight);
            var kings = FilterUnitsByClass(units, StatClass.King);

            warriors = SortByDistanceToClosestPlayerUnit(warriors);
            mages = SortByDistanceToClosestPlayerUnit(mages);
            archers = SortByDistanceToClosestPlayerUnit(archers);
            knights = SortByDistanceToClosestPlayerUnit(knights);
            kings = SortByDistanceToClosestPlayerUnit(kings);

            EnqueueAll(_units, warriors);
            EnqueueAll(_units, mages);
            EnqueueAll(_units, archers);
            EnqueueAll(_units, knights);
            EnqueueAll(_units, kings);

            #region Debug logging

            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"[AIController] Finished preparing for enemy turn");
            sb.AppendLine($"Found {_units.Count} units to perform an action with");
            Debug.Log(sb.ToString());

            #endregion

            _handleTurnsCoroutine = StartCoroutine(nameof(HandleTurns));
        }

        private void Awake()
        {
            _state = State.WaitingForTurn;
        }

        #endregion
    }

    internal enum State
    {
        None,
        WaitingForTurn,
        ProcessingUnit,
        ReadyForNextUnit
    }

    internal struct UnitData
    {
        public BattleUnit Unit;
        public int Distance;
        public int CalculatedDamage;
    }
}