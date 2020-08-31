using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class CharacterSituationController : MonoBehaviour
{
    Steps stepsData;

    public Image imageSituation;
    public float secondsBeforeLoadnextStep;
    // Start is called before the first frame update
    void Start()
    {
        var sRaw = Resources.Load<TextAsset>("StepsData");
        stepsData = JsonUtility.FromJson<Steps>(sRaw.text);
        SetStatusCharacter();
        StartCoroutine(LoadNextGameStep());
    }

    void SetStatusCharacter()
    {
        string currentSituation = ""; 
        switch (GameManager.Instance.playerDetails.situtation)
        {
            case PlayerDetails.Situtation.HOMELESS_LOW:
                currentSituation = stepsData.homeless.situation_images_path.low;
                break;
            case PlayerDetails.Situtation.HOMELESS_MID:
                currentSituation = stepsData.homeless.situation_images_path.mid;
                break;
            case PlayerDetails.Situtation.HOMELESS_HIGH:
                currentSituation = stepsData.homeless.situation_images_path.high;
                break;
            case PlayerDetails.Situtation.PEASANT_LOW:
                currentSituation = stepsData.peasant.situation_images_path.low;
                break;
            case PlayerDetails.Situtation.PEASANT_MID:
                currentSituation = stepsData.peasant.situation_images_path.mid;
                break;
            case PlayerDetails.Situtation.PEASANT_HIGH:
                currentSituation = stepsData.peasant.situation_images_path.high;
                break;
            case PlayerDetails.Situtation.RICH_LOW:
                currentSituation = stepsData.rich.situation_images_path.low;
                break;
            case PlayerDetails.Situtation.RICH_MID:
                currentSituation = stepsData.rich.situation_images_path.mid;
                break;
            case PlayerDetails.Situtation.RICH_HIGH:
                currentSituation = stepsData.rich.situation_images_path.high;
                break;
            case PlayerDetails.Situtation.KING_LOW:
                currentSituation = stepsData.king.situation_images_path.low;
                break;
            case PlayerDetails.Situtation.KING_MID:
                currentSituation = stepsData.king.situation_images_path.mid;
                break;
            case PlayerDetails.Situtation.KING_HIGH:
                currentSituation = stepsData.king.situation_images_path.high;
                break;
            default:
                break;
        }
        var texture = new Texture2D(2, 2);
        texture.LoadImage(System.IO.File.ReadAllBytes(Application.streamingAssetsPath + "/" + currentSituation));
        imageSituation.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f), 100f);
        imageSituation.SetNativeSize();
    }

    IEnumerator LoadNextGameStep(){
        yield return new WaitForSeconds(secondsBeforeLoadnextStep);
        GameManager.Instance.LoadScene(ChildScenes.CharacterDetails);
    }
}
