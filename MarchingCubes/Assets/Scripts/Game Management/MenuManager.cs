using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour {

	[SerializeField]
	MapGenerator mapGenerator;

	[SerializeField]
	Canvas gameOverUI;

	public void ShowGameLostUI () {
		gameOverUI.gameObject.SetActive(true);

		Text resultText = gameOverUI.transform.FindChild("ResultPanel").FindChild("ResultText").gameObject.GetComponent<Text>();
		resultText.text = "You died!";

		Text seedText = gameOverUI.transform.FindChild("SeedPanel").FindChild("SeedText").gameObject.GetComponent<Text>();
		seedText.text = mapGenerator.GetSeed();
	}

	public void ShowGameWonUI () {
		gameOverUI.gameObject.SetActive(true);
		
		Text resultText = gameOverUI.transform.FindChild("ResultPanel").FindChild("ResultText").gameObject.GetComponent<Text>();
		resultText.text = "You killed all the enemies!";
			
		Text seedText = gameOverUI.transform.FindChild("SeedPanel").FindChild("SeedText").gameObject.GetComponent<Text>();
		seedText.text = mapGenerator.GetSeed();
	}

	public void HideGameEndUI () {
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
