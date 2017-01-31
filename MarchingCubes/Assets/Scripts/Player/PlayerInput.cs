using UnityEngine;
using System.Collections;

[RequireComponent(typeof(PlayerController), typeof(WeaponManager))]
public class PlayerInput : MonoBehaviour {

    PlayerController playerController;
    WeaponManager weaponManager;

    float xMov, yMov, zMov;
    float xRot, yRot;

    bool leftMouse, rightMouse;

	// Use this for initialization
	void Start () {
        playerController = GetComponent<PlayerController>();
        weaponManager = GetComponent<WeaponManager>();
    }
	
	// Update is called once per frame
	void Update () {
        UpdateMoveInput();
        UpdateHoverInput();
        UpdateLookInput();

        playerController.ApplyMovement(xMov, zMov);
        playerController.ApplyHover(yMov);
        playerController.ApplyCamRotation(xRot, yRot);

        UpdateGunInput();
        if (leftMouse) {
            weaponManager.StartShooting();
            if (Cursor.lockState != CursorLockMode.Locked)
                Cursor.lockState = CursorLockMode.Locked;

        } else {
            weaponManager.StopShooting();
        }

    }

    public void UpdateMoveInput () {
        xMov = Input.GetAxisRaw("Horizontal");
        zMov = Input.GetAxisRaw("Vertical");
    }

    public void UpdateHoverInput () {
        yMov = Input.GetAxisRaw("Jump");
    }

    public void UpdateLookInput () {
        xRot = Input.GetAxisRaw("Mouse Y");
        yRot = Input.GetAxisRaw("Mouse X");
    }

    public void UpdateGunInput () {
        leftMouse = Input.GetButton("Fire1");
        rightMouse = Input.GetButton("Fire2");
    }
}
