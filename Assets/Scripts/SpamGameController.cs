using System.Collections;
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
    public Image timerImage;
    public float timerGameToFinish;

    private float timeRemaining;

    private const int SLEEP_INCREMENT = 5;
    private const int FOOD_INCREMENT = 5;
    private const int WORK_INCREMENT = 5;

    private const int COMMON_DECREMENT = 1;
    private const int COMMON_HIGH_DECREMENT = 2;

    private float food;
    private float work;
    private float sleep;

    // Start is called before the first frame update
    void Start()
    {
        timeRemaining = 0f;
        SetStartValueOfCurrentSituation();
        StartCoroutine(TimerTick());
    }

    void SetStartValueOfCurrentSituation()
    {
        var currentSituationStepStats = GameManager.Instance.GetCharacterSituationStepStats();
        food = currentSituationStepStats.food.startValue;
        work = currentSituationStepStats.work.startValue;
        sleep = currentSituationStepStats.sleep.startValue;
    }

    void FixedUpdate()
    {
        SubstractValueBar(ref food, .1f);
        SubstractValueBar(ref work, .1f);
        SubstractValueBar(ref sleep, .1f);
        UpdateBars();
    }

    void SubstractValueBar(ref float valueBar, float valueToSubstract)
    {
        valueBar -= valueToSubstract;
        if (valueBar <= 0 && !GameManager.Instance.playerIsDead)
        {
            GameManager.Instance.Loose();
        }
    }

    IEnumerator TimerTick()
    {
        UpdateTimer();
        yield return new WaitForSeconds(0.1f);
        timeRemaining += timeRemaining > timerGameToFinish ? 0f : 0.1f;
        StartCoroutine(TimerTick());
        if (timeRemaining > timerGameToFinish)
        {
            GameManager.Instance.Win();
        }
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
    }

    private void UpdateBars()
    {
        foodBar.fillAmount = food / 100f;
        workBar.fillAmount = work / 100f;
        sleepBar.fillAmount = sleep / 100f;
    }

    private void UpdateTimer()
    {
        timerImage.fillAmount = timeRemaining / timerGameToFinish;
    }
}
