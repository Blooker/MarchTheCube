using UnityEngine;
using System.Collections;

/// <summary>
/// Class for handling the player character's physics behaviours (moving, hovering, etc.)
/// </summary>

/* Following square bracket tags force Unity to add instances of other classes
 * to a GameObject when an instance of this class is added */
[RequireComponent(typeof(Rigidbody))]
public class PlayerMotor : MonoBehaviour {

    // Defining variables
    // Square bracket tags change how Unity displays attributes in the inspector

    [SerializeField]
    private float camRotationLimit = 90f;

    private Vector3 moveVelocity, hoverVelocity, rotation = Vector3.zero;

    private float cameraRotationX = 0f;
    private float curCamRotationX = 0f;

    private PlayerStats playerStats;

	private Rigidbody rb;

    private Camera cam;


    /* ------------------
     * BUILT-IN FUNCTIONS
     * ------------------ */

    // Use this for initialization
    void Start () {
		rb = GetComponent<Rigidbody> ();
        cam = GetComponent<PlayerController>().GetPlayerCam();
        playerStats = GetComponent<PlayerStats>();
	}

    // Run every physics iteration
    void FixedUpdate() {
        PerformMovement();
        PerformHover();
        PerformRotation();
    }


    /* ----------------
     * CUSTOM FUNCTIONS
     * ---------------- */

    // Performs movement along the X/Z axis based on player velocity.
    void PerformMovement() {
        rb.velocity = new Vector3(0, rb.velocity.y, 0);

        // If movement velocity is being applied
        if (moveVelocity != Vector3.zero) {
            rb.MovePosition(rb.position + moveVelocity * Time.fixedDeltaTime);
        }
    }

    // Performs movement along the Y axis based on hover velocity.
    void PerformHover () {
        // If hover velocity is being applied
        if (hoverVelocity != Vector3.zero) {
            if (rb.velocity.y <= 15 && playerStats.JetpackTimeLeft())
                rb.AddForce(hoverVelocity);

            playerStats.RemoveJetpackTime(Time.deltaTime);

        } else {
            playerStats.AddJetpackTime(Time.deltaTime/2);
        }
    }

    // Rotates the player character/camera based on rotation/camera rotation respectively.
	void PerformRotation () {
		rb.MoveRotation(rb.rotation * Quaternion.Euler(rotation) );

        if (cam != null) {
            // Set camera X rotation and clamp it
            curCamRotationX -= cameraRotationX;
            curCamRotationX = Mathf.Clamp(curCamRotationX, -camRotationLimit, camRotationLimit);

            // Apply rotation to camera
            cam.transform.localEulerAngles = new Vector3(curCamRotationX, 0, 0);
        }
	}

    /// Sets the player's movement velocity.
    public void SetMoveVelocity(Vector3 _moveVelocity) {
        moveVelocity = _moveVelocity;
    }

    // Sets the player's hover velocity
    public void SetHoverVelocity(Vector3 _hoverVelocity) {
        hoverVelocity = _hoverVelocity;
    }

    /// Sets the player character's rotation.
    public void SetPlayerRotation(Vector3 _rotation) {
        rotation = _rotation;
    }

    /// Sets the player camera's rotation.
    public void SetCamRotation(float _cameraRotationX) {
        cameraRotationX = _cameraRotationX;
    }

    // Gets the player's camera
    public Camera GetPlayerCam() {
        return cam;
    }
}
