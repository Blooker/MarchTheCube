using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class MapGenerator : MonoBehaviour {

    public int width;
    public int height;
    public int depth;
    public float cubeSize = 5f;
    public int smoothingIterations = 5;

    public string seed;
    public bool useRandomSeed;

    [Range(0, 100)]
    public int randomFillPercent;

    public int[,,] map;

    void Update() {
        if (Input.GetKeyDown(KeyCode.M)) {
            GenerateMap();
        }
    }

    void GenerateMap() {
        map = new int[width, height, depth];
        RandomFillMap();

        for (int i = 0; i < smoothingIterations; i++) {
            SmoothMap();
        }

        ProcessMap();

        MeshGenerator meshGen = GetComponent<MeshGenerator>();
        meshGen.GenerateMesh(map, cubeSize);
    }

    void ProcessMap () {
        Debug.Log("HELLA!!");

        /*List<List<Coord>> wallRegions = GetRegions(1);

        int wallThresholdSize = 50;

        foreach (List<Coord> wallRegion in wallRegions) {
            if (wallRegion.Count < wallThresholdSize) {
                foreach (Coord tile in wallRegion) {
                    map[tile.tileX, tile.tileY, tile.tileZ] = 0;
                }
            }
        }*/

        List<List<Coord>> roomRegions = GetRegions(0);

        int roomThresholdSize = 1000;
        List<Room> survivingRooms = new List<Room>();

        foreach (List<Coord> roomRegion in roomRegions) {
            Debug.Log(roomRegion.Count);
            if (roomRegion.Count < roomThresholdSize) {
                foreach (Coord tile in roomRegion) {
                    map[tile.tileX, tile.tileY, tile.tileZ] = 1;
                }
            } else {
                survivingRooms.Add(new Room(roomRegion, map));
            }
        }

        survivingRooms.Sort();
        survivingRooms[0].isMainRoom = true;
        survivingRooms[0].isAccessibleFromMainRoom = true;

        ConnectClosestRooms(survivingRooms);
    }

    void ConnectClosestRooms (List<Room> allRooms, bool forceAccessibilityFromMainRoom = false) {

        List<Room> roomListA = new List<Room>();
        List<Room> roomListB = new List<Room>();

        if (forceAccessibilityFromMainRoom) {
            foreach (Room room in allRooms) {
                if (room.isAccessibleFromMainRoom) {
                    roomListB.Add(room);
                } else {
                    roomListA.Add(room);
                }
            }
        } else {
            roomListA = allRooms;
            roomListB = allRooms;
        }

        int bestDistance = 0;
        Coord bestTileA = new Coord();
        Coord bestTileB = new Coord();
        Room bestRoomA = new Room();
        Room bestRoomB = new Room();

        bool possibleConnectionFound = false;

        foreach (Room roomA in roomListA) {
            if(!forceAccessibilityFromMainRoom) {
                possibleConnectionFound = false;
            }

            if (roomA.connectedRooms.Count > 0) {
                continue;
            }

            possibleConnectionFound = false;

            foreach (Room roomB in roomListB) {
                if (roomA == roomB || roomA.IsConnected(roomB)) {
                    continue;
                }

                for (int tileIndexA = 0; tileIndexA < roomA.edgeTiles.Count; tileIndexA++) {
                    for (int tileIndexB = 0; tileIndexB < roomB.edgeTiles.Count; tileIndexB++) {
                        Coord tileA = roomA.edgeTiles[tileIndexA];
                        Coord tileB = roomB.edgeTiles[tileIndexB];
                        int distanceBetweenRooms = (int)((tileA.tileX - tileB.tileX) * (tileA.tileX - tileB.tileX) + 
                                                        (tileA.tileY - tileB.tileY) * (tileA.tileY - tileB.tileY) + 
                                                        (tileA.tileZ - tileB.tileZ) * (tileA.tileZ - tileB.tileZ));

                        if (distanceBetweenRooms < bestDistance || !possibleConnectionFound) {
                            bestDistance = distanceBetweenRooms;
                            possibleConnectionFound = true;
                            bestTileA = tileA;
                            bestTileB = tileB;
                            bestRoomA = roomA;
                            bestRoomB = roomB;
                        }

                    }
                }
            }

            if (possibleConnectionFound && !forceAccessibilityFromMainRoom) {
                CreatePassage(bestRoomA, bestRoomB, bestTileA, bestTileB);
            }
        }

        if (possibleConnectionFound && forceAccessibilityFromMainRoom) {
            CreatePassage(bestRoomA, bestRoomB, bestTileA, bestTileB);
            ConnectClosestRooms(allRooms, true);
        }

        if (!forceAccessibilityFromMainRoom) {
            ConnectClosestRooms(allRooms, true);
        }
    }

    void CreatePassage (Room roomA, Room roomB, Coord tileA, Coord tileB) {
        Room.ConnectRooms(roomA, roomB);
        Debug.DrawLine(CoordToWorldPoint(tileA), CoordToWorldPoint(tileB), Color.green, 100); 
    }
    
    /*List<Coord> GetLine (Coord from, Coord to) {
        List<Coord> line = new List<Coord>();

        int x = from.tileX;
        int y = from.tileY;
        int z = from.tileZ;

        int dx = to.tileX - from.tileX;
    }*/

    Vector3 CoordToWorldPoint (Coord tile) {
        return new Vector3(tile.tileX + .5f, tile.tileY + .5f, tile.tileZ + .5f)*cubeSize;
    }

    List<List<Coord>> GetRegions (int tileType) {
        List<List<Coord>> regions = new List<List<Coord>>();

        int[,,] mapFlags = new int[width, height, depth];

        for (int x = 0; x < width; x++) {
            for (int y = 0; y < height; y++) {
                for (int z = 0; z < depth; z++) {

                    if (mapFlags[x,y,z] == 0 && map[x,y,z] == tileType) {
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
        int tileType = map[startX, startY, startZ];

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
                            if (mapFlags[x,y,z] == 0 && map[x,y,z] == tileType) {
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
                    if (neighbourWallTiles >= 15) {
                        map[x, y, z] = 1;
                    } else if (neighbourWallTiles < 13) {
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

                    if (IsInMapRange(neighbourX, neighbourY, neighbourZ)) {
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

    class Room : IComparable<Room> {
        public List<Coord> tiles;
        public List<Coord> edgeTiles;
        public List<Room> connectedRooms;
        public int roomSize;
        public bool isAccessibleFromMainRoom;
        public bool isMainRoom;

        public Room () {
        }

        public Room(List<Coord> roomTiles, int[,,] map) {
            tiles = roomTiles;
            roomSize = tiles.Count;
            connectedRooms = new List<Room>();

            edgeTiles = new List<Coord>();
            foreach (Coord tile in tiles) {
                for (int x = tile.tileX - 1; x <= tile.tileX + 1; x++) {
                    for (int y = tile.tileY - 1; y <= tile.tileY + 1; y++) {
                        for (int z = tile.tileZ - 1; z <= tile.tileZ + 1; z++) {
                            if (x == tile.tileX || y == tile.tileY || z == tile.tileZ) {
                                if (map[x, y, z] == 1) {
                                    edgeTiles.Add(tile);
                                }
                            }
                        }
                    }
                }
            }
        }

        public void SetAccessibleFromMainRoom () {
            if (!isAccessibleFromMainRoom) {
                isAccessibleFromMainRoom = true;
                foreach (Room connectedRoom in connectedRooms) {
                    connectedRoom.SetAccessibleFromMainRoom();
                }
            }
        }

        public static void ConnectRooms(Room roomA, Room roomB) {
            if (roomA.isAccessibleFromMainRoom) {
                roomB.SetAccessibleFromMainRoom();
            } else if (roomB.isAccessibleFromMainRoom) {
                roomA.SetAccessibleFromMainRoom();
            }
            roomA.connectedRooms.Add(roomB);
            roomB.connectedRooms.Add(roomA);
        }

        public bool IsConnected(Room otherRoom) {
            return connectedRooms.Contains(otherRoom);
        }

        public int CompareTo (Room otherRoom) {
            return otherRoom.roomSize.CompareTo(roomSize);
        }
    }
}
