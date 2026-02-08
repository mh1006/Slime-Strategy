using System;
using SlimeStrategy.BattleMap.Grid;
using UnityEngine;

namespace SlimeStrategy.BattleMap.Units
{
    [RequireComponent(typeof(GridObject))]
    public class BattleUnit : MonoBehaviour
    {
        [SerializeField] private StatClass unitClass;
        [SerializeField] private UnitFaction unitFaction;
        [SerializeField] private Animator animator;
        
        private BattleManager _battleManager;

        public UnitFaction UnitFaction => unitFaction;
        public Stats UnitStats { get; private set; }
        public StatClass UnitClass => unitClass;

        public DamageType DamageType { get; private set; }

        public int CurrentHealth { get; private set; }
        public int RemainingMovement { get; private set; }

        private bool _isDead;
        private bool _hasAttacked;
        private bool _isSelected;

        private GridObject _gridObject;
        public GridSpace Space => _gridObject.Space;
        public GridCoordinate Coordinate => _gridObject.Coordinate;

        public bool HasAttacked => _hasAttacked;

        private void TakeDamage(int damage, DamageType type)
        {
            int finalDamage = CalculateDamage(damage, type);
            finalDamage = Math.Clamp(finalDamage, 0, int.MaxValue);

            if (finalDamage >= CurrentHealth)
            {
                _isDead = true;
                // TODO Probably bad
                Destroy(gameObject, 2.0f);
            }

            CurrentHealth = Math.Clamp(CurrentHealth - finalDamage, 0, UnitStats.MaxHealth);
        }

        public bool Attack(BattleUnit other)
        {
            if (HasAttacked)
            {
                Debug.LogError("Unit tried to attack but has already attacked this turn.");
                return false;
            }

            other.TakeDamage(UnitStats.AD, DamageType);
            _hasAttacked = true;
            return true;
        }

        public int CalculateDamage(int damage, DamageType type)
        {
            return type switch
            {
                DamageType.Physical => damage - UnitStats.PDefence,
                DamageType.Magical => damage - UnitStats.MDefence,
                _ => throw new ArgumentOutOfRangeException()
            };
        }

        public bool Move(GridSpace space, int distance)
        {
            if (distance > RemainingMovement)
            {
                Debug.LogError("Unit tried to move further than its remaining movement points");
                return false;
            }

            RemainingMovement -= distance;
            _gridObject.Move(space);
            return true;
        }

        public bool ApplyElement(StatsModifierType element)
        {
            if (UnitStats.HasModifier) return false;
            
            var modifier = StatsModifier.FromClass(element);
            if (modifier.MovRange != 0)
                RemainingMovement += modifier.MovRange;
            UnitStats.SetModifier(modifier);
            
            // Handle unit visuals

            var newController = AnimationControllers.Get(unitClass, UnitStats.Modifier.Type);
            if (ReferenceEquals(newController, null))
            {
                Debug.LogError($"Could not find animator for class: {unitClass}, element: {UnitStats.Modifier.Type}");
                return true;
            }
            animator.runtimeAnimatorController = AnimationControllers.Get(unitClass, UnitStats.Modifier.Type);
            
            return true;
        }

        public void ResetForNewTurn()
        {
            RemainingMovement = UnitStats.MovRange;
            _hasAttacked = false;
            // TODO Handle new turn
        }

        public bool IsEnemy(BattleUnit other)
        {
            return IsEnemy(other.unitFaction);
        }

        public bool IsEnemy(UnitFaction otherUnitFaction)
        {
            if (this.unitFaction == UnitFaction.None)
            {
                Debug.LogWarning("BattleUnit has no faction set");
                return false;
            }
            if (otherUnitFaction == UnitFaction.None)
            {
                Debug.LogWarning("BattleUnit has no faction set");
                return false;
            }
            
            if (this.unitFaction == otherUnitFaction) return false;
            return true;
        }

        #region Plumbing

        public void Select()
        {
            // Currently unused, but might be used for UI
            _isSelected = true;
        }

        public void Deselect()
        {
            // Currently unused, but might be used for UI
            _isSelected = false;
        }
        
        private void Awake()
        {
            UnitStats = Stats.FromClass(unitClass);
            _gridObject = GetComponent<GridObject>();
            RemainingMovement = UnitStats.MovRange;
            CurrentHealth = UnitStats.MaxHealth;
            _hasAttacked = false;
            
            DamageType = unitClass is StatClass.Mage or StatClass.King ? DamageType.Magical : DamageType.Physical;
        }

        private void Update()
        {
        }

        private void Start()
        {
            _battleManager = GameObject.FindWithTag("BattleManager").GetComponent<BattleManager>();
            if (ReferenceEquals(_battleManager, null))
            {
                Debug.LogError("BattleUnit could not find BattleManager on Start");
            }
        }

        #endregion
    }

    public enum DamageType
    {
        None = 0,
        Physical,
        Magical
    }
    
    public enum UnitFaction
    {
        None = 0,
        PlayerFaction,
        EnemyFaction
    }

    internal class AnimationControllers
    {
        public static RuntimeAnimatorController Get(StatClass statClass, StatsModifierType element)
        {
            var className = statClass switch {
                StatClass.Warrior => "Warrior",
                StatClass.Knight => "Knight",
                StatClass.Archer => "Archer",
                StatClass.King => "King",
                StatClass.Mage => "Mage",
                _ => throw new ArgumentOutOfRangeException(nameof(statClass), statClass, null)
            };

            var elementName = element switch
            {
                StatsModifierType.None => "Green",
                StatsModifierType.WaterElement => "Water",
                StatsModifierType.EarthElement => "Earth",
                StatsModifierType.FireElement => "Fire",
                StatsModifierType.AirElement => "Air",
                _ => throw new ArgumentOutOfRangeException(nameof(element), element, null)
            };

            var resourceString = $"Animation/Slime/{className}/{elementName}/{className}_{elementName}_Controller";
            return Resources.Load(resourceString) as RuntimeAnimatorController;
        }
    }
}