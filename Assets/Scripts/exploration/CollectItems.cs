using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CollectItems : MonoBehaviour
{
    // # change items by the soort item that i want
    public int NumberOfItems { get; private set; }
    public int SlimesNumber { get; private set; }
    public int FireElements { get; private set; }
    public int WindElements { get; private set; }
    public int RockElements { get; private set; }
    public int WaterElements { get; private set; }

    //public UnityEvent<CollictItems> OnItemsCollected;
    Player_inventory player_Inventory;
    GameObject gameObject;

    void Awake()
    {
        gameObject = GameObject.Find("inventory");
        if (gameObject != null)
            player_Inventory = gameObject.GetComponent<Player_inventory>();

    }

    //private void Update()
    //{
    //    player_Inventory.NumberOfItems;
    //}
    public void ItemsCollected()
    {
        NumberOfItems++;
        if (player_Inventory != null)
            player_Inventory.IncreaseItems();
        //OnItemsCollected.Invoke(this);
    }
    public void SlimeCollected()
    {
        SlimesNumber++;
        if (player_Inventory != null)
            player_Inventory.IncreaseSlimes();
        //OnItemsCollected.Invoke(this);
    }
    public void FireCollected()
    {
        FireElements++;
        if (player_Inventory != null)
            player_Inventory.IncreaseFire();
        //OnItemsCollected.Invoke(this);
    }
    public void WindCollected()
    {
        WindElements++;
        if (player_Inventory != null)
            player_Inventory.IncreaseWind();
        //OnItemsCollected.Invoke(this);
    }
    public void RockCollected()
    {
        RockElements++;
        if (player_Inventory != null)
            player_Inventory.IncreaseRock();
        //OnItemsCollected.Invoke(this);
    }
    public void WaterCollected()
    {
        WaterElements++;
        if (player_Inventory != null)
            player_Inventory.IncreaseWater();
        //OnItemsCollected.Invoke(this);
    }
}
