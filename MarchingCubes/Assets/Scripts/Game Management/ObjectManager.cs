using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ObjectManager : MonoBehaviour {

    List<GameObject> objectsInLevel = new List<GameObject>();

    List<GameObject> ammoInLevel = new List<GameObject>();
    List<GameObject> healthInLevel = new List<GameObject>();
    List<GameObject> enemiesInLevel = new List<GameObject>();

    GameObject spawnPoint;

    public void ClearObjects () {
        for (int i = 0; i < objectsInLevel.Count; i++) {
            Destroy(objectsInLevel[i]);
            objectsInLevel.RemoveAt(i);
        }
    }

    public void AddToLevelObjects (GameObject levelObject) {
        objectsInLevel.Add(levelObject);

        switch (levelObject.tag) {
            case "Ammo":
                ammoInLevel.Add(levelObject);
                break;

            case "Health":
                healthInLevel.Add(levelObject);
                break;

            case "Enemy":
                enemiesInLevel.Add(levelObject);
                break;

            case "SpawnPoint":
                spawnPoint = levelObject;
                break;
        }
    }

    public GameObject GetSpawnPoint () {
        return spawnPoint;
    }

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
