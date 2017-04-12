using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Class for manipulating and keeping track of data regarding objects in the level (enemies, items, the player, etc.)
/// </summary>

public class ObjectManager : MonoBehaviour {

    // Defining variables

    private static List<GameObject> objectsInLevel = new List<GameObject>();

    private static List<GameObject> ammoInLevel = new List<GameObject>();
    private static List<GameObject> healthInLevel = new List<GameObject>();

    private static List<GameObject> enemiesInLevel = new List<GameObject>();

    private static GameObject spawnPoint;

    private static Vector3 playerLastVelocity;
    private static List<Vector3> enemiesLastVelocity = new List<Vector3>();

    private GameObject currentPlayer;


    /* ----------------
     * CUSTOM FUNCTIONS
     * ---------------- */

    // Returns whether or not an object can be placed in the game world based on certain conditions
    public static bool CanPlaceObject(GameObject objectToPlace) {

        // If object is empty space (null), it can be placed
        if (objectToPlace == null) {
            return true;
        }

        switch (objectToPlace.tag) {

            // If SpawnPoint has already been created, another one cannot be created
            case "SpawnPoint":
                if (spawnPoint != null) {
                    return false;
                }
                break;
        }

        return true;
    }

    // Adds an object in the game world to their respective list of objects
    public static void AddToLevelObjects(GameObject levelObject) {
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

    // Removes an object in the game world from their respective list of objects
    public static void RemoveFromLevelObjects(GameObject levelObject) {
        objectsInLevel.Remove(levelObject);

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

    // Removes objects from the level AND their respective object lists
    public static void ClearAllLevelObjects() {
        for (int i = 0; i < objectsInLevel.Count; i++) {
            Destroy(objectsInLevel[i]);
        }

        objectsInLevel.Clear();
        ammoInLevel.Clear();
        healthInLevel.Clear();
        enemiesInLevel.Clear();
        spawnPoint = null;
    }

    // Destroys the current player character object
    public void ClearCurrentPlayer() {
        Destroy(currentPlayer);
        currentPlayer = null;
    }

    // Stops any physics objects in the level from moving (enemies and the player)
    public void FreezePhysicsObjects() {
        Rigidbody playerRigid = currentPlayer.GetComponent<Rigidbody>();

        playerLastVelocity = playerRigid.velocity;
        playerRigid.constraints = RigidbodyConstraints.FreezeAll;

        enemiesLastVelocity.Clear();

        for (int i = 0; i < enemiesInLevel.Count; i++) {
            Rigidbody enemyRigid = enemiesInLevel[i].GetComponent<Rigidbody>();
            enemiesLastVelocity.Add(enemyRigid.velocity);
            enemyRigid.constraints = RigidbodyConstraints.FreezePosition;
        }
    }

    // Allows physics objects in the level to move again (enemies and the player)
    public void UnfreezePhysicsObjects() {
        Rigidbody playerRigid = currentPlayer.GetComponent<Rigidbody>();

        playerRigid.constraints = RigidbodyConstraints.FreezeRotation;
        playerRigid.velocity = playerLastVelocity;

        for (int i = 0; i < enemiesInLevel.Count; i++) {
            Rigidbody enemyRigid = enemiesInLevel[i].GetComponent<Rigidbody>();
            enemyRigid.constraints = RigidbodyConstraints.None;
            enemyRigid.velocity = enemiesLastVelocity[i];
        }
    }

    // Returns a list of enemies in the level
    public static List<GameObject> GetEnemiesInLevel() {
        return enemiesInLevel;
    }

    // Returns a list of ammo items (red boxes) in the level
    public static int GetAmmoCount() {
        return ammoInLevel.Count;
    }

    // Returns a list of health items (green boxes) in the level
    public static int GetHealthCount() {
        return healthInLevel.Count;
    }

    // Returns the object situated at the point where the player was spawned into the level
    public static GameObject GetSpawnPoint() {
        return spawnPoint;
    }

    // Returns the current player character object
    public GameObject GetCurrentPlayer() {
        return currentPlayer;
    }

    // Sets the current player character object
    public void SetCurrentPlayer(GameObject player) {
        currentPlayer = player;
    }
}
