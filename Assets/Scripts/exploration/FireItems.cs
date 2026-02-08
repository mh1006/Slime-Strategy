using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireItems : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        CollectItems collectItems = other.GetComponent<CollectItems>();

        if (collectItems != null)
        {

            collectItems.FireCollected();
            gameObject.SetActive(false);

        }
    }
}
