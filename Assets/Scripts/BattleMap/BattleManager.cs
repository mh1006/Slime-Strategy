using System.Collections.Generic;
using SlimeStrategy.BattleMap.Grid;
using SlimeStrategy.BattleMap.Grid.Pathfinding;
using SlimeStrategy.BattleMap.Units;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

namespace SlimeStrategy.BattleMap
{
    public class BattleManager : MonoBehaviour
    {
        [SerializeField] private BattleGrid battleGrid;
        [SerializeField] public string nextScene;
        [SerializeField] private InputAction mouseSelectAction;
        [SerializeField] private InputAction mouseContextAction;
        [SerializeField] private InputAction moveCameraLeftAction;
        [SerializeField] private InputAction moveCameraRightAction;
        [SerializeField] private InputAction moveCameraForwardAction;
        [SerializeField] private InputAction moveCameraBackAction;
        [SerializeField] private InputAction resetCameraAction;

        private Camera _mainCamera;
        private BattleUnit _selectedUnit;
        private BattleState _battleState;
        private Dijkstra _dijkstra;
        private bool pointerOverUI;

        public BattleState State => _battleState;

        public BattleGrid BattleGrid => battleGrid;
        public List<BattleUnit> Units => battleGrid.Units;
        public BattleUnit SelectedUnit => _selectedUnit;
        public BattleUnit HoveredUnit { get; set; }
        public bool UnitIsSelected => !ReferenceEquals(_selectedUnit, null);
        public bool UnitIsHovered => !ReferenceEquals(HoveredUnit, null);

        private void SelectAction(InputAction.CallbackContext context)
        {
            // If it is not the player's turn, you may not select a unit
            if (_battleState != BattleState.PlayerTurn) return;
            // If mouse is over UI, don't proceed with raycasting to the level
            if (pointerOverUI) return;
            Ray ray = _mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());
            
            // Only continue if anything is hit
            if (!Physics.Raycast(ray, out var hit, 100) || !hit.collider) return;

            // Only continue if hit object is the battle grid
            if (hit.collider.gameObject.GetComponent<BattleGrid>() != battleGrid) return;
                
            var clickedCoordinate = battleGrid.WorldPosToGridCoordinate(hit.point);
            var clickedSpace = battleGrid.GridSpaces[clickedCoordinate];
            // TODO Ahh dirty
            if (!clickedSpace.HasUnit())
            {
                _selectedUnit = null;
                battleGrid.OverlaySelect(null);
                return;
            }
            var clickedUnit = clickedSpace.BattleUnit;
            if (clickedUnit!.UnitFaction == UnitFaction.PlayerFaction)
            {
                _selectedUnit = clickedUnit;
                battleGrid.OverlaySelect(clickedSpace);
            }
            else
            {
                _selectedUnit = null;
                battleGrid.OverlaySelect(null);
            }
        }

        // This action may move or may attack, depending on the context of the action
        // If a unit can move
        private void ContextAction(InputAction.CallbackContext context)
        {
            if (_battleState != BattleState.PlayerTurn) return;
            
            Ray ray = _mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());
            
            // Only continue if anything is hit
            if (!Physics.Raycast(ray, out var hit, 100) || !hit.collider) return;
            
            // Only continue if hit object is the battle grid
            if (hit.collider.gameObject.GetComponent<BattleGrid>() != battleGrid) return;
            
            var clickedCoordinate = battleGrid.WorldPosToGridCoordinate(hit.point);
            var clickedSpace = battleGrid.GridSpaces[clickedCoordinate];
            
            // TODO Could refactor to use something like _selectedUnit.CanDoAction(clickedUnit) ?
            if (!_selectedUnit) return;
            
            if (!clickedSpace.HasUnit())
            {
                // Try to move to the clicked space
                Unit_MoveAction(_selectedUnit, clickedSpace);
                return;
            }

            var clickedUnit = clickedSpace.BattleUnit;
            if (clickedUnit.IsEnemy(_selectedUnit))
            {
                // Attack the clicked unit
                Unit_AttackAction(_selectedUnit, clickedUnit);
            }
            
        }

        private void Unit_MoveAction(BattleUnit unit, GridSpace space)
        {
            var moveSuccess = battleGrid.MoveUnit(unit, space);
            if (moveSuccess)
            {
                battleGrid.OverlaySelect(space);
            }
        }

        private void Unit_AttackAction(BattleUnit source, BattleUnit target)
        {
            battleGrid.AttackUnit(source, target);
        }

        public bool Unit_ApplyElementAction(StatsModifierType element)
        {
            if (!UnitIsSelected) return false;
            
            var success = SelectedUnit.ApplyElement(element);

            
            if (!success) return false;
            
            // All elements except air modify movement range, so update the highlighted movement range
            if (element != StatsModifierType.FireElement)
            {
                battleGrid.OverlaySelect(_selectedUnit.Space);
            }
            
            return true;
        }
        
        public void PlayerEndTurnAction()
        {
            if (_battleState == BattleState.PlayerTurn)
            {
                _battleState = BattleState.EnemyTurn;
                battleGrid.SetupNextTurn();
                battleGrid.OverlaySelect(null);
                _selectedUnit = null;
            }
            
        }
        
        public void EnemyEndTurnAction()
        {
            if (_battleState == BattleState.EnemyTurn)
            {
                _battleState = BattleState.PlayerTurn;
                battleGrid.SetupNextTurn();
            }
            
        }

        #region Plumbing
        
        private void Awake()
        {
            _mainCamera = Camera.main;
            // TODO Need to start at deployment once that's done
            _battleState = BattleState.PlayerTurn;
            // _battleState = BattleState.Deployment;

        }

        private void OnEnable()
        {
            // Setup input
            mouseSelectAction.Enable();
            mouseSelectAction.performed += SelectAction;
            mouseContextAction.Enable();
            mouseContextAction.performed += ContextAction;
            
            moveCameraLeftAction.Enable();
            moveCameraLeftAction.performed += Action_MoveCameraLeft;
            moveCameraRightAction.Enable();
            moveCameraRightAction.performed += Action_MoveCameraRight;
            moveCameraForwardAction.Enable();
            moveCameraForwardAction.performed += Action_MoveCameraForward;
            moveCameraBackAction.Enable();
            moveCameraBackAction.performed += Action_MoveCameraBack;
            resetCameraAction.Enable();
            resetCameraAction.performed += Action_ResetCamera;
        }

        private void OnDisable()
        {
            // Disable input
            mouseSelectAction.performed -= SelectAction;
            mouseSelectAction.Disable();
            mouseContextAction.performed -= ContextAction;
            mouseContextAction.Disable();
            
            moveCameraLeftAction.Disable();
            moveCameraLeftAction.performed -= Action_MoveCameraLeft;
            moveCameraRightAction.Disable();
            moveCameraRightAction.performed -= Action_MoveCameraRight;
            moveCameraForwardAction.Disable();
            moveCameraForwardAction.performed -= Action_MoveCameraForward;
            moveCameraBackAction.Disable();
            moveCameraBackAction.performed -= Action_MoveCameraBack;
            resetCameraAction.Disable();
            resetCameraAction.performed -= Action_ResetCamera;
        }

        private void Update()
        {
            pointerOverUI = EventSystem.current.IsPointerOverGameObject();
            
            var isBattleOver = State == BattleState.Victory || State == BattleState.Defeat;
            if (isBattleOver) return;

            var friendlyUnits = battleGrid.GetBattleUnits(UnitFaction.PlayerFaction);
            if (friendlyUnits.Count == 0) _battleState = BattleState.Defeat;
            
            var enemyUnits = battleGrid.GetBattleUnits(UnitFaction.EnemyFaction);
            if (enemyUnits.Count == 0) _battleState = BattleState.Victory;
        }

        #region Camera movement actions

        private void MoveCamera(Vector3 moveDir)
        {
            _mainCamera.transform.position += moveDir;
        }

        private void Action_MoveCameraLeft(InputAction.CallbackContext context)
        {
            var moveDir = Vector3.left + Vector3.forward;
            MoveCamera(moveDir);
        }
        
        private void Action_MoveCameraRight(InputAction.CallbackContext context)
        {
            var moveDir = Vector3.right + Vector3.back;
            MoveCamera(moveDir);
        }
        
        private void Action_MoveCameraForward(InputAction.CallbackContext context)
        {
            var moveDir = Vector3.right + Vector3.forward;
            MoveCamera(moveDir);
        }
        
        private void Action_MoveCameraBack(InputAction.CallbackContext context)
        {
            var moveDir = Vector3.left + Vector3.back;
            MoveCamera(moveDir);
        }
        
        private void Action_ResetCamera(InputAction.CallbackContext context)
        {
            _mainCamera.transform.position = new Vector3(0, 4, 0);
        }

        #endregion

        #endregion
    }

    public enum BattleState
    {
        None,
        Deployment,
        PlayerTurn,
        EnemyTurn,
        Victory,
        Defeat
    }
}
