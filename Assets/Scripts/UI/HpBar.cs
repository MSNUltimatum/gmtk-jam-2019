using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class HpBar : MonoBehaviour
{
    public float timeToOff = 1.2f;
    public Image activeHealthBar;

    protected virtual void Start()
    {
        Deactive();
        hpSlider = GetComponent<Slider>();
    }

    private void Update()
    {
        if (currentTimeToOff > 0)
        {
            currentTimeToOff -= Time.deltaTime;
        }
        else
        {
            Deactive();
        }
    }

    protected abstract Vector2 GetCurrentMaxHp();

    public void HealthBarChange()
    {
        var currentHP = GetCurrentMaxHp();
        Active();
        hpSlider.value = currentHP.x / currentHP.y;
        if (hpSlider.value < 0.33)
        {
            activeHealthBar.color = Color.red;
        }
        currentTimeToOff = timeToOff;
    }

    public void Active()
    {
        for (int i = 0; i < gameObject.transform.childCount; i++)
        {
            if (!transform.GetChild(i).gameObject.activeSelf)
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

    private Slider hpSlider;

    private float currentTimeToOff;
}
