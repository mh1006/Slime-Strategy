using SlimeStrategy.BattleMap.Units;
using UnityEngine;

namespace SlimeStrategy
{
    
    public class HealthBarUI : MonoBehaviour
    {
        [SerializeField] private BattleUnit unit;

        [SerializeField] private RectTransform healthRect;
        [SerializeField] private RectTransform underlayRect;

        private float _healthRectMaxWidth;

        private int _currentHealth;
        private int _previousHealth;
        private bool _animating;
        private float _animationTimer;
        private const float AnimationDuration = 1.0f;

        private void Start()
        {
            _healthRectMaxWidth = healthRect.rect.width;

            _currentHealth = unit.CurrentHealth;
            _previousHealth = unit.CurrentHealth;
        }
        
        private void Update()
        {
            if (unit.CurrentHealth != _currentHealth)
            {
                _animationTimer = Time.time;
                _previousHealth = _currentHealth;
                _currentHealth = unit.CurrentHealth;
            }
            // (Current time - move start time) / moveDuration should be [0-1] move completion 
            float t = (Time.time - _animationTimer) / AnimationDuration;
            float tMain = Mathf.Clamp(t, 0, 1);
            float tDelay = Mathf.Clamp(t - (AnimationDuration / 4), 0, 1);

            var currentHealthPercent = (float)_currentHealth / unit.UnitStats.MaxHealth;
            var previousHealthPercent = (float)_previousHealth / unit.UnitStats.MaxHealth;
            var widthFrom = _healthRectMaxWidth * previousHealthPercent;
            var widthTarget = _healthRectMaxWidth * currentHealthPercent;

            float healthWidth = Mathf.Lerp(widthFrom, widthTarget, AnimationCurve(tMain));
            // Debug.Log(healthWidth);
            float underlayWidth = Mathf.Lerp(widthFrom, widthTarget, AnimationCurve(tDelay));
            
            healthRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, healthWidth);
            underlayRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, underlayWidth);
        }

        private float AnimationCurve(float t)
        {
            return Mathf.Clamp(Mathf.Pow(t, 2), 0, 1);
        }
    }
}
