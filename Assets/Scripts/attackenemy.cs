using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class attackenemy : MonoBehaviour
{
    public GameObject attackdia;
    public GameObject endturndia;
    /**/
    public Slider HPStrip;
    public int HP;
    /**/
    bool canattack = false;
    bool canendturn = false;
    // Start is called before the first frame update
    void Start()
    {
        attackdia.SetActive(false);
        endturndia.SetActive(false);
        HPStrip.value = HPStrip.maxValue = HP;
    }
    void Update()
    {

        if (canattack == true)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                //attack
                Debug.Log("Attack");
                attackdia.SetActive(false);
                canattack = false;
                endturndia.SetActive(true);
                canendturn = true;
                /**/
                OnHit(10);
            }
            if (Input.GetKeyDown(KeyCode.Z))
            {
                //exit
                Debug.Log("Exit");
                attackdia.SetActive(false);
                canattack = false;
                endturndia.SetActive(true);
                canendturn = true;
            }
        }
        if (canendturn == true)
        {
            if (Input.GetMouseButtonDown(1))
            {
                endturndia.SetActive(false);
                //end turn
                canendturn = false;
            }
        }
        // Update is called once per frame
    }
    void OnMouseDown()
    {
        Debug.Log("Mouse click on this object");
        if (canendturn == false)
        {
            attackdia.SetActive(true);
            canattack = true;
        }

    }
    public void OnHit(int damage)
    {
        HP -= damage;
        HPStrip.value = HP;
    }
    /*void selectacctack()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            //attack
            Debug.Log("Attack");
            canattack = false;
        }
        if (Input.GetKeyDown(KeyCode.Z))
        {
            //exit
            Debug.Log("Exit");
            attackdia.SetActive(false);
            canattack = false;
        }
    }*/

}
