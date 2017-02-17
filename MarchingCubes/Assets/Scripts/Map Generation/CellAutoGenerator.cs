using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class CellAutoGenerator : MonoBehaviour {

    //MapGenerator mapGenerator;

    int width;
    int height;
    int depth;

    public CapsuleCollider capColl;

    [Range(0, 100)]
    public int randomFillPercent;

    public int[,,] cellMap;

    void Start () {
        //mapGenerator = GetComponent<MapGenerator>();
    }

    public void GenerateCellAuto(Vector3 size, int smoothingIterations, string seed) {
        //Vector3 mapSize = mapGenerator.GetMapSize();
        //int smoothingIterations = mapGenerator.GetSmoothingIterations();
        //string seed = mapGenerator.GetSeed();

        width = (int)size.x;
        height = (int)size.y;
        depth = (int)size.z;

        cellMap = new int[width, height, depth];
        RandomFillMap(seed);
        RemoveCellsInColl();

        for (int i = 0; i < smoothingIterations; i++) {
            SmoothMap();
        }

        ProcessMap();

        SetCellMap(cellMap);
    }

    void RandomFillMap(string seed) {
        System.Random pseudoRandom = new System.Random(seed.GetHashCode());

        for (int x = 0; x < width; x++) {
            for (int y = 0; y < height; y++) {
                for (int z = 0; z < depth; z++) {

                    if (x == 0 || x == width - 1 || y == 0 || y == height - 1 || z == 0 || z == depth - 1) {
                        cellMap[x, y, z] = 1;
                    } else {
                        cellMap[x, y, z] = (pseudoRandom.Next(0, 100) < randomFillPercent) ? 1 : 0;
                    }
                }
            }
        }
    }

    void RemoveCellsInColl() {
        capColl.gameObject.SetActive(true);

        for (int x = 0; x < width; x++) {
            for (int y = 0; y < height; y++) {
                for (int z = 0; z < depth; z++) {
                    if (capColl.bounds.Contains(new Vector3(x, y, z))) {
                        cellMap[x, y, z] = 0;
                    }
                }
            }
        }

        capColl.gameObject.SetActive(false);
    }

    void SmoothMap() {
        for (int x = 0; x < width; x++) {
            for (int y = 0; y < height; y++) {
                for (int z = 0; z < depth; z++) {
                    //if (capColl.bounds.Contains(new Vector3(x, y, z))) {
                      //  map[x, y, z] = 0;
                    //}

                    int neighbourWallTiles = GetSurroundingWallCount(x, y, z);
                    if (neighbourWallTiles >= 15) {
                        cellMap[x, y, z] = 1;
                    } else if (neighbourWallTiles < 13) {
                        cellMap[x, y, z] = 0;
                    }
                }
            }
        }
    }

    int GetSurroundingWallCount(int gridX, int gridY, int gridZ) {
        int wallCount = 0;
        for (int neighbourX = gridX - 1; neighbourX <= gridX + 1; neighbourX++) {
            for (int neighbourY = gridY - 1; neighbourY <= gridY + 1; neighbourY++) {
                for (int neighbourZ = gridZ - 1; neighbourZ <= gridZ + 1; neighbourZ++) {

                    if (IsInMapRange(neighbourX, neighbourY, neighbourZ)) {
                        if (neighbourX != gridX || neighbourY != gridY || neighbourZ != gridZ) {
                            wallCount += cellMap[neighbourX, neighbourY, neighbourZ];
                        }
                    } else {
                        wallCount++;
                    }
                }
            }
        }

        return wallCount;
    }

    void ProcessMap () {
        List<List<Coord>> roomRegions = GetRegions(0);

        int roomThresholdSize = 1000;
        List<Room> survivingRooms = new List<Room>();

        foreach (List<Coord> roomRegion in roomRegions) {
            if (roomRegion.Count < roomThresholdSize) {
                foreach (Coord tile in roomRegion) {
                    cellMap[tile.tileX, tile.tileY, tile.tileZ] = 1;
                }
            } else {
                survivingRooms.Add(new Room(roomRegion, cellMap));
            }
        }

        survivingRooms.Sort();
        for (int i = 1; i < survivingRooms.Count; i++) {
            foreach(Coord tile in survivingRooms[i].tiles) {
                cellMap[tile.tileX, tile.tileY, tile.tileZ] = 1;
            }
        }
    }

    // Returns a list of all the separate regions in the map
    List<List<Coord>> GetRegions (int tileType) {
        List<List<Coord>> regions = new List<List<Coord>>();

        int[,,] mapFlags = new int[width, height, depth];

        for (int x = 0; x < width; x++) {
            for (int y = 0; y < height; y++) {
                for (int z = 0; z < depth; z++) {

                    if (mapFlags[x,y,z] == 0 && cellMap[x,y,z] == tileType) {
                        List<Coord> newRegion = GetRegionTiles(x, y, z);
                        regions.Add(newRegion);

                        foreach (Coord tile in newRegion) {
                            mapFlags[tile.tileX, tile.tileY, tile.tileZ] = 1;
                        }
                    }

                }
            }
        }

        return regions;
    }

    List<Coord> GetRegionTiles (int startX, int startY, int startZ) {
        List<Coord> tiles = new List<Coord>();

        int[,,] mapFlags = new int[width, height, depth];
        int tileType = cellMap[startX, startY, startZ];

        Queue<Coord> queue = new Queue<Coord>();
        queue.Enqueue(new Coord(startX, startY, startZ));
        mapFlags[startX, startY, startZ] = 1;

        while (queue.Count > 0) {
            Coord tile = queue.Dequeue();
            tiles.Add(tile);

            for (int x = tile.tileX - 1; x <= tile.tileX + 1; x++) {
                for (int y = tile.tileY - 1; y <= tile.tileY + 1; y++) {
                    for (int z = tile.tileZ - 1; z <= tile.tileZ + 1; z++) {
                        if (IsInMapRange(x, y, z) && (y == tile.tileY || x == tile.tileX || z == tile.tileZ)) {
                            if (mapFlags[x,y,z] == 0 && cellMap[x,y,z] == tileType) {
                                mapFlags[x, y, z] = 1;
                                queue.Enqueue(new Coord(x, y, z));
                            }
                        }
                    }
                }
            }
        }

        return tiles;
    }
    
    bool IsInMapRange (int x, int y, int z) {
        return x >= 0 && x < width && y >= 0 && y < height && z >= 0 && z < depth;
    }

    struct Coord {
        public int tileX;
        public int tileY;
        public int tileZ;

        public Coord (int x, int y, int z) {
            tileX = x;
            tileY = y;
            tileZ = z;
        }
    }

    class Room : IComparable<Room>{
        public List<Coord> tiles;
        public int roomSize;

        public Room () {
        }

        public Room(List<Coord> roomTiles, int[,,] map) {
            tiles = roomTiles;
            roomSize = tiles.Count;
        }

        public int CompareTo (Room otherRoom) {
            return otherRoom.roomSize.CompareTo(roomSize);
        }
    }

	public void SetCellMap (int[,,] _cellMap) {
		cellMap = _cellMap;
	}

	public int[,,] GetCellMap () {
		return cellMap;
	}
}
