﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpamGameController : MonoBehaviour
{
    public enum SpamButtonType
    {
        Food,
        Work,
        Sleep
    }

    public Image foodBar;
    public Image workBar;
    public Image sleepBar;

    private const int SLEEP_INCREMENT = 1;
    private const int FOOD_INCREMENT = 1;
    private const int WORK_INCREMENT = 1;

    private const int COMMON_DECREMENT = 1;
    private const int COMMON_HIGH_DECREMENT = 2;

    private int food;
    private int work;
    private int sleep;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnSpamButtonClick(int _type)
    {
        var type = (SpamButtonType)_type;
        switch (type)
        {
            case SpamButtonType.Food:
                sleep -= sleep > 0 ? COMMON_DECREMENT : 0;
                work -= work > 0 ? COMMON_HIGH_DECREMENT : 0;
                food += food + FOOD_INCREMENT < 100 ? FOOD_INCREMENT : 0;
                break;
            case SpamButtonType.Work:
                sleep -= sleep > 0 ? COMMON_HIGH_DECREMENT : 0;
                work += work + WORK_INCREMENT < 100 ? WORK_INCREMENT : 0;
                food -= food > 0 ? COMMON_DECREMENT : 0;
                break;
            case SpamButtonType.Sleep:
                sleep += sleep + SLEEP_INCREMENT <= 100 ? SLEEP_INCREMENT : 0;
                work -= work > 0 ? COMMON_HIGH_DECREMENT : 0;
                food -= food > 0 ? COMMON_DECREMENT : 0;
                break;
            default:
                Debug.Log(type + ": type unknow");
                break;
        }
        UpdateBars();
    }

    private void UpdateBars()
    {
        foodBar.fillAmount = food / 100f;
        workBar.fillAmount = work / 100f;
        sleepBar.fillAmount = sleep / 100f;
    }
}
