using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;

/// <summary>
/// Class for handling the random placement of objects in the game world
/// </summary>

public class ObjectPlacement : MonoBehaviour {
    /* NOTE: In this script and the ObjectManager script, I define objects as
     * anything that is placed into the game world, such as items, enemies, the player, etc.
     * 
     * This is not to be confused with a GameObject, which is a built in Unity class
     * that acts as a base for all entities spawned in a scene
     */ 

    // Defining variables
    // Square bracket tags change how Unity displays attributes in the inspector

    [SerializeField]
	private GameObject player, playerCanvas;

    [SerializeField]
    private GameObject[] objects;
	
    [SerializeField]
    private ObjectManager objectManager;

    [SerializeField]
    private int[] objectPlacementChances;

    private bool playerSpawned = false;

    private List<Vector3> floorTilesPos = new List<Vector3>();
    private GameObject spawnPoint;


    /* ----------------
     * CUSTOM FUNCTIONS
     * ---------------- */

    // Loops through all the floor tiles in the map and randomly places objects on them
    public void PlaceRandomObjects (string seed) {
        System.Random pseudoRandom = new System.Random (seed.GetHashCode());

		for (int i = 0; i < floorTilesPos.Count; i++) {
            GameObject objectToPlace = null;
            
            // If unable to place object, loop again until valid object is obtained
            do {
                objectToPlace = ObjectFromPercentage(pseudoRandom.Next(0, 4000));
            } while ( !ObjectManager.CanPlaceObject ( objectToPlace ) );


			if (objectToPlace != null) {
				Vector3 objectPos = new Vector3 (floorTilesPos[i].x, floorTilesPos[i].y + 1f, floorTilesPos[i].z);
				GameObject spawnedObject = Instantiate(objectToPlace, objectPos, Quaternion.identity) as GameObject;

                ObjectManager.AddToLevelObjects(spawnedObject);

				if (spawnedObject.tag == "SpawnPoint" && spawnPoint == null)
					spawnPoint = spawnedObject;
			}
		}

        SpawnPlayer();
    }

    // Spawns the player character and sets a reference to them in all enemies in the level
    void SpawnPlayer () {
        Vector3 playerPos = new Vector3(spawnPoint.transform.position.x, spawnPoint.transform.position.y + 2f, spawnPoint.transform.position.z);
        GameObject newPlayer = Instantiate(player, playerPos, Quaternion.identity) as GameObject;
        objectManager.SetCurrentPlayer(newPlayer);

        PlayerInput playerInput = newPlayer.GetComponent<PlayerInput>();
        playerInput.gameManager = objectManager.GetComponent<GameManager>();

		PlayerUI playerUI = newPlayer.GetComponent<PlayerUI>();
        playerUI.SetCanvas(playerCanvas);

        List<GameObject> enemiesInLevel = ObjectManager.GetEnemiesInLevel();
        for (int i = 0; i < enemiesInLevel.Count; i++) {
            EnemyController enemyControl = enemiesInLevel[i].GetComponent<EnemyController>();

            if (enemyControl != null)
                enemyControl.SetPlayerObject(newPlayer);
        }

        playerUI.SetEnemyCounter(enemiesInLevel.Count);
    }

    // Takes a random number and returns an object. The random number is used to determine which type of object is returned
    GameObject ObjectFromPercentage(int randomNum) {
        GameObject objectToPlace = null;

        for (int i = 0; i < objectPlacementChances.Length; i++) {

            if ( i == 0 ) {
                if (randomNum >= 0 && randomNum < objectPlacementChances[i]) {
                    objectToPlace = objects[i];
                    break;
                }
            } else if ( i == objectPlacementChances.Length ) {
                objectToPlace = null;
                break;
            } else {
                if ( randomNum >= objectPlacementChances.Take(i).Sum() && randomNum < objectPlacementChances.Take(i+1).Sum() ) {
                    objectToPlace = objects[i];
                    break;
                }
            }

        }

        return objectToPlace;
    }

    // Gets positions of all floor tiles in the map and adds them to a list of floor tile positions.
    public void LocateFloorTilesPos(CubeGrid cubeGrid, float cubeSize) {
		List<Vector3> _floorTilesPos = new List<Vector3>();
		_floorTilesPos.Clear ();

		for (int x = 0; x < cubeGrid.cubes.GetLength(0); x++) {
			for (int y = 0; y < cubeGrid.cubes.GetLength(1); y++) {
				for (int z = 0; z < cubeGrid.cubes.GetLength(2); z++) {
					if (cubeGrid.cubes[x, y, z].caseValue == 51) {
						Vector3 _floorTilePos = new Vector3((x * cubeSize)+(cubeSize/2), y * cubeSize, (z * cubeSize)+(cubeSize/2));
						_floorTilesPos.Add(_floorTilePos);
					}
					
				}
			}
		}
		
		SetFloorTilesPos (_floorTilesPos);
	}
	
	// Gets list of positions of all floor tiles in the map.
	public List<Vector3> GetFloorTilesPos () {
		return floorTilesPos;
	}
	
    // Sets list of positions of all floor tiles in the map
	public void SetFloorTilesPos (List<Vector3> _floorTilesPos) {
		floorTilesPos = _floorTilesPos;
	}
}
