using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeItems : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        CollectItems collectItems = other.GetComponent<CollectItems>();

        if (collectItems != null)
        {

            collectItems.SlimeCollected();
            gameObject.SetActive(false);

        }
    }
}
