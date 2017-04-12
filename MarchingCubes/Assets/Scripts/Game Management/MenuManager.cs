using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.UI;

/// <summary>
/// Class for handling all menu behaviours (pause menu, setup menu, game over menu, etc.)
/// </summary>

public class MenuManager : MonoBehaviour {

    // Defining variables
    // Square bracket tags change how Unity displays attributes in the inspector

	[SerializeField]
	private MapGenerator mapGenerator = null;
    [SerializeField]
    private MapInfo mapInfo;

    [Header("Menu UIs")]
    [SerializeField]
    private Canvas gameSetupUI;
    [SerializeField]
    private Canvas gameOverUI;


    /* ------------------
     * BUILT-IN FUNCTIONS
     * ------------------ */

    // Use this for initialization
    void Start() {
        HideGameEndUI();
    }


    /* ----------------
     * CUSTOM FUNCTIONS
     * ---------------- */
    
    // Passes user's seed to MapInfo object and loads the MainGame scene
    public void LoadMainGame () {

        // Gets seed by finding the input field object called "SeedInputField" and reading the text entered into it
        string seedInputText = gameSetupUI.transform.FindChild("SeedInputField").FindChild("Text").GetComponent<Text>().text;
        mapInfo.seed = seedInputText;

        gameSetupUI.gameObject.SetActive(false);

        SceneManager.LoadScene("MainGame");

        //StartCoroutine(StartMainGame(seed));
    }

    // Clears all objects from MainGame scene and loads the GameSetup scene
    public void LoadGameSetup () {
        ObjectManager.ClearAllLevelObjects();
        SceneManager.LoadScene("GameSetup");
    }

    // Closes the application
    public void QuitGame () {
        Application.Quit();
    }

    // Generates a random seed string and inserts it into the seed input field
    public void GenerateRandomSeed () {
        string glyphs = "abcdefghijklmnopqrstuvwxyz0123456789";
        string randomSeed = "";

        int charAmount = Random.Range(8, 15); //set those to the minimum and maximum length of your seed
        for (int i = 0; i < charAmount; i++) {
            randomSeed += glyphs[Random.Range(0, glyphs.Length)];
        }

        InputField seedInputField = gameSetupUI.transform.FindChild("SeedInputField").GetComponent<InputField>();
        seedInputField.text = randomSeed;
    }

    // Shows the lose screen UI
	public void ShowGameLostUI () {
        if (gameOverUI != null) {
            gameOverUI.gameObject.SetActive(true);

            Text resultText = gameOverUI.transform.FindChild("ResultPanel").FindChild("ResultText").gameObject.GetComponent<Text>();
            resultText.text = "You died!";

            Text seedText = gameOverUI.transform.FindChild("SeedPanel").FindChild("SeedText").gameObject.GetComponent<Text>();
            seedText.text = mapGenerator.GetSeed();
        }
	}

    // Shows the win screen UI
	public void ShowGameWonUI () {
        if (gameOverUI != null) {
            gameOverUI.gameObject.SetActive(true);

            Text resultText = gameOverUI.transform.FindChild("ResultPanel").FindChild("ResultText").gameObject.GetComponent<Text>();
            resultText.text = "You killed all the enemies!";

            Text seedText = gameOverUI.transform.FindChild("SeedPanel").FindChild("SeedText").gameObject.GetComponent<Text>();
            seedText.text = mapGenerator.GetSeed();
        }
	}

    // Shows the pause screen UI
    public void ShowGamePauseUI () {
        gameOverUI.gameObject.SetActive(true);

        Text resultText = gameOverUI.transform.FindChild("ResultPanel").FindChild("ResultText").gameObject.GetComponent<Text>();
        resultText.text = "Game paused\n(Esc to unpause)";

        Text seedText = gameOverUI.transform.FindChild("SeedPanel").FindChild("SeedText").gameObject.GetComponent<Text>();
        seedText.text = mapGenerator.GetSeed();
    }

    /* Hides the game end UI (win, lose and pause screen UIs are all slightly modified versions of game end UI,
     * so are hidden when this function is called) */
	public void HideGameEndUI () {
        if (gameOverUI != null)
            gameOverUI.gameObject.SetActive(false);
	}
}
