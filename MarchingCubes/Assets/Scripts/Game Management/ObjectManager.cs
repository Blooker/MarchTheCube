using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ObjectManager : MonoBehaviour {

    static List<GameObject> objectsInLevel = new List<GameObject>();

    static List<GameObject> ammoInLevel = new List<GameObject>();
    static List<GameObject> healthInLevel = new List<GameObject>();
    static List<GameObject> enemiesInLevel = new List<GameObject>();

    static GameObject spawnPoint;
	
    public static void RemoveEnemy (GameObject enemy) {
        enemiesInLevel.Remove(enemy);
        Debug.Log(enemiesInLevel.Count);
    }

    public static void ClearObjects () {
        for (int i = 0; i < objectsInLevel.Count; i++) {
            Destroy(objectsInLevel[i]);
        }
		objectsInLevel.Clear();
		ammoInLevel.Clear ();
		healthInLevel.Clear ();
		enemiesInLevel.Clear ();
		spawnPoint = null;
    }

    public static bool CanPlaceObject (GameObject objectToPlace) {
        if (objectToPlace == null) {
            return true;
        }

        switch (objectToPlace.tag) {
            case "SpawnPoint":
                if (spawnPoint != null) {
                    return false;
                }
                break;
        }

        return true;
    }

    public static void AddToLevelObjects (GameObject levelObject) {
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

	public static void RemoveFromLevelObjects (GameObject levelObject) {
		objectsInLevel.Remove (levelObject);

		switch (levelObject.tag) {
			case "Ammo":
				ammoInLevel.Remove(levelObject);
				break;
				
			case "Health":
				healthInLevel.Remove(levelObject);
				break;
				
			case "Enemy":
				enemiesInLevel.Remove(levelObject);
				break;
				
			case "SpawnPoint":
				spawnPoint = null;
				break;
		}
	}

    public static void SpawnPlayer () {
        
    }

    void InstantiatePlayer () {

    }

    public static List<GameObject> GetEnemiesInLevel () {
        return enemiesInLevel;
    }

	public static int GetAmmoCount() {
		return ammoInLevel.Count;
	}

	public static int GetHealthCount() {
		return healthInLevel.Count;
	}
	
    public static GameObject GetSpawnPoint () {
        return spawnPoint;
    }
	
    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
