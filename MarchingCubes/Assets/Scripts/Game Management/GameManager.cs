using UnityEngine;
using System.Collections;

/// <summary>
/// Class for managing game progression and controlling when the game starts, ends and pauses
/// </summary>

public class GameManager : MonoBehaviour {

    // Defining variables
    // Square bracket tags change how Unity displays attributes in the inspector

    [SerializeField]
    private MapGenerator mapGenerator;

    private bool gameStarted = false, gamePaused = false;

	private ObjectManager objectManager;
	private MenuManager menuManager;


    /* ------------------
     * BUILT-IN FUNCTIONS
     * ------------------ */

    // Use this for initialization
    void Start() {
        objectManager = GetComponent<ObjectManager>();
        menuManager = GetComponent<MenuManager>();
    }

    // Update is called once per frame
    void Update() {
        if (gameStarted && WinConditionMet())
            WinGame();
    }


    /* ----------------
     * CUSTOM FUNCTIONS
     * ---------------- */

    // Starts the game, allowing the player to move and shoot
    public void StartGame () {
        gameStarted = true;

        GameObject currentPlayer = objectManager.GetCurrentPlayer();
        PlayerInput playerInput = currentPlayer.GetComponent<PlayerInput>();

        playerInput.EnableAllInput();
    }

    // Temporarily pauses the game and opens the pause menu
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

    // Unpauses the game and hides the pause menu
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

    // Toggles between paused and unpaused states
    public void TogglePause() {
        if (gamePaused) {
            UnpauseGame();
        } else {
            PauseGame();
        }
    }

    // Ends the game and shows the lose menu
    public void LoseGame() {
        gameStarted = false;

        EnableMenuInput();
        menuManager.ShowGameLostUI();
    }

    // Ends the game and shows the win menu
    public void WinGame() {
        gameStarted = false;

        EnableMenuInput();
        menuManager.ShowGameWonUI();
    }

    // Respawns the player, enemies and items and restarts the initial countdown timer
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

    // Returns whether the game's win condition has been met
    public bool WinConditionMet () {
        return ObjectManager.GetEnemiesInLevel().Count <= 0;
    }

	// Hides the in-game player UI and unlocks the mouse cursor from the centre of the screen
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

    // Shows the in-game player UI and locks the mouse cursor to the centre of the screen
    void DisableMenuInput() {
        GameObject currentPlayer = objectManager.GetCurrentPlayer();

        PlayerInput playerInput = currentPlayer.GetComponent<PlayerInput>();
        playerInput.EnableAllInput();

        currentPlayer.GetComponent<PlayerUI>().ShowPlayerUI(true);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}
