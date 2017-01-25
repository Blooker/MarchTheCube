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

        playerSound.PlayPlayerSound(playerWeapon.gunSound);

        RaycastHit rayHit;

        if (Physics.Raycast(playerCam.transform.position, playerCam.transform.forward, out rayHit, playerWeapon.range)) {
            Debug.Log("You hit " + rayHit.transform.name);

            if (rayHit.collider.gameObject.tag == "Enemy") {

            }

        }

        // If hit enemy, take away health

        // Take away ammo
    }

    void Start () {
        playerCam = GetComponent<PlayerController>().GetPlayerCam();
        playerSound = GetComponent<PlayerSound>();
        shotInterval = playerWeapon.fireRate;
    }

    // Update is called once per frame
    void Update () {
	    
	}
}
