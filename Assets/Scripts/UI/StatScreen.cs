using System;
using System.Collections;
using System.Collections.Generic;
using SlimeStrategy.BattleMap;
using SlimeStrategy.BattleMap.Units;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;

namespace SlimeStrategy
{
    public class StatScreen : MonoBehaviour
    {
        /*Select Hover*/

        [SerializeField] public BattleManager battleManager; //[May be null]

        //BattleManager _hovered; //[May be null]

        /* StatScreen*/

        [SerializeField] private StatScreen statscreen;
        public GameObject StatScr;

        public Sprite Warriorimage;
        public Sprite Knightimage;
        public Sprite Archerimage;
        public Sprite Mageimage;
        public Sprite TypeMagimage;
        public Sprite TypePhyimage;

        /*stat_info*/
        public GameObject CharnameText;
        private TMP_Text _CharnameText;
        public GameObject ChartypeText;
        private TMP_Text _ChartypeText;

        public Slider HealthS;
        public Slider AttackS;
        public Slider PhysDS;
        public Slider MagDS;
        public Slider MovRS;
        public Slider ActRS1;
        public Slider ActRS2;
        public Slider ActRS3;
        public Slider ActRS4;
        public Slider ActRS5;
        public Slider ActRS6;

        public GameObject HealthText;
        public GameObject AttackText;
        public GameObject PhysDText;
        public GameObject MagDText;
        public GameObject MovRText;
        public GameObject ActRText;

        private TMP_Text _HealthText;
        private TMP_Text _AttackText;
        private TMP_Text _PhysDText;
        private TMP_Text _MagDText;
        private TMP_Text _MovRText;
        private TMP_Text _ActRText;

        // Start is called before the first frame update
        void Start()
        {
            StatScr.SetActive(false);
        }

        void Awake()
        {
            _CharnameText = CharnameText.GetComponent<TMP_Text>();
            _ChartypeText = ChartypeText.GetComponent<TMP_Text>();
            _HealthText = HealthText.GetComponent<TMP_Text>();
            _AttackText = AttackText.GetComponent<TMP_Text>();
            _PhysDText = PhysDText.GetComponent<TMP_Text>();
            _MagDText = MagDText.GetComponent<TMP_Text>();
            _MovRText = MovRText.GetComponent<TMP_Text>();
            _ActRText = ActRText.GetComponent<TMP_Text>();
        }

        // Update is called once per frame
        void Update()
        {
            if (battleManager.UnitIsHovered)
            {
                StatShow();
                UpdateStatDisplay(battleManager.HoveredUnit);
            }
            else if (battleManager.UnitIsSelected)
            {
                StatShow();
                UpdateStatDisplay(battleManager.SelectedUnit);
            }
            else
            {
                StatHide();
            }
        }

        public void StatShow()
        {
            StatScr.SetActive(true);
        }

        public void StatHide()
        {
            StatScr.SetActive(false);
        }

        public void UpdateStatDisplay(BattleUnit unit)
        {
            var unitStats = unit.UnitStats;
            
            _CharnameText.text = GetUnitName(unit);
            _ChartypeText.text = GetUnitType(unit); 
            if (unit.UnitClass == StatClass.Warrior)
            {
                GameObject.Find("Char_Icon").GetComponent<Image>().sprite = Warriorimage;
                GameObject.Find("Type_icon").GetComponent<Image>().sprite = TypePhyimage;
            }
            else if (unit.UnitClass == StatClass.Knight)
            {
                GameObject.Find("Char_Icon").GetComponent<Image>().sprite = Knightimage;
                GameObject.Find("Type_icon").GetComponent<Image>().sprite = TypePhyimage;
            }
            else if (unit.UnitClass == StatClass.Archer)
            {
                GameObject.Find("Char_Icon").GetComponent<Image>().sprite = Archerimage;
                GameObject.Find("Type_icon").GetComponent<Image>().sprite = TypePhyimage;
            }
            else if (unit.UnitClass == StatClass.Mage)
            {
                GameObject.Find("Char_Icon").GetComponent<Image>().sprite = Mageimage;
                GameObject.Find("Type_icon").GetComponent<Image>().sprite = TypeMagimage;
            }
            else if (unit.UnitClass == StatClass.King)
            {
                GameObject.Find("Char_Icon").GetComponent<Image>().sprite = Warriorimage;
                GameObject.Find("Type_icon").GetComponent<Image>().sprite = TypeMagimage;
            }


            HealthS.value = unit.CurrentHealth;
            AttackS.value = unitStats.AD;
            PhysDS.value = unitStats.PDefence;
            MagDS.value = unitStats.MDefence;
            MovRS.value = unit.RemainingMovement;
            //ActRS.value = battleManager.SelectedUnit.Stats.ActRange;
            UpdateActionBar(unit);
            //MaxHealth.value = battleManager.SelectedUnit.Stats.MaxHealth;

            _HealthText.text = unit.CurrentHealth.ToString();
            _AttackText.text = unitStats.AD.ToString();
            _PhysDText.text = unitStats.PDefence.ToString();
            _MagDText.text = unitStats.MDefence.ToString();
            _MovRText.text = unit.RemainingMovement.ToString();
            _ActRText.text = unitStats.ActRange.ToString();
        }


        private void UpdateActionBar(BattleUnit unit)
        {
            var unitStats = unit.UnitStats;

            ActRS1.value = ActRS2.value = ActRS3.value = ActRS4.value = ActRS5.value = ActRS6.value = 0;
            switch (unitStats.ActRange)
            {
                case 6:
                    ActRS1.value = ActRS2.value = ActRS3.value = ActRS4.value = ActRS5.value = ActRS6.value = 1;
                    break;
                case 5:
                    ActRS1.value = ActRS2.value = ActRS3.value = ActRS4.value = ActRS5.value = 1;
                    break;
                case 4:
                    ActRS1.value = ActRS2.value = ActRS3.value = ActRS4.value = 1;
                    break;
                case 3:
                    ActRS1.value = ActRS2.value = ActRS3.value = 1;
                    break;
                case 2:
                    ActRS1.value = ActRS2.value = 1;
                    break;
                case 1:
                    ActRS1.value = 1;
                    break;
            }
        }

        private string GetUnitName(BattleUnit unit)
        {
            return unit.UnitClass switch
            {
                StatClass.None => "None",
                StatClass.Warrior => "Warrior",
                StatClass.Knight => "Knight",
                StatClass.Archer => "Archer",
                StatClass.King => "King",
                StatClass.Mage => "Mage",
                _ => "Non-existent"
            };
        }
        
        private string GetUnitType(BattleUnit unit)
        {
            return unit.DamageType switch
            {
                DamageType.None => "None",
                DamageType.Physical => "Physical",
                DamageType.Magical => "Magical",
                _ => "Non-existent"
            };
        }
    }
}