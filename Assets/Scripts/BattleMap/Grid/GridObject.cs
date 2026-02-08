using UnityEngine;

namespace SlimeStrategy.BattleMap.Grid
{
    public class GridObject : MonoBehaviour
    {
        [SerializeField] private BattleGrid battleGrid;

        [Tooltip("This checkbox marks if this object will not move and is blocking pathfinding through a grid space")]
        [SerializeField]
        private bool staticBlocker;

        public bool StaticBlocker => staticBlocker;

        [SerializeField] private bool debugWalk;
        
        public GridCoordinate Coordinate => Space.Coordinate;
        public GridSpace Space { get; private set; }

        public void Move(GridSpace space)
        {
            Space = space;
            DoMoveAnimation();
        }

        // private void OnEnable()
        // {
        //     mouseClickAction.Enable();
        //     mouseClickAction.performed += MouseMove;
        // }
        //
        // private void OnDisable()
        // {
        //     mouseClickAction.performed -= MouseMove;
        //     mouseClickAction.Disable();
        // }

        #region Plumbing

        private Vector3 _targetPosition;
        private float _moveStart;
        private const float MoveDuration = 0.5f;

        private void Update()
        {
            // (Current time - move start time) / moveDuration should be [0-1] move completion 
            float t = Mathf.Clamp((Time.time - _moveStart) / MoveDuration, 0, 1);
            Vector3 newPosition = Vector3.Lerp(gameObject.transform.position, _targetPosition, t);
            // Vector3 newPosition = _targetPosition;
            gameObject.transform.position = newPosition;
        }

        private void Start()
        {
            Space = battleGrid.Register(this);
            _targetPosition = battleGrid.GridCoordinateToWorldPos(Coordinate);
        }

        private void OnDestroy()
        {
            battleGrid.Unregister(this);
        }

        private void DoMoveAnimation()
        {
            _moveStart = Time.time;
            _targetPosition = battleGrid.GridCoordinateToWorldPos(Coordinate);
        }

        #endregion
    }
}