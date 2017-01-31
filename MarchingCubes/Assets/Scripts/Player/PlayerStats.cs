using UnityEngine;
using System.Collections;

public class PlayerStats : MonoBehaviour {

    [SerializeField]
	private int health, ammo;

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
	}

	public void RemoveAmmo (int _ammo) {
		ammo -= _ammo;
	}

	public int GetAmmoCount () {
		return ammo;
	}
}
