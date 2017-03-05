using UnityEngine;
using System.Collections;

[RequireComponent(typeof(PlayerController), typeof(WeaponManager))]
public class PlayerInput : MonoBehaviour {

    public GameManager gameManager;

    PlayerController playerController;
    WeaponManager weaponManager;

    float xMov, yMov, zMov;
    float xRot, yRot;

    bool leftMouse, rightMouse;
    bool canMove, canLook , canHover, canShoot, canPause;

    bool pauseButton;

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
        UpdateMoveInput();
        UpdateHoverInput();
        UpdateLookInput();
        UpdateGunInput();
        UpdatePauseInput();
    }

    #region Input update functions
    public void UpdateMoveInput () {
        if (!canMove) {
            xMov = 0;
            zMov = 0;

        } else {
            xMov = Input.GetAxisRaw("Horizontal");
            zMov = Input.GetAxisRaw("Vertical");

        }

        playerController.ApplyMovement(xMov, zMov);
    }

    public void UpdateHoverInput() {
        if (!canHover) {
            yMov = 0;
        } else {
            yMov = Input.GetAxisRaw("Jump");
        }

        playerController.ApplyHover(yMov);
    }

    public void UpdateLookInput () {
        if (!canLook) {
            xRot = 0;
            yRot = 0;
        } else {
            xRot = Input.GetAxisRaw("Mouse Y");
            yRot = Input.GetAxisRaw("Mouse X");
        }

        playerController.ApplyCamRotation(xRot, yRot);
    }

    public void UpdateGunInput () {
        if (!canShoot) {
            leftMouse = false;
            rightMouse = false;
        } else {
            leftMouse = Input.GetButton("Fire1");
            rightMouse = Input.GetButton("Fire2");
        }

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

    public void UpdatePauseInput() {
        if (!canPause)
            return;

        pauseButton = Input.GetKeyDown(KeyCode.Escape);

        if (pauseButton)
            gameManager.TogglePause();
    }

    public void EnableAllInput () {
        AbleToMove(true);
        AbleToLook(true);
        AbleToHover(true);
        AbleToShoot(true);
        AbleToPause(true);
    }

    public void DisableAllInput() {
        AbleToMove(false);
        AbleToLook(false);
        AbleToHover(false);
        AbleToShoot(false);
        AbleToPause(false);
    }
    #endregion

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

    public void AbleToPause (bool _canPause) {
        canPause = _canPause;
    }
    #endregion
}
