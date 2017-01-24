using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMotor : MonoBehaviour {

	private Vector3 moveVelocity, hoverVelocity, rotation = Vector3.zero;

    private float cameraRotationX = 0f;
    private float curCamRotationX = 0f;


    [SerializeField]
    private float camRotationLimit = 85f;

    [SerializeField]
    private float hoverTimeLimit = 5f;
    private float curHoverTime = 0f;

	private Rigidbody rb;

    private Camera cam;

	void Start () {
		rb = GetComponent<Rigidbody> ();
        cam = GetComponent<PlayerController>().GetPlayerCam();
	}

    // Run every physics iteration
    void FixedUpdate() {
        PerformMovement();
        PerformHover();
        PerformRotation();
    }

    /// Sets the player's velocity.
    public void SetVelocity (Vector3 _moveVelocity) {
		moveVelocity = _moveVelocity;
	}

    // Sets the player's hover velocity
    public void SetHoverVelocity (Vector3 _hoverVelocity) {
        hoverVelocity = _hoverVelocity;
    }

	/// Sets the player's rotation.
	public void SetRotation (Vector3 _rotation) {
		rotation = _rotation;
	}

    /// Sets the player's rotation.
    public void SetCamRotation (float _cameraRotationX) {
        cameraRotationX = _cameraRotationX;
    }

    public Camera GetPlayerCam () {
        return cam;
    }

    // Performs movement along the X/Z axis based on player velocity.
    void PerformMovement () {
        rb.velocity = new Vector3(0, rb.velocity.y, 0);
		if (moveVelocity != Vector3.zero)
			rb.MovePosition(rb.position + moveVelocity * Time.fixedDeltaTime);
	}

    // Performs movement along the Y axis based on hover velocity.
    void PerformHover () {
        // If hover velocity is being applied
        if (hoverVelocity != Vector3.zero) {
            if (rb.velocity.y <= 15 && curHoverTime < hoverTimeLimit)
                rb.AddForce(hoverVelocity);

            if (curHoverTime < hoverTimeLimit)
                curHoverTime += Time.deltaTime;

        } else {
            if (curHoverTime > 0)
                curHoverTime -= Time.deltaTime;

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
}
