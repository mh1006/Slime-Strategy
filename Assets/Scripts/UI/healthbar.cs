using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class healthbar : MonoBehaviour
{
    // Start is called before the first frame update
    public Slider HPStrip;    
    public int HP;
    void Start()
    {
        HPStrip.value = HPStrip.maxValue = HP;    
    }
    public void OnHit(int damage)
    {
        HP -= damage;
        HPStrip.value = HP;    
    }
    public void Heal(int heal)
    {
        HP += heal;
        HPStrip.value = HP;
    }
}
