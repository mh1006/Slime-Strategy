using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class next_level : MonoBehaviour
{
    //Player_inventory playerInventory;

    // Start is called before the first frame update
    //public void update()
    //{
    //    if (playerInventory.NumberOfItems.ToString() == "2")
    //    {
    //        SceneManager.LoadScene("testing_scene");
    //    }
    //}
    //private TextMeshProUGUI items_num;
    //Start is called before the first frame update
    // void Start()
    //{
    //    items_num = GetComponent<TextMeshProUGUI>();
    //}
    Player_inventory player_Inventory;
    GameObject gameObject;

    void Awake()
    {
        gameObject = GameObject.Find("inventory");
        if (gameObject != null)
            player_Inventory = gameObject.GetComponent<Player_inventory>();

    }

    private void Update()
    {
        int i = SceneManager.GetActiveScene().buildIndex;
        if (player_Inventory.NumberOfItems == 2)
        {
            SceneManager.LoadScene(i + 1);
        }
    }

    //public void UpdateItems(Player_inventory player_inventory)
    //{
    //    int i = SceneManager.GetActiveScene().buildIndex;
    //    if (player_inventory.NumberOfItems==7){
    //        SceneManager.LoadScene(i+1);
    //    }
    //}
    
}
