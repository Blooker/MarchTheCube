using UnityEngine;
using System.Collections;

public class CameraMove : MonoBehaviour {

    Vector3 moveDir;
    public float moveSpeed;

    void Start() {
        moveDir = new Vector3();
    }

    void Update () {
        moveDir *= 0.05f;
        moveDir += Vector3.right * Input.GetAxis("Horizontal") * moveSpeed;
        moveDir += Vector3.forward * Input.GetAxis("Vertical") * moveSpeed;

        transform.Translate(moveDir);
    }
}
