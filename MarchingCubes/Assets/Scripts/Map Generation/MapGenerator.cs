using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(CellAutoGenerator))]
[RequireComponent(typeof(MarchingCubes))]
[RequireComponent(typeof(ObjectPlacement))]

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

    CellAutoGenerator cellAuto;
    MarchingCubes marchCubes;
    ObjectPlacement objectPlacement;

	// Use this for initialization
	void Start () {
        cellAuto = GetComponent<CellAutoGenerator>();
        marchCubes = GetComponent<MarchingCubes>();
        objectPlacement = GetComponent<ObjectPlacement>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    /// <summary>
    /// Generates an in-game map.
    /// </summary>
    public void GenerateMap () {
		cellAuto.GenerateCellAuto ( mapSize, smoothingIterations, seed );
        marchCubes.CreateCubeGrid( cellAuto.GetCellMap(), cubeSize );

        marchCubes.GenerateMesh();
		
        objectPlacement.LocateFloorTilesPos( marchCubes.GetCubeGrid(), cubeSize );
		objectPlacement.PlaceRandomObjects ( seed );
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
