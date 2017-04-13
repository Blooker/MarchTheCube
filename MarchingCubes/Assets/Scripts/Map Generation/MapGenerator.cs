using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;

/* Following square bracket tags force Unity to add instances of other classes
 * to a GameObject when an instance of this class is added */
[RequireComponent(typeof(CellAutoGenerator))]
[RequireComponent(typeof(MarchingCubes))]
[RequireComponent(typeof(ObjectPlacement))]

public class MapGenerator : MonoBehaviour {

    // Defining variables
    // Square bracket tags change how Unity displays attributes in the inspector

    [SerializeField]
    private Vector3 mapSize;
    [SerializeField]
    private int smoothingIterations;

    [SerializeField]
    private float cubeSize;

    [SerializeField]
    private string seed;

    private CellAutoGenerator cellAutoGenerator;
    private MarchingCubes marchCubesGenerator;
    private ObjectPlacement objectPlacement;


    /* ------------------
     * BUILT-IN FUNCTIONS
     * ------------------ */

    // Use this for initialization
    void Start () {
        cellAutoGenerator = GetComponent<CellAutoGenerator>();
        marchCubesGenerator = GetComponent<MarchingCubes>();
        objectPlacement = GetComponent<ObjectPlacement>();

        MapInfo mapInfo = GameObject.FindObjectOfType<MapInfo>();
        SetSeed(mapInfo.seed);

        Destroy(mapInfo.gameObject);

        GenerateMap();
	}


    /* ----------------
     * CUSTOM FUNCTIONS
     * ---------------- */

    // Generates an entire in-game "cave" map (cell. auto generation, marching cubes and random object placement)
    public void GenerateMap () {
		cellAutoGenerator.GenerateCellAuto ( mapSize, smoothingIterations, seed );
        marchCubesGenerator.CreateCubeGrid( cellAutoGenerator.GetCellMap(), cubeSize );
        marchCubesGenerator.GenerateMesh();

        objectPlacement.LocateFloorTilesPos( marchCubesGenerator.GetCubeGrid(), cubeSize );
        objectPlacement.PlaceRandomObjects ( seed );
    }


    // Sets the map seed
    public void SetSeed(string _seed) {
        seed = _seed;
    }

    // Gets the current map seed
    public string GetSeed () {
        return seed;
    }


    // Sets the map's size (width, height and depth)
    public void SetMapSize(Vector3 _mapSize) {
        mapSize = _mapSize;
    }

    // Gets the current map size
    public Vector3 GetMapSize () {
        return mapSize;
    }

    
    // Sets the number of smoothing iterations to be run on the cellular automaton
    public void SetSmoothingIterations(int _smoothingIterations) {
        smoothingIterations = _smoothingIterations;
    }

    // Gets the number of smoothing iterations to be run on the cellular automaton
    public int GetSmoothingIterations() {
        return smoothingIterations;
    }
}
