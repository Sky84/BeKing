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
    public Text characterNameText;
    public Text generationCountText;
    public Text yearCountText;
    public Text yearFunFactText;

    public Image frontHair;
    public Image eyes;
    public Image face;
    public Image clothes;
    public Image skin;
    public Image backHair;

    private FunFacts funFactsData;
    private Names namesData;
    // Start is called before the first frame update
    void Start()
    {
        genrePanel.SetActive(false);
        yearCountText.gameObject.SetActive(false);
        yearFunFactText.gameObject.SetActive(false);
        if (GameManager.Instance.playerDetails.genre == null)
        {
            genrePanel.SetActive(true);
            GameObject.Find("Canvas/GenrePanel/ValidateGenreButton").SetActive(false);
        }
        else
        {
            LoadPlayerDetailsData();
        }

        var ffData = Resources.Load<TextAsset>("FunFactsData");
        funFactsData = JsonUtility.FromJson<FunFacts>(ffData.text);

        var nData = Resources.Load<TextAsset>("NamesData");
        namesData = JsonUtility.FromJson<Names>(nData.text);
    }

    private void LoadPlayerDetailsData()
    {
        print("Need to load data"); // TODO LOAD DATA PLAYER DETAILS
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
        var randomAction = funFactsData.action[Random.Range(0, funFactsData.action.Capacity)];
        var randomSubject = funFactsData.subject[Random.Range(0, funFactsData.subject.Capacity)];
        var randomCompletion = funFactsData.completion[Random.Range(0, funFactsData.completion.Capacity)];
        var randomVenue = funFactsData.venue[Random.Range(0, funFactsData.venue.Capacity)];

        return randomAction + " " + randomSubject + " " + randomCompletion + " " + randomVenue;
    }

    void ApplyTextureOnImage(Texture2D texture, ref Image img)
    {
        img.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f), 100f);
        img.SetNativeSize();
    }

    System.Tuple<Texture2D, string> ApplyRandomTextureByPath(ref Texture2D texture, string path)
    {
        var fileInfo = new DirectoryInfo(Application.streamingAssetsPath + "/" + path);
        var files = fileInfo.GetFiles("*png");
        var index = Random.Range(0, files.Length);
        var filePath = files[index].FullName;
        var randomFile = System.IO.File.ReadAllBytes(filePath);
        texture.LoadImage(randomFile);
        return System.Tuple.Create(texture, filePath);
    }

    void GetRandomCharacter()
    {
        var characterGenre = GameManager.Instance.playerDetails.genre;
        Texture2D clotheTex = new Texture2D(2, 2);
        string clotheTextureFilePath = ApplyRandomTextureByPath(ref clotheTex, "Human_Parts/" + characterGenre + "/Clothes").Item2;
        ApplyTextureOnImage(clotheTex, ref clothes);

        Texture2D eyesTex = new Texture2D(2, 2);
        string eyesTextureFilePath = ApplyRandomTextureByPath(ref eyesTex, "Human_Parts/" + characterGenre + "/Eyes").Item2;
        ApplyTextureOnImage(eyesTex, ref eyes);

        Texture2D faceTex = new Texture2D(2, 2);
        string faceTextureFilePath = ApplyRandomTextureByPath(ref faceTex, "Human_Parts/" + characterGenre + "/Faces").Item2;
        ApplyTextureOnImage(faceTex, ref face);

        Texture2D hairBackTex = new Texture2D(2, 2);
        string hairBackTextureFilePath = ApplyRandomTextureByPath(ref hairBackTex, "Human_Parts/" + characterGenre + "/HairBack").Item2;
        ApplyTextureOnImage(hairBackTex, ref backHair);

        Texture2D hairFrontTex = new Texture2D(2, 2);
        string hairFrontTextureFilePath = ApplyRandomTextureByPath(ref hairFrontTex, "Human_Parts/" + characterGenre + "/HairFront").Item2;
        ApplyTextureOnImage(hairFrontTex, ref frontHair);

        Texture2D skinTex = new Texture2D(2, 2);
        string skinTextureFilePath = ApplyRandomTextureByPath(ref skinTex, "Human_Parts/neutral/skin").Item2;
        ApplyTextureOnImage(skinTex, ref skin);

        GameManager.Instance.playerDetails.name = GetRandomName();
        characterNameText.text = GameManager.Instance.playerDetails.name;

        GameManager.Instance.playerDetails.clothes = clotheTextureFilePath;
        GameManager.Instance.playerDetails.eyes = eyesTextureFilePath;
        GameManager.Instance.playerDetails.face = faceTextureFilePath;
        GameManager.Instance.playerDetails.hairBack = hairBackTextureFilePath;
        GameManager.Instance.playerDetails.hairFront = hairFrontTextureFilePath;
        GameManager.Instance.playerDetails.skin = skinTextureFilePath;

        for (int i = 0; i < funFactTexts.Capacity; i++)
        {
            Text text = funFactTexts[i];
            text.text = GetFunFact();
        }
    }

    string GetRandomName()
    {
        var namesByGenre = new List<string>();
        if (GameManager.Instance.playerDetails.genre == "male")
        {
            namesByGenre = namesData.male;
        }
        else
        {
            namesByGenre = namesData.female;
        }
        return namesByGenre[Random.Range(0, namesByGenre.Capacity)];
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
        genrePanel.SetActive(false);
        GetRandomCharacter();
    }
}
