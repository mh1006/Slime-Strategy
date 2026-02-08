using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UI_inventory : MonoBehaviour
{
    private TextMeshProUGUI items_num;
    Player_inventory player_Inventory;
    GameObject gameObject;

    void Awake()
    {
        gameObject = GameObject.Find("inventory");
        if (gameObject != null)
            player_Inventory = gameObject.GetComponent<Player_inventory>();

    }
    // Start is called before the first frame update
    void Start()
    {
        items_num = GetComponent<TextMeshProUGUI>();
    }

    private void Update()
    {
        if (player_Inventory != null)
            items_num.text = player_Inventory.NumberOfItems.ToString();
        //Debug.Log(player_Inventory.NumberOfItems);
    }
    

    //public void UpdateItemsText(Player_inventory player_inventory)
    //{
    //    items_num.text = player_inventory.NumberOfItems.ToString();
    //}
    //private void Update()
    //{
    //    Player_inventory player_Inventory = GetComponent< Player_inventory>();
    //    if (player_Inventory!= null)
    //        UpdateItemsText(player_Inventory);
    //}

}
