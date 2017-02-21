using UnityEngine;
using System.Collections;

[RequireComponent(typeof(PlayerController), typeof(WeaponManager))]
public class PlayerInput : MonoBehaviour {

    PlayerController playerController;
    WeaponManager weaponManager;

    float xMov, yMov, zMov;
    float xRot, yRot;

    bool leftMouse, rightMouse;
    bool canMove = true, canLook = true, canHover = true, canShoot = true;

	// Use this for initialization
	void Start () {
        playerController = GetComponent<PlayerController>();
        weaponManager = GetComponent<WeaponManager>();
		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;
    }
	
	// Update is called once per frame
	void Update () {
        AbleToMove(GameCountdown.CountdownComplete());
        AbleToHover(GameCountdown.CountdownComplete());
        AbleToShoot(GameCountdown.CountdownComplete());

        UpdateMoveInput();
        UpdateHoverInput();
        UpdateLookInput();

        playerController.ApplyMovement(xMov, zMov);
        playerController.ApplyHover(yMov);
        playerController.ApplyCamRotation(xRot, yRot);

        UpdateGunInput();
        if (leftMouse) {
            weaponManager.StartShooting();
            if (Cursor.lockState != CursorLockMode.Locked) {
                Cursor.lockState = CursorLockMode.Locked;
				Cursor.visible = false;
			}

        } else {
            weaponManager.StopShooting();
        }

    }

    public void UpdateMoveInput () {
        if (!canMove)
            return;

        xMov = Input.GetAxisRaw("Horizontal");
        zMov = Input.GetAxisRaw("Vertical");
    }

    public void UpdateHoverInput () {
        if (!canHover)
            return;

        yMov = Input.GetAxisRaw("Jump");
    }

    public void UpdateLookInput () {
        if (!canLook)
            return;

        xRot = Input.GetAxisRaw("Mouse Y");
        yRot = Input.GetAxisRaw("Mouse X");
    }

    public void UpdateGunInput () {
        if (!canShoot)
            return;

        leftMouse = Input.GetButton("Fire1");
        rightMouse = Input.GetButton("Fire2");
    }

    #region Accessor functions
    public void AbleToMove (bool _canMove) {
        canMove = _canMove;
    }

    public void AbleToLook(bool _canLook) {
        canMove = _canLook;
    }

    public void AbleToHover (bool _canHover) {
        canHover = _canHover;
    }

    public void AbleToShoot (bool _canShoot) {
        canShoot = _canShoot;
    }
    #endregion
}
