using UnityEngine;
using System.Collections;

public class WeaponManager : MonoBehaviour {

    [SerializeField]
    Weapon playerWeapon;
    int ammo = 0;

    Camera playerCam;

    public void Shoot() {
        // Increase timer

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
    }

    // Update is called once per frame
    void Update () {
	    
	}
}
