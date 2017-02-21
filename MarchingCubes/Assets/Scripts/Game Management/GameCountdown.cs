using UnityEngine;
using System.Collections;

public class GameCountdown : MonoBehaviour {

    PlayerUI playerUI;

    [SerializeField]
    int timeToCountdown;
    float countdownTimer;
    int nextCounterChange;

    static bool timerComplete = false;
    bool timerDepleting;

	// Use this for initialization
	void Start () {
        ResetCountdown();
    }

    // Update is called once per frame
    void Update() {            
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

        } else {
            playerUI.SetCountdownTimer((Mathf.CeilToInt(countdownTimer)).ToString());
        }
	}

    public void StartCountdown () {
        timerDepleting = true;
    }

    public void StopCountdown () {
        timerDepleting = false;
        playerUI.SetCountdownTimer("");
    }

    public void ResetCountdown() {
        timerComplete = false;
        countdownTimer = timeToCountdown;
        nextCounterChange = timeToCountdown - 1;
    }

    public static bool CountdownComplete () {
        return timerComplete;
    }
}
