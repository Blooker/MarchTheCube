using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;

public class ObjectPlacement : MonoBehaviour {

	[SerializeField]
	GameObject player, playerCanvas;

    [SerializeField]
	GameObject[] objects;
	
    [SerializeField]
    ObjectManager objectManager;

    [SerializeField]
    int[] objectPlacementChances;

    bool playerSpawned = false;

	List<Vector3> floorTilesPos = new List<Vector3>();
    GameObject spawnPoint;

    public void PlaceRandomObjects (string seed) {
        System.Random pseudoRandom = new System.Random (seed.GetHashCode());

		for (int i = 0; i < floorTilesPos.Count; i++) {
            GameObject objectToPlace = null;
            
            do {
                objectToPlace = ObjectFromPercentage(pseudoRandom.Next(0, 4000));
            } while ( !ObjectManager.CanPlaceObject ( objectToPlace ) );


			if (objectToPlace != null) {
				Vector3 objectPos = new Vector3 (floorTilesPos[i].x, floorTilesPos[i].y + 1f, floorTilesPos[i].z);
				GameObject spawnedObject = Instantiate(objectToPlace, objectPos, Quaternion.identity) as GameObject;

                ObjectManager.AddToLevelObjects(spawnedObject);

				if (spawnedObject.name == "SpawnPoint(Clone)" && spawnPoint == null)
					spawnPoint = spawnedObject;
			}
		}

        SpawnPlayer();
    }

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

    GameObject ObjectFromPercentage(int chance) {
        GameObject objectToPlace = null;

        for (int i = 0; i < objectPlacementChances.Length; i++) {

            if ( i == 0 ) {
                if (chance >= 0 && chance < objectPlacementChances[i]) {
                    objectToPlace = objects[i];
                    break;
                }
            } else if ( i == objectPlacementChances.Length ) {
                objectToPlace = null;
                break;
            } else {
                if ( chance >= objectPlacementChances.Take(i).Sum() && chance < objectPlacementChances.Take(i+1).Sum() ) {
                    objectToPlace = objects[i];
                    break;
                }
            }

        }

        return objectToPlace;
    }

    #region Floor tile positions methods

    /// <summary>
    /// Gets positions of all floor tiles in the map and adds them to a list of floor tiles.
    /// </summary>
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
	
	/// <summary>
	/// Returns list of all the floor tiles in the map.
	/// </summary>
	/// <returns>Vector3 list</returns>
	public List<Vector3> GetFloorTilesPos () {
		return floorTilesPos;
	}
	
	public void SetFloorTilesPos (List<Vector3> _floorTilesPos) {
		floorTilesPos = _floorTilesPos;
	}
	#endregion

	// Use this for initialization
	void Start () {

	}
}
