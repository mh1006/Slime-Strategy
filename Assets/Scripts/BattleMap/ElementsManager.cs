using System;
using SlimeStrategy.BattleMap.Units;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace SlimeStrategy.BattleMap
{
    public class ElementsManager : MonoBehaviour
    {
        [SerializeField] private Button waterButton;
        [SerializeField] private Button earthButton;
        [SerializeField] private Button fireButton;
        [SerializeField] private Button airButton;
        [SerializeField] private TextMeshProUGUI waterRemainingText;
        [SerializeField] private TextMeshProUGUI earthRemainingText;
        [SerializeField] private TextMeshProUGUI fireRemainingText;
        [SerializeField] private TextMeshProUGUI airRemainingText;

        private BattleManager _battleManager;
        private Player_inventory _inventory;

        private int _waterElements;
        private int _earthElements;
        private int _fireElements;
        private int _airElements;

        private bool ApplyElement(StatsModifierType element)
        {
            if (!_battleManager.UnitIsSelected) return false;

            return _battleManager.Unit_ApplyElementAction(element);
        }

        public void Button_Water()
        {
            if (_waterElements > 0)
            {
                if (ApplyElement(StatsModifierType.WaterElement))
                    _waterElements -= 1;
            }

            Update_WaterButton();
        }

        public void Button_Earth()
        {
            if (_earthElements > 0)
            {
                if (ApplyElement(StatsModifierType.EarthElement))
                    _earthElements -= 1;
            }

            Update_EarthButton();
        }

        public void Button_Fire()
        {
            if (_fireElements > 0)
            {
                if (ApplyElement(StatsModifierType.FireElement))
                    _fireElements -= 1;
            }

            Update_FireButton();
        }

        public void Button_Air()
        {
            if (_airElements > 0)
            {
                if (ApplyElement(StatsModifierType.AirElement))
                    _airElements -= 1;
            }

            Update_AirButton();
        }

        private void Start()
        {
            _battleManager = GameObject.FindWithTag("BattleManager").GetComponent<BattleManager>();
            // Try to get the inventory from the exploration mode
            try
            {
                _inventory = GameObject.FindWithTag("ExploreInventory").GetComponent<Player_inventory>();

                _waterElements = _inventory.WaterElements;
                _earthElements = _inventory.RockElements;
                _fireElements = _inventory.FireElements;
                _airElements = _inventory.WindElements;
            }
            catch (NullReferenceException)
            {
                Debug.LogError(
                    "<color=red>Started battle but could not find inventory</color>. Inventory will default to preset");
                var scene = SceneManager.GetActiveScene().name;
                (_waterElements, _earthElements, _fireElements, _airElements) = scene switch
                {
                    "Battle1" => (0, 0, 1, 1),
                    "Battle2" => (0, 1, 1, 1),
                    "Battle3" => (1, 1, 1, 1),
                    _ => (0, 0, 0, 0)
                };
            }

            Update_WaterButton();
            Update_EarthButton();
            Update_FireButton();
            Update_AirButton();

            Debug.Log($"Inventory: [W:{_waterElements}, E:{_earthElements}, F:{_fireElements}, A:{_airElements}]");
        }

        private void Update_WaterButton()
        {
            waterRemainingText.text = _waterElements.ToString();
            if (_waterElements == 0)
            {
                waterButton.interactable = false;
            }
        }

        private void Update_EarthButton()
        {
            earthRemainingText.text = _earthElements.ToString();
            if (_earthElements == 0)
            {
                earthButton.interactable = false;
            }
        }

        private void Update_FireButton()
        {
            fireRemainingText.text = _fireElements.ToString();
            if (_fireElements == 0)
            {
                fireButton.interactable = false;
            }
        }

        private void Update_AirButton()
        {
            airRemainingText.text = _airElements.ToString();
            if (_airElements == 0)
            {
                airButton.interactable = false;
            }
        }
    }
}