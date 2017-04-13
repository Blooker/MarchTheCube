using UnityEngine;
using System.Collections;

/// <summary>
/// Class that listens for player input (from a keyboard and mouse)
/// </summary>

/* Following square bracket tags force Unity to add instances of other classes
 * to a GameObject when an instance of this class is added */
[RequireComponent(typeof(PlayerController))]
[RequireComponent(typeof(WeaponManager))]

public class PlayerInput : MonoBehaviour {

    // Defining variables

    public GameManager gameManager;

    private PlayerController playerController;
    private WeaponManager weaponManager;

    private float xMov, yMov, zMov;
    private float xRot, yRot;

    private bool leftMouse, rightMouse;
    private bool canMove, canLook , canHover, canShoot, canPause;

    private bool pauseButton;


    /* ------------------
     * BUILT-IN FUNCTIONS
     * ------------------ */

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


    /* ----------------
     * CUSTOM FUNCTIONS
     * ---------------- */

    #region Input update functions

    // Checks for player movement input
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

    // Checks for player hover input
    public void UpdateHoverInput() {
        if (!canHover) {
            yMov = 0;
        } else {
            yMov = Input.GetAxisRaw("Jump");
        }

        playerController.ApplyHover(yMov);
    }

    // Checks for player camera look input
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

    // Checks for player gun shooting input
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

    // Checks for pause menu input
    public void UpdatePauseInput() {
        if (!canPause)
            return;

        pauseButton = Input.GetKeyDown(KeyCode.Escape);

        if (pauseButton)
            gameManager.TogglePause();
    }

    // Enables all player input
    public void EnableAllInput () {
        AbleToMove(true);
        AbleToLook(true);
        AbleToHover(true);
        AbleToShoot(true);
        AbleToPause(true);
    }

    // Disables all player input
    public void DisableAllInput() {
        AbleToMove(false);
        AbleToLook(false);
        AbleToHover(false);
        AbleToShoot(false);
        AbleToPause(false);
    }
    #endregion

    #region Accessor functions
    // Sets whether the player can move
    public void AbleToMove (bool _canMove) {
        canMove = _canMove;
    }

    // Sets whether the player can look
    public void AbleToLook(bool _canLook) {
        canLook = _canLook;
    }

    // Sets whether the player can hover
    public void AbleToHover (bool _canHover) {
        canHover = _canHover;
    }

    // Sets whether the player can shoot
    public void AbleToShoot (bool _canShoot) {
        canShoot = _canShoot;
    }

    // Sets whether the player can pause
    public void AbleToPause (bool _canPause) {
        canPause = _canPause;
    }
    #endregion
}
