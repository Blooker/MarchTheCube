using UnityEngine;
using System.Collections;

public class EnemyController : MonoBehaviour {

    [SerializeField]
    private float health;

    ObjectManager objectManager;

    public void DamageEnemy (float damage) {
        health -= damage;

        if (health <= 0) {
            KillEnemy();
        }
    }

    void KillEnemy () {
        ObjectManager.RemoveEnemy(this.gameObject);
        Destroy(this.gameObject);
    }

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
