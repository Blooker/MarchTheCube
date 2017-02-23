using UnityEngine;
using System.Collections;

[RequireComponent(typeof(PlayerController), typeof(WeaponManager))]
public class PlayerInput : MonoBehaviour {

    PlayerController playerController;
    WeaponManager weaponManager;

    float xMov, yMov, zMov;
    float xRot, yRot;

    bool leftMouse, rightMouse;
    bool canMove, canLook , canHover, canShoot ;

	// Use this for initialization
	void Start () {
        playerController = GetComponent<PlayerController>();
        weaponManager = GetComponent<WeaponManager>();

        DisableAllInput();
        AbleToLook(true);

		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;
    }
	
	// Update is called once per frame
	void Update () {
        Debug.Log(canMove.ToString() + ", " + canLook.ToString() + ", " + canHover.ToString() + ", " + canShoot.ToString());

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
        if (!canMove) {
            xMov = 0;
            yMov = 0;
            return;
        }

        xMov = Input.GetAxisRaw("Horizontal");
        zMov = Input.GetAxisRaw("Vertical");
    }

    public void UpdateHoverInput () {
        if (!canHover) {
            yMov = 0;
            return;
        }

        yMov = Input.GetAxisRaw("Jump");
    }

    public void UpdateLookInput () {
        if (!canLook) {
            xRot = 0;
            yRot = 0;
            return;
        }

        xRot = Input.GetAxisRaw("Mouse Y");
        yRot = Input.GetAxisRaw("Mouse X");
    }

    public void UpdateGunInput () {
        if (!canShoot) {
            leftMouse = false;
            rightMouse = false;
            return;
        }

        leftMouse = Input.GetButton("Fire1");
        rightMouse = Input.GetButton("Fire2");
    }

    public void EnableAllInput () {
        AbleToMove(true);
        AbleToLook(true);
        AbleToHover(true);
        AbleToShoot(true);
    }

    public void DisableAllInput() {
        AbleToMove(false);
        AbleToLook(false);
        AbleToHover(false);
        AbleToShoot(false);
    }

    #region Accessor functions
    public void AbleToMove (bool _canMove) {
        canMove = _canMove;
    }

    public void AbleToLook(bool _canLook) {
        canLook = _canLook;
    }

    public void AbleToHover (bool _canHover) {
        canHover = _canHover;
    }

    public void AbleToShoot (bool _canShoot) {
        canShoot = _canShoot;
    }
    #endregion
}
