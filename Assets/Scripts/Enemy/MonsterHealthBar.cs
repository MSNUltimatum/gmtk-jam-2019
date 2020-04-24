using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class MonsterHealthBar : MonoBehaviour
{
    public float timeToOff = 1.2f;
    public Image activeHealthBar;
    private void Start()
    {
        Deactive();
        monsterLife = GetComponentInParent<MonsterLife>();
        maxHP = monsterLife.maxHP;
        monsterLife.hpChangedEvent.AddListener(HealthBarChange);
        hpSlider = GetComponent<Slider>();
    }

    private void Update()
    {
        if(currentTimeToOff > 0)
        {
            currentTimeToOff -= Time.deltaTime;
        }
        else
        {
            Deactive();
        }
    }

    public void HealthBarChange()
    {
        Active();
        hpSlider.value = monsterLife.HP / monsterLife.maxHP;
        if(hpSlider.value < 0.33)
        {
            activeHealthBar.color = Color.red;
        }
        currentTimeToOff = timeToOff;
    }

    public void Active()
    {
        for(int i = 0;i < gameObject.transform.childCount; i++)
        {
            if(!transform.GetChild(i).gameObject.activeSelf)
                transform.GetChild(i).gameObject.SetActive(true);
        }
    }

    public void Deactive()
    {
        for (int i = 0; i < gameObject.transform.childCount; i++)
        {
            if (transform.GetChild(i).gameObject.activeSelf == true)
                transform.GetChild(i).gameObject.SetActive(false);
        }
    }
    private MonsterLife monsterLife;
    private Slider hpSlider;

    
    private float currentTimeToOff;
    private float maxHP;
}
