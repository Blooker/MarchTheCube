using UnityEngine;
using System.Collections;

/// <summary>
/// Class for handling the initial game countdown
/// </summary>

public class GameCountdown : MonoBehaviour {

    // Defining variables
    // Square bracket tags change how Unity displays attributes in the inspector

    [SerializeField]
    private int timeToCountdown;

    private static bool timerComplete = false;

    private float countdownTimer;
    private int nextCounterChange;

    private bool timerDepleting;

    private PlayerUI playerUI;
    private GameManager gameManager;

    /* ------------------
     * BUILT-IN FUNCTIONS
     * ------------------ */

    // Use this for initialization
    void Start () {
        ResetCountdown();
        gameManager = GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update() {

        // If the player character has been created in the game world, get player UI and start initial countdown
        if (GetComponent<ObjectManager>().GetCurrentPlayer() != null) {
            playerUI = GetComponent<ObjectManager>().GetCurrentPlayer().GetComponent<PlayerUI>();
            StartCountdown();
        }

        if (timerDepleting)
            countdownTimer -= Time.deltaTime;

        if (countdownTimer <= -1) {
            StopCountdown();

        } else if (countdownTimer <= 0) {
            timerComplete = true;
            playerUI.SetCountdownTimer("Go!!");
            gameManager.StartGame();

        } else if (playerUI != null) {
            playerUI.SetCountdownTimer( (Mathf.CeilToInt(countdownTimer)).ToString() );
        }
	}


    /* ----------------
     * CUSTOM FUNCTIONS
     * ---------------- */

    // Starts the initial game countdown
    public void StartCountdown () {
        timerDepleting = true;
    }

    // Stops the initial game countdown
    public void StopCountdown () {
        timerDepleting = false;
        playerUI.SetCountdownTimer("");
    }

    // Resets the initial game countdown
    public void ResetCountdown() {
        timerComplete = false;
        countdownTimer = timeToCountdown;
        nextCounterChange = timeToCountdown - 1;
    }

    // Returns whether the initial game countdown has completed
    public static bool CountdownComplete () {
        return timerComplete;
    }
}
