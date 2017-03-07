using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

    [SerializeField]
    MapGenerator mapGenerator;

    bool gameStarted = false, gamePaused = false;

	ObjectManager objectManager;
	MenuManager menuManager;

	public void StartGame () {
        gameStarted = true;

        GameObject currentPlayer = objectManager.GetCurrentPlayer();
        PlayerInput playerInput = currentPlayer.GetComponent<PlayerInput>();

        playerInput.EnableAllInput();
    }

    public void PauseGame () {
        if (gameStarted) {
            gamePaused = true;
            EnableMenuInput();

            objectManager.FreezePhysicsObjects();

            menuManager.ShowGamePauseUI();

            Time.timeScale = 0;
            Time.fixedDeltaTime = 0;
        }
    }

    public void UnpauseGame() {
        if (gameStarted) {
            Time.timeScale = 1;
            Time.fixedDeltaTime = 0.02f;

            gamePaused = false;
            DisableMenuInput();

            objectManager.UnfreezePhysicsObjects();

            menuManager.HideGameEndUI();
        }
    }

    public void TogglePause() {
        if (gamePaused) {
            UnpauseGame();
        } else {
            PauseGame();
        }
    }

    public void LoseGame() {
        gameStarted = false;

        EnableMenuInput();
        menuManager.ShowGameLostUI();
    }

    public void WinGame() {
        gameStarted = false;

        EnableMenuInput();
        menuManager.ShowGameWonUI();
    }

    public void RestartGame () {
        UnpauseGame();

        string seed = mapGenerator.GetSeed();

        ObjectManager.ClearAllLevelObjects();
        objectManager.ClearCurrentPlayer();

        ObjectPlacement objectPlacement = mapGenerator.GetComponent<ObjectPlacement>();
        objectPlacement.PlaceRandomObjects(seed);

        GameCountdown gameCountdown = GetComponent<GameCountdown>();
        gameCountdown.ResetCountdown();

        menuManager.HideGameEndUI();
        DisableMenuInput();
        
    }

    public bool WinConditionMet () {
        return ObjectManager.GetEnemiesInLevel().Count <= 0;
    }


	/// <summary>
	/// Hides the player UI and unlocks the mouse cursor from the centre of the screen
	/// </summary>
	void EnableMenuInput () {
		GameObject currentPlayer = objectManager.GetCurrentPlayer();
		
		PlayerInput playerInput = currentPlayer.GetComponent<PlayerInput>();

        playerInput.DisableAllInput();
        if (gameStarted)
            playerInput.AbleToPause(true);

		currentPlayer.GetComponent<PlayerUI>().ShowPlayerUI(false);

		Cursor.lockState = CursorLockMode.Confined;
		Cursor.visible = true;
	}

    /// <summary>
    /// Shows the player UI and locks the mouse cursor to the centre of the screen
    /// </summary>
    void DisableMenuInput() {
        GameObject currentPlayer = objectManager.GetCurrentPlayer();

        PlayerInput playerInput = currentPlayer.GetComponent<PlayerInput>();
        playerInput.EnableAllInput();

        currentPlayer.GetComponent<PlayerUI>().ShowPlayerUI(true);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Use this for initialization
    void Start () {
		objectManager = GetComponent<ObjectManager>();
		menuManager = GetComponent<MenuManager>();
	}
	
	// Update is called once per frame
	void Update () {
        if (gameStarted && WinConditionMet())
            WinGame();
	}
}
