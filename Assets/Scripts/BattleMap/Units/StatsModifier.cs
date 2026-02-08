using System;

namespace SlimeStrategy.BattleMap.Units
{
    public class StatsModifier
    {
        // TODO Make stats modifier (elements)
        private readonly int _maxHealth;
        private readonly int _ad;
        private readonly int _pDefence;
        private readonly int _mDefence;
        private readonly int _movRange;
        private readonly int _actRange;
        public int MaxHealth => _maxHealth;
        public int AD => _ad;
        public int PDefence => _pDefence;
        public int MDefence => _mDefence;
        public int MovRange => _movRange;
        public int ActRange => _actRange;
        public StatsModifierType Type { get; private set; }

        public StatsModifier(StatsModifierType type, int maxHealth, int ad, int pDefence, int mDefence, int movRange, int actRange)
        {
            _maxHealth = maxHealth;
            _ad = ad;
            _pDefence = pDefence;
            _mDefence = mDefence;
            _movRange = movRange;
            _actRange = actRange;
            Type = type;
        }

        public static StatsModifier FromClass(StatsModifierType modifierElement)
        {
            return modifierElement switch
            {
                StatsModifierType.None => new StatsModifier(modifierElement, 0, 0, 0, 0, 0, 0),
                StatsModifierType.WaterElement => new StatsModifier(modifierElement, 0, 0, 0, 3, -1, 0),
                StatsModifierType.EarthElement => new StatsModifier(modifierElement, 0, 0, 3, 0, -1, 0),
                StatsModifierType.FireElement => new StatsModifier(modifierElement, 0, 3, -2, 0, 0, 0),
                StatsModifierType.AirElement => new StatsModifier(modifierElement, 0, -2, 0, 0, 2, 0),
                _ => throw new ArgumentOutOfRangeException(nameof(modifierElement), modifierElement, null)
            };
        }
    }


    public enum StatsModifierType
    {
        None,
        WaterElement,
        EarthElement,
        FireElement,
        AirElement
    }
}