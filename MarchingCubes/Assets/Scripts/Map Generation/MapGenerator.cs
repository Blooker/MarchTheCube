using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;

[RequireComponent(typeof(CellAutoGenerator))]
[RequireComponent(typeof(MarchingCubes))]
[RequireComponent(typeof(ObjectPlacement))]

public class MapGenerator : MonoBehaviour {

    [SerializeField]
    Vector3 mapSize;
    [SerializeField]
    int smoothingIterations;

    [SerializeField]
    float cubeSize;

    [SerializeField]
    string seed;

    CellAutoGenerator cellAutoGenerator;
    MarchingCubes marchCubesGenerator;
    ObjectPlacement objectPlacement;

	// Use this for initialization
	void Start () {
        cellAutoGenerator = GetComponent<CellAutoGenerator>();
        marchCubesGenerator = GetComponent<MarchingCubes>();
        objectPlacement = GetComponent<ObjectPlacement>();

        MapInfo mapInfo = GameObject.FindObjectOfType<MapInfo>();
        SetSeed(mapInfo.seed);

        GenerateMap();
	}

    /// <summary>
    /// Generates an in-game map.
    /// </summary>
    public void GenerateMap () {
        //Stopwatch stopwatch = new Stopwatch();
        //stopwatch.Start();

		cellAutoGenerator.GenerateCellAuto ( mapSize, smoothingIterations, seed );
        //UnityEngine.Debug.Log(stopwatch.Elapsed.TotalMilliseconds);

        marchCubesGenerator.CreateCubeGrid( cellAutoGenerator.GetCellMap(), cubeSize );
       // UnityEngine.Debug.Log(stopwatch.Elapsed.TotalMilliseconds);
        marchCubesGenerator.GenerateMesh();
        //UnityEngine.Debug.Log(stopwatch.Elapsed.TotalMilliseconds);

        objectPlacement.LocateFloorTilesPos( marchCubesGenerator.GetCubeGrid(), cubeSize );
        //UnityEngine.Debug.Log(stopwatch.Elapsed.TotalMilliseconds);
        objectPlacement.PlaceRandomObjects ( seed );
        //UnityEngine.Debug.Log(stopwatch.Elapsed.TotalMilliseconds);

       // stopwatch.Stop();
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
