using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

    bool gameStarted = false;

	ObjectManager objectManager;
	MenuManager menuManager;

	public void StartGame () {
        gameStarted = true;

        GameObject currentPlayer = objectManager.GetCurrentPlayer();
        PlayerInput playerInput = currentPlayer.GetComponent<PlayerInput>();

        playerInput.EnableAllInput();
    }

    public bool GameWon () {
        return ObjectManager.GetEnemiesInLevel().Count <= 0;
    }

	public void LoseGame () {
        gameStarted = false;

        EnableMenuInput();
		menuManager.ShowGameLostUI();
	}

	public void WinGame () {
        gameStarted = false;

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
        if (gameStarted && GameWon())
            WinGame();
	}
}
