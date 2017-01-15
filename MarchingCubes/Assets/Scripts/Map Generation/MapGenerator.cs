using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(CellAutoGenerator))]
[RequireComponent(typeof(MarchingCubes))]
[RequireComponent(typeof(ItemPlacement))]

public class MapGenerator : MonoBehaviour {

	// TEST SEED : 2.013698

    [SerializeField]
    Vector3 mapSize;
    [SerializeField]
    int smoothingIterations;

    [SerializeField]
    float cubeSize;

    [SerializeField]
    string seed;
    [SerializeField]
    bool useRandomSeed;

    CellAutoGenerator cellAuto;
    MarchingCubes marchCubes;
    ItemPlacement itemPlacement;

	// Use this for initialization
	void Start () {
        cellAuto = GetComponent<CellAutoGenerator>();
        marchCubes = GetComponent<MarchingCubes>();
        itemPlacement = GetComponent<ItemPlacement>();
		
        GenerateMap();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    /// <summary>
    /// Generates an in-game map.
    /// </summary>
    public void GenerateMap () {
        if (useRandomSeed)
            seed = Time.time.ToString();

		cellAuto.GenerateCellAuto ( mapSize, smoothingIterations, seed );
        marchCubes.CreateCubeGrid( cellAuto.GetCellMap(), cubeSize );

        marchCubes.GenerateMesh();
		
        itemPlacement.LocateFloorTilesPos( marchCubes.GetCubeGrid(), cubeSize );
		itemPlacement.PlaceRandomItems ( seed );
    }

    #region Seed methods

    /// <summary>
    /// Used to set seed to a string value.
    /// </summary>
    /// <param name="_seed">The seed value.</param>
    public void SetSeed(string _seed) {
        seed = _seed;
    }

    /// <summary>
    /// Gets the map's current seed.
    /// </summary>
    /// <returns>String seed</returns>
    public string GetSeed () {
        return seed;
    }
    #endregion

    #region Cube grid methods

    /*/// <summary>
    /// Gets the map's current cube grid.
    /// </summary>
    /// <returns>Instance of CubeGrid class</returns>
    public CubeGrid GetCubeGrid () {
        return cubeGrid;
    }*/
    #endregion

    #region Use random seed methods

    /// <summary>
    /// Used to set whether the map is generated randomly or not.
    /// </summary>
    /// <param name="_useRandomSeed">Determines whether the map uses a random seed.</param>
    public void SetUseRandomSeed (bool _useRandomSeed) {
        useRandomSeed = _useRandomSeed;
    }

    /// <summary>
    /// Returns true if map has been randomly generated, false otherwise. 
    /// </summary>
    /// <returns>Boolean value</returns>
    public bool GetUseRandomSeed() {
        return useRandomSeed;
    }
    #endregion

    #region Map size methods

    /// <summary>
    /// Used to set map size to a Vector3 value.
    /// </summary>
    /// <param name="_mapSize">The width, height and depth of the map.</param>
    public void SetMapSize(Vector3 _mapSize) {
        mapSize = _mapSize;
    }

    /// <summary>
    /// Gets the current map size
    /// </summary>
    /// <returns>Vector3 map size</returns>
    public Vector3 GetMapSize () {
        return mapSize;
    }
    #endregion

    #region Smoothing iterations methods
    
    /// <summary>
    /// Used to set smoothing iterations to an integer value.
    /// </summary>
    /// <param name="_smoothingIterations">The number of smoothing iterations.</param>
    public void SetSmoothingIterations(int _smoothingIterations) {
        smoothingIterations = _smoothingIterations;
    }

    /// <summary>
    /// Gets the number of smoothing iterations used on the cellular automaton.
    /// </summary>
    /// <returns>Integer smoothing iterations</returns>
    public int GetSmoothingIterations() {
        return smoothingIterations;
    }
    #endregion
}
