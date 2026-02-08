using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterItems : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        CollectItems collectItems = other.GetComponent<CollectItems>();

        if (collectItems != null)
        {

            collectItems.WaterCollected();
            gameObject.SetActive(false);

        }
    }
}