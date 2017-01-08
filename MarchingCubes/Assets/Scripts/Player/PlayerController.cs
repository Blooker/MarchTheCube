using UnityEngine;

[RequireComponent(typeof(PlayerMotor))]
public class PlayerController : MonoBehaviour {

	[SerializeField]
	private float speed = 10f;

	[SerializeField]
	private float mouseSens = 3f;

    [SerializeField]
    private float hoverForce;

	private PlayerMotor motor;

	void Start () {
		motor = GetComponent<PlayerMotor> ();
	}

    void Update() {
        ApplyMovement();
        ApplyCamRotation();
        ApplyHover();
    }

    void ApplyMovement () {
        // Calculate movement velocity as a 3D vector
        float _xMov = Input.GetAxisRaw("Horizontal");
        float _zMov = Input.GetAxisRaw("Vertical");

        Vector3 _movHorizontal = transform.right * _xMov;
        Vector3 _movForward = transform.forward * _zMov;

        // Final movement vector
        Vector3 _velocity = (_movHorizontal + _movForward).normalized * speed;

        // Apply movement
        motor.SetVelocity(_velocity);
    }

    void ApplyHover () {
        // Calculate hover velocity as a 3D vector
        float _yMov = Input.GetAxisRaw("Jump");
        Vector3 _movVertical = transform.up * _yMov;

        // Final hover vector
        Vector3 _hoverVelocity = _movVertical * hoverForce;

        // Apply hover
        motor.SetHoverVelocity(_hoverVelocity);
    }

    void ApplyCamRotation () {
        // Calculate rotation as a 3D vector (turning around)
        float _yRot = Input.GetAxisRaw("Mouse X");
        Vector3 _rotation = new Vector3(0f, _yRot, 0f) * mouseSens;
        motor.SetRotation(_rotation);

        // Calculate rotation as a 3D vector
        float _xRot = Input.GetAxisRaw("Mouse Y");
        float _cameraRotationX = _xRot * mouseSens;

        // Apply camera rotation
        motor.SetCamRotation(_cameraRotationX);
    }
}
