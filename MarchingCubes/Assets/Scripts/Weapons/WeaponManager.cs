using UnityEngine;
using System.Collections;

public class WeaponManager : MonoBehaviour {

    [SerializeField]
    Weapon playerWeapon;
    int ammo = 0;

    bool shooting = false;
    float shotInterval;

    Camera playerCam;

    PlayerSound playerSound;
    PlayerStats playerStats;

    public void StartShooting () {
        shooting = true;
        shotInterval += Time.deltaTime;

        if (shotInterval > playerWeapon.fireRate) {
            Shoot();
            shotInterval = 0;
        }
    }

    public void StopShooting () {
        shooting = false;
        shotInterval = playerWeapon.fireRate;
    }

    public void Shoot() {
        if (playerStats.GetAmmoCount() > 0) {
            playerSound.PlayPlayerSound(playerWeapon.gunSound);

            RaycastHit rayHit;

            if (Physics.Raycast(playerCam.transform.position, playerCam.transform.forward, out rayHit, playerWeapon.range)) {

                if (rayHit.collider.gameObject.tag == "Enemy") {
                    EnemyController enemyController = rayHit.collider.GetComponent<EnemyController>();
                    enemyController.DamageEnemy(playerWeapon.damage);
                }

            }

            // If hit enemy, take away health

            // Take away ammo
            playerStats.RemoveAmmo(playerWeapon.bulletsPerShot);
        }
    }

    void Start () {
        playerCam = GetComponent<PlayerController>().GetPlayerCam();
        playerSound = GetComponent<PlayerSound>();
        playerStats = GetComponent<PlayerStats>();

        shotInterval = playerWeapon.fireRate;
    }

    // Update is called once per frame
    void Update () {
	    
	}
}
