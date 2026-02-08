using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockItems : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        CollectItems collectItems = other.GetComponent<CollectItems>();

        if (collectItems != null)
        {

            collectItems.RockCollected();
            gameObject.SetActive(false);

        }
    }
}
