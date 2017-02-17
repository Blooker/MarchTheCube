using UnityEngine;
using System.Collections;

public class WeaponManager : MonoBehaviour {

    [SerializeField]
    Weapon playerWeapon;

    bool shooting = false;
    float shotInterval;

    Camera playerCam;

    PlayerSound playerSound;
    PlayerStats playerStats;

    public void StartShooting () {
        if (playerStats.GetAmmoCount() > 0)
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

            // If raycast has come into contact with a collider
            if (Physics.Raycast(playerCam.transform.position, playerCam.transform.forward, out rayHit, playerWeapon.range)) {

                // If hit enemy, take away health
                if (rayHit.collider.gameObject.tag == "Enemy") {
                    EnemyController enemyController = rayHit.collider.GetComponent<EnemyController>();
                    enemyController.DamageEnemy(playerWeapon.damage);
                }

            }

            // Take away ammo
            playerStats.RemoveAmmo(playerWeapon.bulletsPerShot);
        }
    }

    public bool PlayerIsShooting () {
        return shooting;
    }

    void Start () {
        playerCam = GetComponent<PlayerController>().GetPlayerCam();
        playerSound = GetComponent<PlayerSound>();
        playerStats = GetComponent<PlayerStats>();

        shotInterval = playerWeapon.fireRate;
    }

    // Update is called once per frame
    void Update () {
        //Debug.Log(shooting);
	}
}
