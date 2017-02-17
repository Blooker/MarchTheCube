using UnityEngine;
using System.Collections;

public class PlayerStats : MonoBehaviour {

    [SerializeField]
	private int health, ammo;
	private PlayerUI playerUI;

	public void AddHealth (int _health) {
		health += _health;
	}

	public void RemoveHealth (int _health) {
		health -= _health;
	}

	public int GetHealthCount () {
		return health;
	}

	public void AddAmmo (int _ammo) {
		ammo += _ammo;
		playerUI.SetAmmoCounter(ammo);
	}

	public void RemoveAmmo (int _ammo) {
		ammo -= _ammo;
		playerUI.SetAmmoCounter(ammo);
	}

	public int GetAmmoCount () {
		return ammo;
	}

	void Start () {
		playerUI = GetComponent<PlayerUI>();
		playerUI.SetAmmoCounter(ammo);
	}
}
