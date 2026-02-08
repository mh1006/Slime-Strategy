using System.Collections.Generic;
using SlimeStrategy.BattleMap.Units;
using UnityEngine;

namespace SlimeStrategy.BattleMap.Grid
{
    public class GridSpace
    {
        private readonly HashSet<GridObject> _gridObjects;
        private BattleUnit _battleUnit;
        private bool _staticBlocker;
        public int ID { get; }

        public BattleUnit BattleUnit => _battleUnit;
        public GridCoordinate Coordinate { get; private set; }

        public GridSpace(int id, GridCoordinate coordinate)
        {
            this.ID = id;
            this.Coordinate = coordinate;
            _gridObjects = new HashSet<GridObject>();
        }

        public bool HasUnit()
        {
            return !ReferenceEquals(_battleUnit, null);
        }

        /// <summary>
        /// Will calculate if a specific faction can move out of this space
        /// </summary>
        /// <param name="unitFaction"></param>
        /// <returns></returns>
        public bool IsBlocked_MoveOut(UnitFaction unitFaction)
        {
            if (HasUnit() && _battleUnit.IsEnemy(unitFaction)) return true;
            return IsTerrainBlocked();
        }
        
        /// <summary>
        /// Will calculate if a specific faction can move into this space
        /// </summary>
        /// <param name="unitFaction"></param>
        /// <returns></returns>
        public bool IsBlocked_MoveIn(UnitFaction unitFaction)
        {
            return IsTerrainBlocked();
        }

        // Is this grid space occupied by terrain
        public bool IsTerrainBlocked()
        {
            return _staticBlocker;
        }

        public bool Contains(GridObject gridObject)
        {
            return _gridObjects.Contains(gridObject);
        }

        #region Data structure methods

        public bool Add(GridObject gridObject)
        {
            if (gridObject.TryGetComponent(out BattleUnit battleUnit))
            {
                if (HasUnit())
                {
                    Debug.LogError("Tried to add a BattleUnit to a GridSpace that already contains a BattleUnit");
                    return false;
                }

                _battleUnit = battleUnit;
            }

            _staticBlocker = _staticBlocker | gridObject.StaticBlocker;
            _gridObjects.Add(gridObject);
            return true;
        }

        public bool Remove(GridObject gridObject)
        {
            var removed = _gridObjects.Remove(gridObject);
            if (!removed)
            {
                Debug.LogError("Tried to remove something from a GridSpace that wasn't found");
                return false;
            }

            if (gridObject.TryGetComponent(out BattleUnit _))
            {
                _battleUnit = null;
            }

            if (!gridObject.StaticBlocker) return true;
            
            // If the gridobject we're removing was a blocker, we need to recalculate the blocking.
            // Assume no blocker anymore
            _staticBlocker = false;
            foreach (var setGridObject in _gridObjects)
            {
                if (!setGridObject.StaticBlocker) continue;
                
                // If blocker is found, set the variable and stop looking
                _staticBlocker = true;
                break;
            }

            return true;
        }

        public GridObject Pop(GridObject gridObject)
        {
            var removed = _gridObjects.Remove(gridObject);
            if (!removed)
            {
                Debug.LogError("Tried to remove something from a GridSpace that wasn't found");
                return null;
            }

            if (gridObject.TryGetComponent(out BattleUnit battleUnit))
            {
                _battleUnit = null;
            }

            return gridObject;
        }

        public int Count => _gridObjects.Count;

        #endregion
    }
}