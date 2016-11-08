using UnityEngine;
using System.Collections;
using System;

public class MapGenerator : MonoBehaviour {

    public int width;
    public int height;
    public int depth;
    public int smoothingIterations = 5;

    public string seed;
    public bool useRandomSeed;

    [Range(0, 100)]
    public int randomFillPercent;

    public int[,,] map;

    void Start () {
        GenerateMap();
    }

    void Update() {
        if (Input.GetMouseButtonDown(0)) {
            GenerateMap();
        }
    }

    void GenerateMap() {
        map = new int[width, height, depth];
        RandomFillMap();

        for (int i = 0; i < smoothingIterations; i++) {
            SmoothMap();
        }

        MeshGenerator meshGen = GetComponent<MeshGenerator>();
        meshGen.GenerateMesh(map, 1);
    }

    void RandomFillMap() {
        if (useRandomSeed)
            seed = Time.time.ToString();

        System.Random pseudoRandom = new System.Random(seed.GetHashCode());

        for (int x = 0;  x < width;  x++) {
            for (int y = 0; y < height; y++) {
                for (int z = 0; z < depth; z++) {

                    if (x == 0 || x == width - 1 || y == 0 || y == height - 1 || z == 0 || z == depth - 1) {
                        map[x, y, z] = 1;
                    } else {
                        map[x, y, z] = (pseudoRandom.Next(0, 100) < randomFillPercent) ? 1 : 0;
                    }  
                }
            }
        }
    }

    void SmoothMap() {
        for (int x = 0; x < width; x++) {
            for (int y = 0; y < height; y++) {
                for (int z = 0; z < depth; z++) {
                    int neighbourWallTiles = GetSurroundingWallCount(x, y, z);
                    //Debug.Log(neighbourWallTiles);
                    if (neighbourWallTiles > 14) {
                        map[x, y, z] = 1;
                    } else if (neighbourWallTiles < 12) {
                        map[x, y, z] = 0;
                    }
                }
            }
        }
    }

    int GetSurroundingWallCount(int gridX, int gridY, int gridZ) {
        int wallCount = 0;
        for (int neighbourX = gridX - 1; neighbourX <= gridX+1; neighbourX++) {
            for (int neighbourY = gridY - 1; neighbourY <= gridY + 1; neighbourY++) {
                for (int neighbourZ = gridZ - 1; neighbourZ <= gridZ + 1; neighbourZ++) {

                    if (neighbourX >= 0 && neighbourX < width &&
                        neighbourY >= 0 && neighbourY < height &&
                        neighbourZ >= 0 && neighbourZ < depth) {

                        if (neighbourX != gridX || neighbourY != gridY || neighbourZ != gridZ) {
                            wallCount += map[neighbourX, neighbourY, neighbourZ];
                        }
                    } else {
                        wallCount++;
                    }
                }
            }
        }

        return wallCount;
    }

    /*void OnDrawGizmos () {
        if (map != null) {
            for (int x = 0; x < width; x++) {
                for (int y = 0; y < height; y++) {
                    for (int z = 0; z < depth; z++) {

                        Gizmos.color = (map[x, y, z] == 1) ? Color.black : Color.white;
                        Vector3 pos = new Vector3(x + .5f, y + .5f, z + .5f);
                        Gizmos.DrawCube(pos, Vector3.one);
                    }
                }
            }
        }
    }*/
}
