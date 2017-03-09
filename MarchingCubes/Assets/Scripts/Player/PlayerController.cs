using UnityEngine;

[RequireComponent(typeof(PlayerMotor))]
public class PlayerController : MonoBehaviour {

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

	void Start () {
		motor = GetComponent<PlayerMotor> ();
		playerStats = GetComponent<PlayerStats> ();
	}

    public void ApplyMovement (float xMov, float zMov) {
        // Calculate movement velocity as a 3D vector
        Vector3 _movHorizontal = transform.right * xMov;
        Vector3 _movForward = transform.forward * zMov;

        // Final movement vector
        Vector3 _velocity = (_movHorizontal + _movForward).normalized * speed;

        //Debug.Log(xMov.ToString() + ", " + zMov.ToString() + "\n" + _velocity.ToString());

        // Apply movement
        motor.SetVelocity(_velocity);
    }

    public void ApplyHover (float yMov) {
        // Calculate hover velocity as a 3D vector
        Vector3 _movVertical = transform.up * yMov;

        // Final hover vector
        Vector3 _hoverVelocity = _movVertical * hoverForce;

        // Apply hover
        motor.SetHoverVelocity(_hoverVelocity);
    }

    public void ApplyCamRotation (float xRot, float yRot) {
        // Calculate rotation as a 3D vector (turning around)
        Vector3 _rotation = new Vector3(0f, yRot, 0f) * mouseSens;
        motor.SetRotation(_rotation);

        // Calculate rotation as a 3D vector
        float _cameraRotationX = xRot * mouseSens;

        // Apply camera rotation
        motor.SetCamRotation(_cameraRotationX);
    }

    public Camera GetPlayerCam () {
        return playerCam;
    }

	void OnTriggerEnter (Collider coll) {
        bool destroyTriggerObj = false;

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
}
