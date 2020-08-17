using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum ChildScenes
{
    Menu = 1,
    Options = 2,
    CharacterDetails = 3,
    MainGame = 4,
}

public class GameManager : MonoBehaviour
{
    public int SpamStepBeforeTrasition = 4;
    public PlayerDetails playerDetails;
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
        SceneManager.LoadScene((int)ChildScenes.Menu, LoadSceneMode.Additive);
        LoadSave();
    }

    private void LoadSave()
    {
        GameManager.Instance.playerDetails = new PlayerDetails();
    }

    public void LoadScene(ChildScenes scene)
    {
        SceneManager.LoadScene((int)scene);
    }

}
