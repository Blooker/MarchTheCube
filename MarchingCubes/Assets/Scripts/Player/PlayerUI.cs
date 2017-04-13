using UnityEngine;
using System.Collections;
using UnityEngine.UI;

/// <summary>
/// Class for changing and updating the player's user interface
/// </summary>

public class PlayerUI : MonoBehaviour {
    /* NOTE: Canvas is Unity's current user interface system
     * When I reference a "canvas", I am talking about the 2D plane that
     * all UI elements in my game are displayed on */

    // Defining variables
    // Square bracket tags change how Unity displays attributes in the inspector
    [SerializeField]
    private DamageIndicator damageIndicator;

    [SerializeField]
    private float maxJetpackGuageSize;

	private GameObject playerCanvas;

    private Text countdownTimer, healthCounter, ammoCounter, enemyCounter;
    private RectTransform jetpackGuage;

    /* ----------------
     * CUSTOM FUNCTIONS
     * ---------------- */

    // Sets the player's UI canvas
    public void SetCanvas (GameObject canvas) {
		playerCanvas = canvas;

        countdownTimer = playerCanvas.transform.FindChild("CountdownTimer").gameObject.GetComponent<Text>();
        healthCounter = playerCanvas.transform.FindChild("HealthCounter").gameObject.GetComponent<Text>();
		ammoCounter = playerCanvas.transform.FindChild("AmmoCounter").gameObject.GetComponent<Text>();
        enemyCounter = playerCanvas.transform.FindChild("EnemyCounter").gameObject.GetComponent<Text>();

        jetpackGuage = playerCanvas.transform.FindChild("JetpackTime").FindChild("JetpackGuage").gameObject.GetComponent<RectTransform>();
    }

    // Shows the player UI
	public void ShowPlayerUI (bool showUI) {
		playerCanvas.SetActive(showUI);
	}

    // Hides the player UI
    public void SetCountdownTimer (string time) {
        countdownTimer.text = "<b>" + time + "</b>";
    }

    // Sets the health counter's text in the player UI
    public void SetHealthCounter(int health) {
        healthCounter.text = "<b>Health</b>\n" + health.ToString();
    }

    // Sets the ammo counter's text in the player UI
    public void SetAmmoCounter (int ammo) {
		ammoCounter.text = "<b>Ammo</b>\n" + ammo.ToString();
	}

    // Sets the enemy counter's text in the player UI
    public void SetEnemyCounter (int enemiesInLevel) {
        enemyCounter.text = "<b>Enemies left</b>\n" + enemiesInLevel.ToString();
    }

    // Sets the jetpack guage's size in the player UI
    public void SetJetpackGuage (float jetpackTime, float jetpackTimeLimit) {
        float guageSize = maxJetpackGuageSize * (jetpackTime / jetpackTimeLimit);
        jetpackGuage.sizeDelta = new Vector2(guageSize, jetpackGuage.sizeDelta.y);
    }

    /* Creates a damage indicator that shows the amount of damage done to an enemy
     * Follows the point where the enemy was hit and eventually disappears */
    public void CreateDamageIndicator (float damage, Vector3 hitPoint) {
        string damageString = "-" + damage.ToString();
        DamageIndicator newIndicator = Instantiate(damageIndicator.gameObject, Vector3.zero, Quaternion.identity, playerCanvas.transform).GetComponent<DamageIndicator>();

        newIndicator.SetText(damageString);
        newIndicator.SetHitPoint(hitPoint);
    }
}
