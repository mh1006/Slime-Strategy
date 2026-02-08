using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace SlimeStrategy
{
    public class inventory : MonoBehaviour
    {
        [SerializeField] private Player_inventory player_Inventory;
        // Start is called before the first frame update
        public GameObject FireElementText;
        public GameObject WindElementText;
        public GameObject RockElementText;
        public GameObject WaterElementText;

        private TMP_Text _FireElementText;
        private TMP_Text _WindElementText;
        private TMP_Text _RockElementText;
        private TMP_Text _WaterElementText;


        void Awake()
        {
            //player_Inventory = GameObject.Find("inventory").GetComponent<Player_inventory>();
            _FireElementText = FireElementText.GetComponent<TMP_Text>();
            _WindElementText = WindElementText.GetComponent<TMP_Text>();
            _RockElementText = RockElementText.GetComponent<TMP_Text>();
            _WaterElementText = WaterElementText.GetComponent<TMP_Text>();
        }
        void Start()
        {
        }
        // Update is called once per frame
        void Update()
        {
            UpdateInventoryDisplay();
        }
        public void UpdateInventoryDisplay()
        {
            _FireElementText.text = player_Inventory.FireElements.ToString();
            _WindElementText.text = player_Inventory.WindElements.ToString();
            _RockElementText.text = player_Inventory.RockElements.ToString();
            _WaterElementText.text = player_Inventory.WaterElements.ToString();
        }

    }
}
/*
//first you need to call the inventory
Player_inventory player_Inventory;
//then in Awake function (this should be in the first of the script)
player_Inventory = GameObject.Find("inventory").GetComponent<Player_inventory>();
//then at last use the variable you want by typeing
//(in the function that you want) 
player_Inventory.var;
//var is in player inventory is one of those (SlimesNumber, FireElements, WindElements, RockElement, WaterElements)
//and don't forget (i don't know if you want this in you part) if the element are used then to call the function that make the element less (DecreaseSlimes(), DecreaseFire(), ...)
//and if there is any thing not clear then send me a message
 */