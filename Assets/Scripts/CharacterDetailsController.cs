using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

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
            GameManager.Instance.playerDetails.generationCountSurvive = 1;
        }
        else
        {
            LoadPlayerDetailsData();
        }

        if (GameManager.Instance.funFactsData == null)
        {
            var ffData = Resources.Load<TextAsset>("FunFactsData");
            GameManager.Instance.funFactsData = JsonUtility.FromJson<CharacterFunFacts>(ffData.text);

            var nData = Resources.Load<TextAsset>("NamesData");
            GameManager.Instance.namesData = JsonUtility.FromJson<CharacterNames>(nData.text);
        }
    }

    private void LoadPlayerDetailsData()
    {
        if (GameManager.Instance.playerIsDead)
        {
            GetRandomCharacter();
            GameManager.Instance.playerIsDead = false;
            return;
        }
        UpdateInterfaceView();
    }

    public void OnClickReadyButton()
    {
        if (GameManager.Instance.playerDetails.genre != null)
        {
            GameManager.Instance.LoadScene(ChildScenes.SpamGame);
        }
    }

    string GetFunFact()
    {
        var funFactsData = GameManager.Instance.funFactsData;
        var randomAction = funFactsData.action[Random.Range(0, funFactsData.action.Capacity)];
        var randomSubject = funFactsData.subject[Random.Range(0, funFactsData.subject.Capacity)];
        var randomCompletion = funFactsData.completion[Random.Range(0, funFactsData.completion.Capacity)];
        var randomVenue = funFactsData.venue[Random.Range(0, funFactsData.venue.Capacity)];

        return randomAction + " " + randomSubject + " " + randomCompletion + " " + randomVenue;
    }

    void ApplyTextureOnImage(Texture2D texture, Image img)
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

        Texture2D eyesTex = new Texture2D(2, 2);
        string eyesTextureFilePath = ApplyRandomTextureByPath(ref eyesTex, "Human_Parts/" + characterGenre + "/Eyes").Item2;

        Texture2D faceTex = new Texture2D(2, 2);
        string faceTextureFilePath = ApplyRandomTextureByPath(ref faceTex, "Human_Parts/" + characterGenre + "/Faces").Item2;

        Texture2D hairBackTex = new Texture2D(2, 2);
        string hairBackTextureFilePath = ApplyRandomTextureByPath(ref hairBackTex, "Human_Parts/" + characterGenre + "/HairBack").Item2;

        Texture2D hairFrontTex = new Texture2D(2, 2);
        string hairFrontTextureFilePath = ApplyRandomTextureByPath(ref hairFrontTex, "Human_Parts/" + characterGenre + "/HairFront").Item2;

        Texture2D skinTex = new Texture2D(2, 2);
        string skinTextureFilePath = ApplyRandomTextureByPath(ref skinTex, "Human_Parts/neutral/skin").Item2;

        GameManager.Instance.playerDetails.name = GetRandomName();

        GameManager.Instance.playerDetails.clothes = clotheTextureFilePath;
        GameManager.Instance.playerDetails.eyes = eyesTextureFilePath;
        GameManager.Instance.playerDetails.face = faceTextureFilePath;
        GameManager.Instance.playerDetails.hairBack = hairBackTextureFilePath;
        GameManager.Instance.playerDetails.hairFront = hairFrontTextureFilePath;
        GameManager.Instance.playerDetails.skin = skinTextureFilePath;
        GameManager.Instance.playerDetails.situtation = PlayerDetails.Situtation.HOMELESS_LOW;
        UpdateInterfaceView();
    }

    void UpdateInterfaceView()
    {
        var tupleFilePathsImages = new System.Tuple<string, Image>[] {
           System.Tuple.Create(GameManager.Instance.playerDetails.clothes, clothes),
           System.Tuple.Create(GameManager.Instance.playerDetails.eyes, eyes),
           System.Tuple.Create(GameManager.Instance.playerDetails.face, face),
           System.Tuple.Create(GameManager.Instance.playerDetails.hairBack, backHair),
           System.Tuple.Create(GameManager.Instance.playerDetails.hairFront, frontHair),
           System.Tuple.Create(GameManager.Instance.playerDetails.skin, skin)
        };

        for (int i = 0; i < tupleFilePathsImages.Length; i++)
        {
            var file = System.IO.File.ReadAllBytes(tupleFilePathsImages[i].Item1);
            var texTmp = new Texture2D(2, 2);
            texTmp.LoadImage(file);
            ApplyTextureOnImage(texTmp, tupleFilePathsImages[i].Item2);
        }

        characterNameText.text = GameManager.Instance.playerDetails.name;
        generationCountText.text = "Generation " + GameManager.Instance.playerDetails.generationCountSurvive;

        for (int i = 0; i < funFactTexts.Capacity; i++)
        {
            Text text = funFactTexts[i];
            text.text = GetFunFact();
        }
    }

    string GetRandomName()
    {
        var namesData = GameManager.Instance.namesData;
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

    public void OnValidateGenreButtonClick()
    {
        genrePanel.SetActive(false);
        GetRandomCharacter();
    }
}
