using UnityEngine;
using System.Collections;

/// <summary>
/// Class for handling enemy character attributes and behaviours (health, movement, etc.)
/// </summary>

public class EnemyController : MonoBehaviour {

    // Defining variables
    // Square bracket tags change how Unity displays attributes in the inspector

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

    /* ------------------
     * BUILT-IN FUNCTIONS
     * ------------------ */

    // Use this for initialization
    void Start() {
        rigid = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update() {

        // Enemy can move once initial countdown timer is complete
        AbleToMove(GameCountdown.CountdownComplete());

        attackIntervalTimer -= Time.deltaTime;

        // Various if statements for checking if enemy has found player character
        if (player != null) {
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

        // If enemy falls out of map, they are killed once they fall below a certain point
        if (transform.position.y <= -10)
            KillEnemy();
    }

    // Triggered for every frame that the enemy is touching an object
    void OnCollisionStay(Collision coll) {

        // If touching the player character
        if (coll.gameObject.tag == "Player") {
            DamagePlayer(attackDamage);
        }
    }


    /* ----------------
     * CUSTOM FUNCTIONS
     * ---------------- */

    // Takes away health from the enemy and alerts them of the player
    public void DamageEnemy (float damage) {
		foundPlayer = true;
		health -= damage;

        if (health <= 0) {
            KillEnemy();
        }
    }

    // Sets a reference to the player character object and its components
    public void SetPlayerObject (GameObject playerObj) {
        player = playerObj;
        playerStats = playerObj.GetComponent<PlayerStats>();
        playerWeapon = playerObj.GetComponent<WeaponManager>();
        playerUI = playerObj.GetComponent<PlayerUI>();
    }

    // Enable/disable enemy movement
    public void AbleToMove (bool _canMove) {
        canMove = _canMove;
    }

    // Enable/disable enemy looking
    public void AbleToLook(bool _canLook) {
        canLook = _canLook;
    }

    // Takes away health from the player character at set intervals
    void DamagePlayer (int healthToRemove) {
        if (attackIntervalTimer <= 0) {
            playerStats.RemoveHealth(healthToRemove);
            attackIntervalTimer = attackInterval;
        }
    }

    // Returns whether the enemy is within "following" distance from the player character
    bool WithinPlayerDistance (Transform player) {
		if (Vector3.Distance(transform.position, player.position) <= followDistance) {
			return true;
		}

		return false;
	}

    // Returns whether the enemy has heard the player character firing their gun
    bool HeardPlayerGun (Transform player) {
        if (Vector3.Distance(transform.position, player.position) <= hearingDistance && playerWeapon.PlayerIsShooting()) {
            return true;
        }

        return false;
    }

    // Kills the enemy, removing them from the game world
    void KillEnemy () {
        ObjectManager.RemoveFromLevelObjects(this.gameObject);

        if (player != null) {
            int enemyCount = ObjectManager.GetEnemiesInLevel().Count;
            playerUI.SetEnemyCounter(enemyCount);
        }

        Destroy(this.gameObject);
    }

    // Rotates the enemy to look at the player character
    void LookAtPlayer (Transform player) {
        if (!canLook)
            return;

        transform.LookAt(player);
    }

    // Moves the enemy towards the player character's position
    void MoveTowardsPlayer (Transform player) {
        if (!canMove)
            return;

        rigid.velocity = new Vector3(0, rigid.velocity.y, 0);
		Vector3 enemyVel = new Vector3(transform.forward.x, 0, transform.forward.z);
		rigid.MovePosition(rigid.position + enemyVel * moveSpeed * Time.fixedDeltaTime);
    }
}
