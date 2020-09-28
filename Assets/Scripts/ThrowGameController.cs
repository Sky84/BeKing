using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static SpamGameController;

public class ThrowGameController : MonoBehaviour
{
    public float timeBeforeSpawn;
    private GameObject currentProjectile;

    void Start()
    {
        currentProjectile = GameObject.FindGameObjectWithTag("projectile");
    }

    IEnumerator RespawnIfPossible(GameObject nextProjectile)
    {
        if (nextProjectile)
        {
            yield return new WaitForSeconds(timeBeforeSpawn);
            nextProjectile.SetActive(true);
            currentProjectile = nextProjectile;
        }
    }
    public void OnBarButtonClick(int _type)
    {
        var projectileCtrl = currentProjectile.GetComponent<ProjectileController>();
        projectileCtrl.selectedBar = (SpamButtonType)_type;
        for (int i = 0; i < projectileCtrl.throwBarButtons.Count; i++)
        {
            var button = projectileCtrl.throwBarButtons[i].GetComponent<Button>();
            if (button.gameObject.name.Contains(_type.ToString()))
            {
                button.interactable = false;
            }
            else
            {

                button.interactable = true;
            }
        }
    }
}
