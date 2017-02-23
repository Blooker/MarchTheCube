using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour {

    [SerializeField]
    float maxJetpackGuageSize;

	GameObject playerCanvas;
    Text countdownTimer, healthCounter, ammoCounter, enemyCounter;
    RectTransform jetpackGuage;

	public void SetCanvas (GameObject canvas) {
		playerCanvas = canvas;

        countdownTimer = playerCanvas.transform.FindChild("CountdownTimer").gameObject.GetComponent<Text>();
        healthCounter = playerCanvas.transform.FindChild("HealthCounter").gameObject.GetComponent<Text>();
		ammoCounter = playerCanvas.transform.FindChild("AmmoCounter").gameObject.GetComponent<Text>();
        enemyCounter = playerCanvas.transform.FindChild("EnemyCounter").gameObject.GetComponent<Text>();

        jetpackGuage = playerCanvas.transform.FindChild("JetpackTime").FindChild("JetpackGuage").gameObject.GetComponent<RectTransform>();
    }

	public void ShowPlayerUI (bool showUI) {
		playerCanvas.SetActive(showUI);
	}

    public void SetCountdownTimer (string time) {
        countdownTimer.text = "<b>" + time + "</b>";
    }

    public void SetHealthCounter(int health) {
        healthCounter.text = "<b>Health</b>\n" + health.ToString();
    }

    public void SetAmmoCounter (int ammo) {
		ammoCounter.text = "<b>Ammo</b>\n" + ammo.ToString();
	}

    public void SetEnemyCounter (int enemiesInLevel) {
        enemyCounter.text = "<b>Enemies left</b>\n" + enemiesInLevel.ToString();
    }

    public void SetJetpackGuage (float jetpackTime, float jetpackTimeLimit) {
        float guageSize = maxJetpackGuageSize * (jetpackTime / jetpackTimeLimit);
        jetpackGuage.sizeDelta = new Vector2(guageSize, jetpackGuage.sizeDelta.y);
    }

	// Use this for initialization
	void Start () {
	    
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
