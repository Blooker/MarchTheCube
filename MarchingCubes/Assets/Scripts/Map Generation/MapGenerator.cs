using UnityEngine;
using System.Collections;

[RequireComponent(typeof(CellAutoGenerator))]
[RequireComponent(typeof(MarchingCubes))]
[RequireComponent(typeof(ItemPlacement))]

public class MapGenerator : MonoBehaviour {

    string seed;
    //Cube[] floorTiles;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void GenerateMap () {

    }

    string GetSeed () {
        return seed;
    }

    void SetSeed (string _seed) {
        seed = _seed;
    }
}
