using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ItemPlacement : MonoBehaviour {

    GameObject spawnPoint;
    List<GameObject> enemies;

    List<GameObject> ammoItems;
    List<GameObject> healthItems;

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    
    public void PlaceRandomItems (List<Vector3> floorTilesPos) {

    }

    public GameObject GetSpawn() {
        return spawnPoint;
    }

    public List<GameObject> GetEnemies() {
        return enemies;
    }

    public List<GameObject> GetAmmoItems () {
        return ammoItems;
    }

    public List<GameObject> GetHealthItems () {
        return healthItems;
    }
}
