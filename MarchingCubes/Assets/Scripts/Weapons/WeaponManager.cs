using UnityEngine;
using System.Collections;

/// <summary>
/// Class for handling weapon behaviours
/// </summary>

public class WeaponManager : MonoBehaviour {

    // Defining variables
    // Square bracket tags change how Unity displays attributes in the inspector

    [SerializeField]
    private Weapon playerWeapon;

    private bool shooting = false;
    private float shotInterval;

    private Camera playerCam;

    private PlayerSound playerSound;
    private PlayerStats playerStats;
    private PlayerUI playerUI;


    /* ------------------
     * BUILT-IN FUNCTIONS
     * ------------------ */

    // Use this for initialization
    void Start() {
        playerCam = GetComponent<PlayerController>().GetPlayerCam();
        playerSound = GetComponent<PlayerSound>();
        playerStats = GetComponent<PlayerStats>();
        playerUI = GetComponent<PlayerUI>();

        shotInterval = playerWeapon.fireRate;
    }


    /* ----------------
     * CUSTOM FUNCTIONS
     * ---------------- */

    // Starts shooting the player's gun
    public void StartShooting () {
        if (playerStats.GetAmmoCount() > 0)
            shooting = true;

        shotInterval += Time.deltaTime;

        if (shotInterval > playerWeapon.fireRate) {
            Shoot();
            shotInterval = 0;
        }
    }

    // Stops shooting the player's gun
    public void StopShooting () {
        shooting = false;
        shotInterval = playerWeapon.fireRate;
    }

    // Shoots a single shot from the player's gun
    public void Shoot() {

        // If the player has ammo
        if (playerStats.GetAmmoCount() > 0) {
            playerSound.PlayPlayerSound(playerWeapon.gunSound);

            RaycastHit rayHit;

            // If raycast has come into contact with a collider
            if (Physics.Raycast(playerCam.transform.position, playerCam.transform.forward, out rayHit, playerWeapon.range)) {
                // If hit enemy, take away health
                if (rayHit.collider.gameObject.tag == "Enemy") {
                    EnemyController enemyController = rayHit.collider.GetComponent<EnemyController>();
                    enemyController.DamageEnemy(playerWeapon.damage);
                    playerUI.CreateDamageIndicator(playerWeapon.damage, rayHit.point);
                }

            }

            // Take away ammo
            playerStats.RemoveAmmo(playerWeapon.bulletsPerShot);
        }
    }

    // Returns whether the player is shooting their gun
    public bool PlayerIsShooting () {
        return shooting;
    }
}
