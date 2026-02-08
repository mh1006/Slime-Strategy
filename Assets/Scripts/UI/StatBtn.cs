using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SlimeStrategy
{
    public class StatBtn : MonoBehaviour
    {
        
        StatScreen a;
        public GameObject StatScr;
        

        // Start is called before the first frame update
        void Start()
        {
            
            a = GameObject.Find("Stat_info_Panel").GetComponent<StatScreen>();
            StatScr.SetActive(false);

        }

        // Update is called once per frame
        void Update()
        {
            /*
            if (BM.SelectedUnit != null)
            { 
                UnitIsSelected = true;
                Debug.Log("selected");
            }
            else if (BM.SelectedUnit == null) UnitIsSelected = false;

            if (BM.HoveredUnit !=null && !UnitIsSelected) {
                UnitIsHovered = true; 
                Debug.Log("hovered");
            }
            else if (BM.HoveredUnit == null) UnitIsHovered = false;
            
            */
        }
        public void StatB()
        {
            StatScr.SetActive(true);
        }

        public void WarriorB()
        {
           // a.Warrior();
        }
        public void KnightB()
        {
            //a.Knight();
        }
        public void ArcherB()
        {
            //a.Archer();
        }
        public void MageB()
        {
           // a.Mage();
        }

    }
    /*
             public void Warrior()
        {
            GameObject.Find("Char_Icon").GetComponent<Image>().sprite = Warriorimage;
            _CharnameText.text = "Warrior";
            _ChartypeText.text = "Physical";
            AttackS.value = 15;
            PhysDS.value = 10;
            MagDS.value = 7;
            MovRS.value = 6;
            ActRS.value = 1;
            _AttackText.text = "15";
            _PhysDText.text = "10";
            _MagDText.text = "7";
            _MovRText.text = "6";
            _ActRText.text = "1";
        }
        public void Knight()
        {
            GameObject.Find("Char_Icon").GetComponent<Image>().sprite = Knightimage;
            _CharnameText.text = "Knight";
            _ChartypeText.text = "Physical";
            AttackS.value = 11;
            PhysDS.value = 12;
            MagDS.value = 5;
            MovRS.value = 5;
            ActRS.value = 1;
            _AttackText.text = "11";
            _PhysDText.text = "12";
            _MagDText.text = "5";
            _MovRText.text = "5";
            _ActRText.text = "1";
        }
        public void Archer()
        {
            GameObject.Find("Char_Icon").GetComponent<Image>().sprite = Archerimage;
            _CharnameText.text = "Archer";
            _ChartypeText.text = "Physical";
            AttackS.value = 10;
            PhysDS.value = 8;
            MagDS.value = 9;
            MovRS.value = 5;
            ActRS.value = 3;
            _AttackText.text = "10";
            _PhysDText.text = "8";
            _MagDText.text = "9";
            _MovRText.text = "5";
            _ActRText.text = "3";
        }
        public void Mage()
        {
            GameObject.Find("Char_Icon").GetComponent<Image>().sprite = Mageimage;
            _CharnameText.text = "Mage";
            _ChartypeText.text = "Magical";
            AttackS.value = 14;
            PhysDS.value = 7;
            MagDS.value = 9;
            MovRS.value = 5;
            ActRS.value = 2;
            _AttackText.text = "14";
            _PhysDText.text = "7";
            _MagDText.text = "9";
            _MovRText.text = "5";
            _ActRText.text = "2";
        }
    */
}
/*
 Accessor methods for selected and hovered unit exist now and pushed to your branch,
with checks for null pre-written if you want to use them.
BattleManager.SelectedUnit [May be null]
BattleManager.HoveredUnit [May be null]
bool BattleManager.UnitIsSelected [If checked for this, guaranteed to not be null when accessed] 
bool BattleManager.UnitIsHovered [If checked for this, guaranteed to not be null when accessed] 
First two return the BattleUnit that is selected or hovered, if there is one (null otherwise), 
where you can access 
BattleUnit.Stats, 
public BattleUnit SelectedUnit => _selectedUnit;.RemainingMovement, 
BattleUnit.CurrentHealth, 
etc.
 */