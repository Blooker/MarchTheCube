using UnityEngine;
using System.Collections;

public class EnemyController : MonoBehaviour {

    [SerializeField]
    private float health, moveSpeed;
    [SerializeField]
    private int attackDamage;
    [SerializeField]
    private float attackInterval, followDistance, hearingDistance;

    private GameObject player;
    private PlayerStats playerStats;
    private WeaponManager playerWeapon;
    private PlayerUI playerUI;

	private Rigidbody rigid;
	private bool foundPlayer = false;

    private float attackIntervalTimer = 0;

    public void DamageEnemy (float damage) {
		foundPlayer = true;
		health -= damage;

        if (health <= 0) {
            KillEnemy();
        }
    }

    public void SetPlayerObject (GameObject playerObj) {
        player = playerObj;
        playerStats = playerObj.GetComponent<PlayerStats>();
        playerWeapon = playerObj.GetComponent<WeaponManager>();
        playerUI = playerObj.GetComponent<PlayerUI>();
    }

    void DamagePlayer (int healthToRemove) {
        if (attackIntervalTimer <= 0) {
            playerStats.RemoveHealth(healthToRemove);
            attackIntervalTimer = attackInterval;
        }
    }

    void OnCollisionStay(Collision coll) {
        if (coll.gameObject.tag == "Player") {
            DamagePlayer(attackDamage);
        }
    }

    bool WithinPlayerDistance (Transform player) {
		if (Vector3.Distance(transform.position, player.position) <= followDistance) {
			return true;
		}

		return false;
	}

    bool HeardPlayerGun (Transform player) {
        if (Vector3.Distance(transform.position, player.position) <= hearingDistance && playerWeapon.PlayerIsShooting()) {
            return true;
        }

        return false;
    }

    void KillEnemy () {
        ObjectManager.RemoveFromLevelObjects(this.gameObject);

        if (player != null) {
            int enemyCount = ObjectManager.GetEnemiesInLevel().Count;
            playerUI.SetEnemyCounter(enemyCount);
        }

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
        attackIntervalTimer -= Time.deltaTime;

        if(player != null) {
			if (WithinPlayerDistance(player.transform) && !foundPlayer) {
				foundPlayer = true;
			}

            if (HeardPlayerGun(player.transform) && !foundPlayer) {
                foundPlayer = true;
            }

			if (foundPlayer) {
				LookAtPlayer(player.transform);
				MoveTowardsPlayer(player.transform);
			}
		}
	}
}
