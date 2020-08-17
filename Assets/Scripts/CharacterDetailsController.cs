using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine.Windows;

public class CharacterDetailsController : MonoBehaviour
{
    public List<Text> funFactTexts;
    public GameObject genrePanel;

    public Image frontHair;
    public Image eyes;
    public Image face;
    public Image clothes;
    public Image skin;
    public Image backHair;

    private FunFacts funFacts;
    // Start is called before the first frame update
    void Start()
    {
        genrePanel.SetActive(false);
        if (GameManager.Instance.playerDetails.genre == null)
        {
            genrePanel.SetActive(true);
            GameObject.Find("Canvas/GenrePanel/ValidateGenreButton").SetActive(false);
        }

        var ffData = Resources.Load<TextAsset>("FunFactsData");
        funFacts = JsonUtility.FromJson<FunFacts>(ffData.text);
        for (int i = 0; i < funFactTexts.Capacity; i++)
        {
            Text text = funFactTexts[i];
            text.text = GetFunFact();
        }
    }

    public void OnClickReadyButton()
    {
        if (GameManager.Instance.playerDetails.genre != null)
        {
            GameManager.Instance.LoadScene(ChildScenes.MainGame);
        }
    }

    string GetFunFact()
    {
        var randomAction = funFacts.action[Random.Range(0, funFacts.action.Capacity)];
        var randomSubject = funFacts.subject[Random.Range(0, funFacts.subject.Capacity)];
        var randomCompletion = funFacts.completion[Random.Range(0, funFacts.completion.Capacity)];
        var randomVenue = funFacts.venue[Random.Range(0, funFacts.venue.Capacity)];

        return randomAction + " " + randomSubject + " " + randomCompletion + " " + randomVenue;
    }

    void GetRandomCharacter()
    {
        var characterGenre = GameManager.Instance.playerDetails.genre;
        Texture2D clotheTex = new Texture2D(2,2);
        //GetRandomFileByPath("Human_Parts/" + characterGenre + "/Clothes").Current.text.LoadImageIntoTexture(clotheTex);
        //clothes.sprite = Sprite.Create(clotheTex, new Rect(0, 0, clotheTex.width, clotheTex.height), new Vector2(0.5f, 0.5f), 100f, 0, SpriteMeshType.FullRect);
        //clothes.SetNativeSize();

        //Texture2D eyesTex = new Texture2D(2, 2);
        //GetRandomFileByPath("Human_Parts/" + characterGenre + "/Eyes").Current.LoadImageIntoTexture(eyesTex);
        //eyes.sprite = Sprite.Create(eyesTex, new Rect(0, 0, eyesTex.width, eyesTex.height), new Vector2(0.5f, 0.5f), 100f, 0, SpriteMeshType.FullRect);
        //eyes.SetNativeSize();

        //Texture2D faceTex = new Texture2D(2, 2);
        //GetRandomFileByPath("Human_Parts/" + characterGenre + "/Faces").Current.LoadImageIntoTexture(faceTex);
        //face.sprite = Sprite.Create(faceTex, new Rect(0, 0, faceTex.width, faceTex.height), new Vector2(0.5f, 0.5f), 100f, 0, SpriteMeshType.FullRect);
        //face.SetNativeSize();

        //Texture2D hairBackTex = new Texture2D(2, 2);
        //GetRandomFileByPath("Human_Parts/" + characterGenre + "/HairBack").Current.LoadImageIntoTexture(hairBackTex);
        //backHair.sprite = Sprite.Create(hairBackTex, new Rect(0, 0, hairBackTex.width, hairBackTex.height), new Vector2(0.5f, 0.5f), 100f, 0, SpriteMeshType.FullRect);
        //backHair.SetNativeSize();

        //Texture2D hairFrontTex = new Texture2D(2, 2);
        //GetRandomFileByPath("Human_Parts/" + characterGenre + "/HairFront").Current.LoadImageIntoTexture(hairFrontTex);
        //frontHair.sprite = Sprite.Create(hairFrontTex, new Rect(0, 0, hairFrontTex.width, hairFrontTex.height), new Vector2(0.5f, 0.5f), 100f, 0, SpriteMeshType.FullRect);
        //frontHair.SetNativeSize();

        Texture2D skinTex = new Texture2D(50, 50);
        StartCoroutine(GetRandomFileByPath("Human_Parts/neutral/skin", (byte[] imgBytes) => {
            print(imgBytes.Length);
            skinTex.LoadRawTextureData(imgBytes);
            skinTex.Apply();
            skin.sprite = Sprite.Create(skinTex, new Rect(0, 0, skinTex.width, skinTex.height), new Vector2(0.5f, 0.5f), 100f);
            skin.SetNativeSize();
        }));
        
    }

    IEnumerator GetRandomFileByPath(string path, System.Action<byte[]> callback)
    {
        var fileInfo = new System.IO.DirectoryInfo(Application.streamingAssetsPath +"/"+ path);
        var files = fileInfo.GetFiles();
        var index = Random.Range(0, files.Length);
        var url = files[index].FullName;
        byte[] imgData;
        //Check if we should use UnityWebRequest or File.ReadAllBytes
        if (url.Contains("://") || url.Contains(":///"))
        {
            UnityWebRequest www = UnityWebRequest.Get(url);
            yield return www.SendWebRequest();
            imgData = www.downloadHandler.data;
        }
        else
        {
            imgData = System.IO.File.ReadAllBytes(url);
        }
        callback(imgData);
    }

    public void SetGenre(string genre)
    {
        GameManager.Instance.playerDetails.genre = genre;
        Button maleButton = GameObject.Find("Canvas/GenrePanel/Button_male").GetComponent<Button>();
        Button femaleButton = GameObject.Find("Canvas/GenrePanel/Button_female").GetComponent<Button>();
        if (genre == "male")
        {
            maleButton.interactable = false;
            femaleButton.interactable = true;
        }
        else
        {
            maleButton.interactable = true;
            femaleButton.interactable = false;
        }

        GameObject.Find("Canvas/GenrePanel/ValidateGenreButton").SetActive(true);
    }

    public async void OnValidateGenreButtonClick()
    {
        //genrePanel.SetActive(false);
        GetRandomCharacter();
    }
}
