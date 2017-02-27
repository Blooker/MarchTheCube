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

	public void LoseGame () {
		EnableMenuInput();
		menuManager.ShowGameLostUI();
	}

	public void WinGame () {
		EnableMenuInput();
		menuManager.ShowGameWonUI();
	}

	/// <summary>
	/// Hides the player UI and unlocks the mouse cursor from the centre of the screen
	/// </summary>
	void EnableMenuInput () {
		GameObject currentPlayer = objectManager.GetCurrentPlayer();
		
		PlayerInput playerInput = currentPlayer.GetComponent<PlayerInput>();
		playerInput.DisableAllInput();

		currentPlayer.GetComponent<PlayerUI>().ShowPlayerUI(false);

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
        if (ObjectManager.GetEnemiesInLevel().Count <= 0)
			WinGame();
	}
}
