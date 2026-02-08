using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindItems : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        CollectItems collectItems = other.GetComponent<CollectItems>();

        if (collectItems != null)
        {

            collectItems.WindCollected();
            gameObject.SetActive(false);

        }
    }
}
