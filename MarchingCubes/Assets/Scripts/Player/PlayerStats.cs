using UnityEngine;
using System.Collections;

public class PlayerStats : MonoBehaviour {

    [SerializeField]
	private int health, ammo;
    [SerializeField]
    private float jetpackTimeLimit = 5f;

    private float jetpackTime;

	private PlayerUI playerUI;

    public void KillPlayer () {
        Debug.Log("Yo ass died........");
    }

    #region Health functions
    public void AddHealth (int _health) {
		health += _health;
        UpdateHealthCounter();
	}

	public void RemoveHealth (int _health) {
		health -= _health;
        UpdateHealthCounter();

        if (health <= 0) {
            KillPlayer();
        }
    }

    public int GetHealthCount () {
		return health;
	}
    #endregion

    #region Ammo functions
    public void AddAmmo (int _ammo) {
		ammo += _ammo;
		UpdateAmmoCounter();
	}

	public void RemoveAmmo (int _ammo) {
		ammo -= _ammo;
        UpdateAmmoCounter();
	}

	public int GetAmmoCount () {
		return ammo;
	}
    #endregion

    #region Jetpack functions
    public void AddJetpackTime (float _jetpackTime) {
        if (CanAddJetpackTime()) {
            jetpackTime += _jetpackTime;
            UpdateJetpackGuage();
        }
    }

    public void RemoveJetpackTime(float _jetpackTime) {
        if (JetpackTimeLeft()) {
            jetpackTime -= _jetpackTime;
            UpdateJetpackGuage();
        }
    }

    public bool JetpackTimeLeft () {
        return jetpackTime > 0;
    }

    public bool CanAddJetpackTime () {
        return jetpackTime < jetpackTimeLimit;
    }

    public float GetJetpackTime () {
        return jetpackTime;
    }
    #endregion

    void UpdateHealthCounter () {
        if (playerUI != null)
            playerUI.SetHealthCounter(health);
    }

    void UpdateAmmoCounter () {
        if (playerUI != null)
            playerUI.SetAmmoCounter(ammo);
    }

    void UpdateJetpackGuage () {
        if(playerUI != null)
            playerUI.SetJetpackGuage(jetpackTime, jetpackTimeLimit);
    }

    void Start () {
		playerUI = GetComponent<PlayerUI>();
        jetpackTime = jetpackTimeLimit;

        UpdateHealthCounter();
        UpdateAmmoCounter();
        UpdateJetpackGuage();
	}
}
