using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class bagbutton : MonoBehaviour
{
    public Sprite closeimage;
    public Sprite openimage;
    public Button bagbtn;
    public GameObject bag;
    bool Isclose = true;
    void Start()
    {
        Close();
    }
    public void Onclick()
    {
        if (Isclose == false)
        {
            Isclose = true;
        }
        else if (Isclose == true)
        {
            Isclose = false;
        }
    }
    void Update()
    {
        if (Isclose == false)
        {
            Open();
        }
        else if (Isclose == true)
        {
            Close();
        }
    }
    public void Open()
    {
        bag.SetActive(true);
        bagbtn.image.sprite= closeimage;
    }
    public void Close()
    {
        bag.SetActive(false);
        bagbtn.image.sprite = openimage;
    }
}
