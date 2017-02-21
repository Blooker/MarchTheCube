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

    private bool canMove = true, canLook = true;

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

    public void AbleToMove (bool _canMove) {
        canMove = _canMove;
    }

    public void AbleToLook(bool _canLook) {
        canLook = _canLook;
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
        if (!canLook)
            return;

        transform.LookAt(player);
    }

    void MoveTowardsPlayer (Transform player) {
        if (!canMove)
            return;

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
        AbleToMove(GameCountdown.CountdownComplete());
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
