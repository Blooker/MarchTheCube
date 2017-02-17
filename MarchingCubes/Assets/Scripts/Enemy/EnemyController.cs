using UnityEngine;
using System.Collections;

public class EnemyController : MonoBehaviour {

    [SerializeField]
	private float health, moveSpeed, followDistance;

    private GameObject player;
	private Rigidbody rigid;
	private bool foundPlayer = false;

    public void DamageEnemy (float damage) {
		foundPlayer = true;
		health -= damage;

        if (health <= 0) {
            KillEnemy();
        }
    }

    public void SetPlayerObject (GameObject playerObj) {
        player = playerObj;
    }

	bool WithinPlayerDistance (Transform player) {
		if (Vector3.Distance(transform.position, player.position) <= followDistance) {
			return true;
		}

		return false;
	}

    void KillEnemy () {
        ObjectManager.RemoveFromLevelObjects(this.gameObject);
        Destroy(this.gameObject);
    }

    void LookAtPlayer (Transform player) {
		//Quaternion oldRot = transform.rotation;
        transform.LookAt(player);
		//Quaternion newRot = transform.rotation;

		//transform.rotation = Quaternion.Slerp(oldRot, newRot, 0.25f);
    }

    void MoveTowardsPlayer (Transform player) {
		rigid.velocity = new Vector3(0, rigid.velocity.y, 0);
		Vector3 enemyVel = new Vector3(transform.forward.x, 0, transform.forward.z);
		rigid.MovePosition(rigid.position + enemyVel * moveSpeed * Time.fixedDeltaTime);
    }

    bool IsInRangeOfPlayer (Vector3 playerPos) {
        bool inRange = false;
        return inRange;
    }

	// Use this for initialization
	void Start () {
		rigid = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update () {
        if(player != null) {
			if (WithinPlayerDistance(player.transform) && !foundPlayer) {
				foundPlayer = true;
			}

			if (foundPlayer) {
				LookAtPlayer(player.transform);
				MoveTowardsPlayer(player.transform);
			}
		}
	}
}
