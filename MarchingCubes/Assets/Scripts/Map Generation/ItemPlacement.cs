using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;

public class ItemPlacement : MonoBehaviour {

	[SerializeField]
	GameObject player;

	[SerializeField]
	GameObject[] items;

    [SerializeField]
    int[] itemPlacementChances;

	List<Vector3> floorTilesPos = new List<Vector3>();
	List<GameObject> enemies = new List<GameObject>();
	
	List<GameObject> ammoItems = new List<GameObject>();
	List<GameObject> healthItems = new List<GameObject>();

	//List<Vector3> floorTilesPos;

    GameObject spawnPoint;
    //List<GameObject> enemies;

    //List<GameObject> ammoItems;
    //List<GameObject> healthItems;
    
    public void PlaceRandomItems (string seed) {
		System.Random pseudoRandom = new System.Random (seed.GetHashCode());
        Debug.Log(itemPlacementChances.Take(2).Sum().ToString());

		for (int i = 0; i < floorTilesPos.Count; i++) {
			GameObject itemToPlace = ItemFromPercentage(pseudoRandom.Next(0, 400));

			if (itemToPlace != null) {
				Vector3 itemPos = new Vector3 (floorTilesPos[i].x, floorTilesPos[i].y + 0.7f, floorTilesPos[i].z);
				GameObject spawnedItem = Instantiate(itemToPlace, itemPos, Quaternion.identity) as GameObject; 

				if (spawnedItem.name == "SpawnPoint(Clone)" && spawnPoint == null)
					spawnPoint = spawnedItem;
			}
		}

		Debug.Log (spawnPoint);

		Vector3 playerPos = new Vector3 (spawnPoint.transform.position.x, spawnPoint.transform.position.y + 2f, spawnPoint.transform.position.z);
		Instantiate (player, playerPos, Quaternion.identity);
    }

    /*GameObject ItemFromPercentage (int percentage) {

		GameObject item = null;

		// Spawn ammo
		if (percentage >= 0 && percentage < 4) {
			item = items[0];
			ammoItems.Add( item );
		}

		// Spawn health
		if (percentage >= 4 && percentage < 8) {
			item = items[1];
			healthItems.Add( item );
		}

		// Spawn player spawn point
		if (percentage >= 8 && percentage < 12 && spawnPoint == null) {
			item = items[2];
			//spawnPoint = item;
		}

		return item;
	}*/

    GameObject ItemFromPercentage(int chance) {
        GameObject itemToPlace = null;

        for (int i = 0; i < itemPlacementChances.Length; i++) {

            if ( i == 0 ) {
                if (chance >= 0 && chance < itemPlacementChances[i]) {
                    itemToPlace = items[i];
                    break;
                }
            } else if ( i == itemPlacementChances.Length ) {
                itemToPlace = null;
                break;
            } else {
                if ( chance >= itemPlacementChances.Take(i).Sum() && chance < itemPlacementChances.Take(i+1).Sum() ) {
                    itemToPlace = items[i];
                    break;
                }
            }

        }

        return itemToPlace;
    }

    #region Floor tile positions methods

        /// <summary>
        /// Gets all floor tiles in the map and adds them to a list of floor tiles.
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

	// Use this for initialization
	void Start () {

	}
}
