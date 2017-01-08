using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(CellAutoGenerator))]
[RequireComponent(typeof(MarchingCubes))]
[RequireComponent(typeof(ItemPlacement))]

public class MapGenerator : MonoBehaviour {

    int[,,] cellMap;
    CubeGrid cubeGrid;

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

    List<Vector3> floorTilesPos;

    CellAutoGenerator cellAuto;
    MarchingCubes marchCubes;
    ItemPlacement itemPlacement;

	// Use this for initialization
	void Start () {
        cellAuto = GetComponent<CellAutoGenerator>();
        marchCubes = GetComponent<MarchingCubes>();
        itemPlacement = GetComponent<ItemPlacement>();

        floorTilesPos = new List<Vector3>();

        GenerateMap();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void GenerateMap () {
        cellMap = cellAuto.GenerateCellAuto(mapSize);

        cubeGrid = new CubeGrid(cellMap, cubeSize);
        marchCubes.GenerateMesh(cubeGrid);

        SetFloorTilesPos(cubeGrid);

        Debug.Log(GetFloorTilesPos()[0]);
    }

    #region Seed methods
    public void SetSeed(string _seed) {
        seed = _seed;
    }

    public string GetSeed () {
        return seed;
    }
    #endregion

    #region Map size methods
    public void SetMapSize(Vector3 _mapSize) {
        mapSize = _mapSize;
    }

    public Vector3 GetMapSize () {
        return mapSize;
    }
    #endregion

    #region Floor tile positions methods
    void SetFloorTilesPos(CubeGrid cubeGrid) {
        for (int x = 0; x < cubeGrid.cubes.GetLength(0); x++) {
            for (int y = 0; y < cubeGrid.cubes.GetLength(1); y++) {
                for (int z = 0; z < cubeGrid.cubes.GetLength(2); z++) {
                    if (cubeGrid.cubes[x, y, z].caseValue == 51) {
                        Vector3 _floorTilePos = new Vector3(x * cubeSize, y * cubeSize, z * cubeSize);
                        floorTilesPos.Add(_floorTilePos);
                    }
                }
            }
        }
    }

    public List<Vector3> GetFloorTilesPos () {
        return floorTilesPos;
    }
    #endregion
}
