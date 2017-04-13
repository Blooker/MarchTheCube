using UnityEngine;
using System.Collections;

/// <summary>
/// Class that contains statistics about the player character (how much health/ammo they have, etc.)
/// </summary>

public class PlayerStats : MonoBehaviour {

    // Defining variables
    // Square bracket tags change how Unity displays attributes in the inspector

    [SerializeField]
	private int health, ammo;

    [SerializeField]
    private float jetpackTimeLimit = 5f;

	private bool playerDead = false;
	private float jetpackTime;

    private GameManager gameManager;
    private PlayerUI playerUI;


    /* ------------------
     * BUILT-IN FUNCTIONS
     * ------------------ */

    // Use this for initialization
    void Start() {
        playerUI = GetComponent<PlayerUI>();
        jetpackTime = jetpackTimeLimit;

        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

        UpdateHealthCounter();
        UpdateAmmoCounter();
        UpdateJetpackGuage();
    }


    /* ----------------
     * CUSTOM FUNCTIONS
     * ---------------- */

    // Kills the player character, making them lose the game
    public void KillPlayer () {
        Debug.Log("You're dead");
		gameManager.LoseGame();
		playerDead = true;
    }

    #region Health functions

    // Gives a set amount of health to the player
    public void AddHealth (int _health) {
		health += _health;
        UpdateHealthCounter();
	}

    // Removes a set amount of health from the player
    public void RemoveHealth (int _health) {
        if (!playerDead) {
            health -= _health;
            UpdateHealthCounter();

            if (health <= 0) {
                KillPlayer();
            }
        }
    }

    // Returns the player's current health count
    public int GetHealthCount () {
		return health;
	}
    #endregion

    #region Ammo functions

    // Gives a set amount of ammunition to the player
    public void AddAmmo (int _ammo) {
		ammo += _ammo;
		UpdateAmmoCounter();
	}

    // Removes a set amount of ammunition from the player
    public void RemoveAmmo (int _ammo) {
		ammo -= _ammo;
        UpdateAmmoCounter();
	}

    // Returns the player's current ammo count
    public int GetAmmoCount () {
		return ammo;
	}
    #endregion

    #region Jetpack functions

    // Adds time to jetpack time limit
    public void AddJetpackTime (float _jetpackTime) {
        if (CanAddJetpackTime()) {
            jetpackTime += _jetpackTime;
            UpdateJetpackGuage();
        }
    }

    // Takes away time from jetpack time limit
    public void RemoveJetpackTime(float _jetpackTime) {
        if (JetpackTimeLeft()) {
            jetpackTime -= _jetpackTime;
            UpdateJetpackGuage();
        }
    }

    // Returns whether or not the jetpack can be used currently
    public bool JetpackTimeLeft () {
        return jetpackTime > 0;
    }

    // Returns whether or not time can be added to the jetpack
    public bool CanAddJetpackTime () {
        return jetpackTime < jetpackTimeLimit;
    }

    // Gets the current amount of time left that the player can use the jetpack for
    public float GetJetpackTime () {
        return jetpackTime;
    }
    #endregion

    // Updates the player UI's health counter
    void UpdateHealthCounter () {
        if (playerUI != null)
            playerUI.SetHealthCounter(health);
    }

    // Updates the player UI's ammo counter
    void UpdateAmmoCounter () {
        if (playerUI != null)
            playerUI.SetAmmoCounter(ammo);
    }

    // Updates the player UI's jetpack guage slider
    void UpdateJetpackGuage () {
        if(playerUI != null)
            playerUI.SetJetpackGuage(jetpackTime, jetpackTimeLimit);
    }
}
