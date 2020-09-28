using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using static SpamGameController;

public enum ChildScene
{
    Menu = 1,
    Options = 2,
    CharacterDetails = 3,
    SpamGame = 4,
    ThrowGame = 5,
    CharacterSituation = 6,
}

public class GameManager : MonoBehaviour
{
    public PlayerDetails playerDetails;

    public List<SpamButtonType> slowBonuses = new List<SpamButtonType>();

    public bool playerIsDead = false;
    public CharacterFunFacts funFactsData;
    public CharacterNames namesData;
    public Steps stepsData;
    private static GameManager _instance;
    #region singleton
    public static GameManager Instance
    {
        get => _instance;
    }

    void Awake()
    {

        if (_instance == null)
        {

            _instance = this;
            DontDestroyOnLoad(this.gameObject);

            //Rest of your Awake code

        }
        else
        {
            Destroy(this);
        }
    }
    #endregion singleton

    // Start is called before the first frame update
    void Start()
    {
        SceneManager.LoadScene((int)ChildScene.Menu, LoadSceneMode.Additive);
        LoadDataJson();
        LoadSave();
    }

    private void LoadDataJson()
    {
        var sRaw = Resources.Load<TextAsset>("StepsData");
        GameManager.Instance.stepsData = JsonUtility.FromJson<Steps>(sRaw.text);
    }

    private void LoadSave()
    {
        GameManager.Instance.playerDetails = new PlayerDetails();
    }

    public Stats GetCharacterSituationStepStats()
    {
        if (GameManager.Instance.playerDetails.situtation <= PlayerDetails.Situtation.HOMELESS_HIGH)
        {
            return GameManager.Instance.stepsData.homeless.stats;
        }
        else if (GameManager.Instance.playerDetails.situtation <= PlayerDetails.Situtation.PEASANT_HIGH)
        {
            return GameManager.Instance.stepsData.peasant.stats;
        }
        else if (GameManager.Instance.playerDetails.situtation <= PlayerDetails.Situtation.RICH_HIGH)
        {
            return GameManager.Instance.stepsData.rich.stats;
        }
        else if (GameManager.Instance.playerDetails.situtation <= PlayerDetails.Situtation.KING_HIGH)
        {
            return GameManager.Instance.stepsData.king.stats;
        }
        return null;
    }

    public void Win()
    {
        GameManager.Instance.playerDetails.situtation++;
        LoadScene(ChildScene.CharacterSituation);
    }

    public void LoadScene(ChildScene scene)
    {
        SceneManager.LoadScene((int)scene);
    }

    public void Loose()
    {
        GameManager.Instance.playerDetails.situtation = PlayerDetails.Situtation.HOMELESS_LOW;
        GameManager.Instance.playerDetails.yearCountSurvive = 0;
        GameManager.Instance.playerDetails.generationCountSurvive++;
        GameManager.Instance.playerIsDead = true;
        GameManager.Instance.LoadScene(ChildScene.CharacterDetails);
    }
}
