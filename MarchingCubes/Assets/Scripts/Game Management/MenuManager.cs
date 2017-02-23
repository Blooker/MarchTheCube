using UnityEngine;
using System.Collections;

public class MenuManager : MonoBehaviour {

	[SerializeField]
	Canvas gameOverUI;

	public void ShowGameOverUI (bool showUI) {
		gameOverUI.gameObject.SetActive(showUI);
	}

	// Use this for initialization
	void Start () {
		ShowGameOverUI(false);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
