using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class items : MonoBehaviour
{
    private void OnTriggerEnter(Collider other){
        CollectItems collectItems = other.GetComponent<CollectItems>();

        if (collectItems != null){

            collectItems.ItemsCollected();
            gameObject.SetActive(false);
            
        }
    }
}
