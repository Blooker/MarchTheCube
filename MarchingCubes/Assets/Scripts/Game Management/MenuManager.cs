using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour {

	[SerializeField]
	MapGenerator mapGenerator = null;
    [SerializeField]
    MapInfo mapInfo;

    [Header("Menu UIs")]
    [SerializeField]
    Canvas gameSetupUI;
    [SerializeField]
    Canvas gameOverUI;

    public void LoadMainGame () {
        string seedInputText = gameSetupUI.transform.FindChild("SeedInputField").FindChild("Text").GetComponent<Text>().text;
        mapInfo.seed = seedInputText;

        gameSetupUI.gameObject.SetActive(false);

        SceneManager.LoadScene("MainGame");

        //StartCoroutine(StartMainGame(seed));
    }

    public void LoadGameSetup () {
        ObjectManager.ClearObjects();
        SceneManager.LoadScene("GameSetup");
    }

    public void QuitGame () {
        Application.Quit();
    }

    public void GenerateRandomSeed () {
        string glyphs = "abcdefghijklmnopqrstuvwxyz0123456789";
        string randomSeed = "";

        int charAmount = Random.Range(8, 15); //set those to the minimum and maximum length of your string
        for (int i = 0; i < charAmount; i++) {
            randomSeed += glyphs[Random.Range(0, glyphs.Length)];
        }

        InputField seedInputField = gameSetupUI.transform.FindChild("SeedInputField").GetComponent<InputField>();
        seedInputField.text = randomSeed;
    }

	public void ShowGameLostUI () {
        if (gameOverUI != null) {
            gameOverUI.gameObject.SetActive(true);

            Text resultText = gameOverUI.transform.FindChild("ResultPanel").FindChild("ResultText").gameObject.GetComponent<Text>();
            resultText.text = "You died!";

            Text seedText = gameOverUI.transform.FindChild("SeedPanel").FindChild("SeedText").gameObject.GetComponent<Text>();
            seedText.text = mapGenerator.GetSeed();
        }
	}

	public void ShowGameWonUI () {
        if (gameOverUI != null) {
            gameOverUI.gameObject.SetActive(true);

            Text resultText = gameOverUI.transform.FindChild("ResultPanel").FindChild("ResultText").gameObject.GetComponent<Text>();
            resultText.text = "You killed all the enemies!";

            Text seedText = gameOverUI.transform.FindChild("SeedPanel").FindChild("SeedText").gameObject.GetComponent<Text>();
            seedText.text = mapGenerator.GetSeed();
        }
	}

    public void ShowGamePauseUI () {
        gameOverUI.gameObject.SetActive(true);

        Text resultText = gameOverUI.transform.FindChild("ResultPanel").FindChild("ResultText").gameObject.GetComponent<Text>();
        resultText.text = "Game paused\n(Esc to unpause)";

        Text seedText = gameOverUI.transform.FindChild("SeedPanel").FindChild("SeedText").gameObject.GetComponent<Text>();
        seedText.text = mapGenerator.GetSeed();
    }

	public void HideGameEndUI () {
        if (gameOverUI != null)
            gameOverUI.gameObject.SetActive(false);
	}

	// Use this for initialization
	void Start () {
		HideGameEndUI();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
