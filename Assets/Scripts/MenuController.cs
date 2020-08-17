using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    private GameManager gameManager;
    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameManager.Instance;
    }

    public void LaunchGame()
    {
        gameManager.LoadScene(ChildScenes.CharacterDetails);
    }

    public void Options()
    {
        gameManager.LoadScene(ChildScenes.Options);
    }
}
