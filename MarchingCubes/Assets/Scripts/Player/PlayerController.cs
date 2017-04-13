using UnityEngine;

/// <summary>
/// Class for registering player input into the player character
/// </summary>

/* Following square bracket tag forces Unity to add instances of other classes
 * to a GameObject when an instance of this class is added */
[RequireComponent(typeof(PlayerMotor))]

public class PlayerController : MonoBehaviour {

    // Defining variables
    // Square bracket tags change how Unity displays attributes in the inspector

    [SerializeField]
	private float speed = 10f;

	[SerializeField]
	private float mouseSens = 3f;

    [SerializeField]
    private float hoverForce;

    [SerializeField]
    private Camera playerCam;

    private PlayerMotor motor;
	private PlayerStats playerStats;


    /* ------------------
     * BUILT-IN FUNCTIONS
     * ------------------ */

    // Use this for initialization
    void Awake () {
        motor = GetComponent<PlayerMotor>();
        playerStats = GetComponent<PlayerStats>();
    }

    // This function is called whenever the player character enters a "trigger" collider
    void OnTriggerEnter(Collider coll) {
        bool destroyTriggerObj = false;

        /* If trigger object is Ammo item, add ammo
         * Else if trigger object is Health item, add health */
        switch (coll.gameObject.tag) {
            case ("Ammo"):
                playerStats.AddAmmo(20);
                destroyTriggerObj = true;
                break;
            case ("Health"):
                playerStats.AddHealth(10);
                destroyTriggerObj = true;
                break;
        }

        if (destroyTriggerObj) {
            ObjectManager.RemoveFromLevelObjects(coll.gameObject);
            Destroy(coll.gameObject);
        }
    }


    /* ----------------
     * CUSTOM FUNCTIONS
     * ---------------- */
    
    // Applies movement force to the player character
    public void ApplyMovement (float xMov, float zMov) {
        // Calculate movement velocity as a 3D vector
        Vector3 _movHorizontal = transform.right * xMov;
        Vector3 _movForward = transform.forward * zMov;

        // Final movement vector
        Vector3 _velocity = (_movHorizontal + _movForward).normalized * speed;

        // Apply movement
        motor.SetMoveVelocity(_velocity);
    }

    // Applies hover force to the player character
    public void ApplyHover (float yMov) {
        // Calculate hover velocity as a 3D vector
        Vector3 _movVertical = transform.up * yMov;

        // Final hover vector
        Vector3 _hoverVelocity = _movVertical * hoverForce;

        // Apply hover
        motor.SetHoverVelocity(_hoverVelocity);
    }

    // Applies rotation force to the player camera
    public void ApplyCamRotation (float xRot, float yRot) {
        // Calculate rotation as a 3D vector (turning around)
        Vector3 _rotation = new Vector3(0f, yRot, 0f) * mouseSens;
        motor.SetPlayerRotation(_rotation);

        // Calculate rotation as a 3D vector
        float _cameraRotationX = xRot * mouseSens;

        // Apply camera rotation
        motor.SetCamRotation(_cameraRotationX);
    }

    // Gets the player camera
    public Camera GetPlayerCam () {
        return playerCam;
    }
}
