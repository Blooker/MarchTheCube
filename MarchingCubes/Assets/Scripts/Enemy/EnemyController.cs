using UnityEngine;
using System.Collections;

public class EnemyController : MonoBehaviour {

    [SerializeField]
    private float health;

    private GameObject player;

    public void DamageEnemy (float damage) {
        health -= damage;

        if (health <= 0) {
            KillEnemy();
        }
    }

    public void SetPlayerObject (GameObject playerObj) {
        player = playerObj;
    }

    void KillEnemy () {
        ObjectManager.RemoveFromLevelObjects(this.gameObject);
        Destroy(this.gameObject);
    }

    void LookAtPlayer (Transform player) {
        transform.LookAt(player);
    }

    void MoveTowardsPlayer (Transform player) {

    }

    bool IsInRangeOfPlayer (Vector3 playerPos) {
        bool inRange = false;
        return inRange;
    }

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if(player != null)
            LookAtPlayer(player.transform);
	}
}
