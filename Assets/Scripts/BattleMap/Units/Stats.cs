using System;

namespace SlimeStrategy.BattleMap.Units
{
    public class Stats
    {
        private readonly int _maxHealth;
        private readonly int _ad;
        private readonly int _pDefence;
        private readonly int _mDefence;
        private readonly int _movRange;
        private readonly int _actRange;
        public int MaxHealth => _maxHealth;
        public int AD {
            get
            {
                if (!HasModifier) return _ad;
                return _ad + _modifier.AD;
            }
        }
        
        public int PDefence {
            get
            {
                if (!HasModifier) return _pDefence;
                return _pDefence + _modifier.PDefence;
            }
        }
        
        public int MDefence {
            get
            {
                if (!HasModifier) return _mDefence;
                return _mDefence + _modifier.MDefence;
            }
        }
        
        public int MovRange {
            get
            {
                if (!HasModifier) return _movRange;
                return _movRange + _modifier.MovRange;
            }
        }
        
        public int ActRange {
            get
            {
                if (!HasModifier) return _actRange;
                return _actRange + _modifier.ActRange;
            }
        }

        // TODO Make stats be affected by modifier (element)
        private StatsModifier _modifier;
        public StatsModifier Modifier => _modifier;
        public bool HasModifier
        {
            get
            {
                if (ReferenceEquals(_modifier, null)) return false;
                if (Modifier.Type == StatsModifierType.None) return false;
                return true;
            }
        }

        public Stats(int maxHealth, int ad, int pDefence, int mDefence, int movRange, int actRange)
        {
            _maxHealth = maxHealth;
            _ad = ad;
            _pDefence = pDefence;
            _mDefence = mDefence;
            _movRange = movRange;
            _actRange = actRange;
            _modifier = StatsModifier.FromClass(StatsModifierType.None);
        }

        public void SetModifier(StatsModifier modifier)
        {
            _modifier = modifier;
        }

        public static Stats FromClass(StatClass statClass)
        {
            return statClass switch
            {
                StatClass.Warrior => new Stats(20, 13, 5, 3, 5, 1),
                StatClass.Knight => new Stats(20, 11, 8, 2, 4, 1),
                StatClass.Archer => new Stats(20, 11, 3, 6, 4, 3),
                StatClass.Mage => new Stats(20, 12, 3, 5, 4, 2),
                StatClass.King => new Stats(20, 14, 7, 5, 5, 1),
                _ => throw new ArgumentOutOfRangeException(nameof(statClass), statClass, null)
            };
        }
    }

    public enum StatClass
    {
        None = 0,
        // 0 intentionally skipped to catch not-set errors, else it would default to warrior for some reason...
        // WHYY C#? Why is an unset enum == 0??? You're a grown up language, not flippin' JavaScript. Act like it.
        Warrior,
        Knight,
        Archer,
        King,
        Mage
    }
}