using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Player_inventory : MonoBehaviour
{
    // # change items by the soort item that i want
    //public int NumberOfItems{get; private set;}
    public int NumberOfItems { get; private set; }
    public int SlimesNumber { get; private set; }
    public int FireElements { get; private set; }
    public int WindElements { get; private set; }
    public int RockElements { get; private set; }
    public int WaterElements { get; private set; }
    public Vector3 Current_Position;
    public bool LevelOne = true;

    public void CurrentPositions(Vector3 position_c)
    {
        LevelOne = false;
        Current_Position = position_c;
    }
    public void IncreaseSlimes()
    {
        SlimesNumber += 1;
    }
    public void DecreaseSlimes()
    {
        SlimesNumber -= 1;
    }
    public void IncreaseFire()
    {
        FireElements += 1;
    }
    public void DecreaseFire()
    {
        FireElements -= 1;
    }
    public void IncreaseWind()
    {
        WindElements += 1;
    }
    public void DecreaseWind()
    {
        WindElements -= 1;
    }
    public void IncreaseRock()
    {
        RockElements += 1;
    }
    public void DecreaseRock()
    {
        RockElements -= 1;
    }
    public void IncreaseWater()
    {
        WaterElements += 1;
    }
    public void DecreaseWater()
    {
        WaterElements -= 1;
    }
    public void IncreaseItems()
    {
        NumberOfItems += 1;
    }
    public void DecreaseItems()
    {
        NumberOfItems -= 1;
    }

    //public UnityEvent<Player_inventory> OnItemsCollected;
    //public int items_number;
    //CollictItems collict_items = GetComponent<CollictItems>();

    //public void ItemsCollected()
    //{
    //    NumberOfItems = CollictItems.NumberOfItems;
    //}
    //public void Number_of_items(CollictItems collict_items)
    //{
    //        NumberOfItems = collict_items.NumberOfItems;
    //    //OnItemsCollected.Invoke(this);

    //}
    //private void Update()
    //{
    //    CollictItems collict_items = GetComponent<CollictItems>();
    //    if (collict_items != null)
    //        Number_of_items(collict_items);
    //}
    //void Update()
    //{
    //    items_number = NumberOfItems;
    //}
}
