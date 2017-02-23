using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

	ObjectManager objectManager;
	MenuManager menuManager;

	public void StartGame () {
        GameObject currentPlayer = objectManager.GetCurrentPlayer();
        PlayerInput playerInput = currentPlayer.GetComponent<PlayerInput>();

        playerInput.EnableAllInput();
    }

	public void WinGame () {

	}

	public void LoseGame () {
		GameObject currentPlayer = objectManager.GetCurrentPlayer();

		PlayerInput playerInput = currentPlayer.GetComponent<PlayerInput>();
        playerInput.DisableAllInput();

		currentPlayer.GetComponent<PlayerUI>().ShowPlayerUI(false);
		menuManager.ShowGameOverUI(true);

        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
	}

	// Use this for initialization
	void Start () {
		objectManager = GetComponent<ObjectManager>();
		menuManager = GetComponent<MenuManager>();
	}
	
	// Update is called once per frame
	void Update () {
        //Debug.Log(objectManager.GetCurrentPlayer().name);
	}
}
