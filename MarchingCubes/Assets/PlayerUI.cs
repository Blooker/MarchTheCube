using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour {

	GameObject playerCanvas;
	Text ammoCounter;
    Text enemyCounter;

	public void SetCanvas (GameObject canvas) {
		playerCanvas = canvas;
		ammoCounter = playerCanvas.transform.FindChild("AmmoCounter").gameObject.GetComponent<Text>();
	}

	public void SetAmmoCounter (int ammo) {
		ammoCounter.text = "Ammo: " + ammo.ToString();
	}

    public void SetEnemyCounter (int enemiesInLevel) {
        enemyCounter.text = "Enemies left: " + enemiesInLevel.ToString();
    }

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
