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
        string seed = gameSetupUI.transform.FindChild("SeedInputField").FindChild("Text").GetComponent<Text>().text;
        mapInfo.seed = seed;

        gameSetupUI.gameObject.SetActive(false);

        SceneManager.LoadScene("MainGame");

        //StartCoroutine(StartMainGame(seed));
    }

    public void LoadGameSetup () {
        SceneManager.LoadScene("GameSetup");
    }

    public void GenerateRandomSeed () {

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

	public void HideGameEndUI () {
        if (gameOverUI != null)
            gameOverUI.gameObject.SetActive(false);
	}

    public void FindMapGenerator () {
        GameObject mapGeneratorObj = null;
        mapGeneratorObj = GameObject.Find("MapGenerator");

        Debug.Log(mapGeneratorObj);

        mapGenerator = mapGeneratorObj.GetComponent<MapGenerator>();
    }

    /*IEnumerator StartMainGame (string seed) {
        yield return new WaitForSeconds(2);

        gameOverUI = GameObject.Find("GameOverUI").GetComponent<Canvas>();

        HideGameEndUI();
        FindMapGenerator();

        gameSetupUI = null;

        if (mapGenerator != null) {
            mapGenerator.SetSeed(seed);
            mapGenerator.GenerateMap();
        } else {
            Debug.Log("Couldn't find map generator");
        }

        GetComponent<GameCountdown>().StartCountdown();
    }*/

	// Use this for initialization
	void Start () {
		HideGameEndUI();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
