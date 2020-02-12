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
        originalScale = activeHealthBar.transform.localScale.y;
        Deactive();
        monsterLife = GetComponentInParent<MonsterLife>();
        maxHP = monsterLife.maxHP;
        monsterLife.hpChangedEvent.AddListener(HealthBarChange);
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
        int tmpHP = monsterLife.HP;
        Vector3 tmpScale = activeHealthBar.transform.localScale;
        tmpScale.y = (float)tmpHP / maxHP * originalScale;
        if(tmpScale.y / originalScale * 100 < 50.0f)
        {
            activeHealthBar.color = Color.red;
        }
        activeHealthBar.transform.localScale = tmpScale;
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

    private float originalScale;
    private float currentTimeToOff;
    private int maxHP;
}
