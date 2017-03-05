using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour {

    [SerializeField]
    private DamageIndicator damageIndicator;

    [SerializeField]
    float maxJetpackGuageSize;

	GameObject playerCanvas;

    Camera cam;

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

    public void CreateDamageIndicator (float damage, Vector3 hitPoint) {
        string damageString = "-" + damage.ToString();
        DamageIndicator newIndicator = Instantiate(damageIndicator.gameObject, Vector3.zero, Quaternion.identity, playerCanvas.transform).GetComponent<DamageIndicator>();

        newIndicator.SetText(damageString);
        newIndicator.SetHitPoint(hitPoint);
    }

	// Use this for initialization
	void Start () {
        cam = GetComponent<PlayerController>().GetPlayerCam();
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
