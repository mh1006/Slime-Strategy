using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class keyboard : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            Debug.Log("hurt");
        }

        if (Input.GetKeyDown(KeyCode.X))
        {
            Debug.Log("heal");
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            Debug.Log("add heart");
        }

    }
}
