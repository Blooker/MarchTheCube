using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

/// <summary>
/// Class for creating and manipulating a cellular automaton (represented by a 3D array of 1s and 0s)
/// </summary>

public class CellAutoGenerator : MonoBehaviour {

    /* NOTE: The cell map is used later on in the marching cubes algorithm to generate the "cave" mesh.
     * Inside of the cell map array,
     * 1s represent physical walls and all empty space OUTSIDE of the play area,
     * 0s represent empty space INSIDE the play area */

    // Defining variables
    // Square bracket tags change how Unity displays attributes in the inspector

    [SerializeField]
    [Range(0, 100)]
    private int randomFillPercent;

    private int width;
    private int height;
    private int depth;

    private int[,,] cellMap;


    /* ----------------
     * CUSTOM FUNCTIONS
     * ---------------- */

    /* Generates a cellular automaton with cells randomly set to on or off (1 or 0, respectively)
     * Then applies a set number of smoothing iterations */
    public void GenerateCellAuto(Vector3 size, int smoothingIterations, string seed) {

        width = (int)size.x;
        height = (int)size.y;
        depth = (int)size.z;

        cellMap = new int[width, height, depth];
        RandomFillMap(seed);

        int[,,] tempCellMap = new int[width, height, depth];

        for (int i = 0; i < smoothingIterations; i++) {
            SmoothMap(cellMap, tempCellMap);
        }

        RemoveAllButLargestRoom();
    }

    /* Fills 3D cellMap array with 1s and 0s to indicate on/off cells
     * If x, y or z pos is on the edge of the map, cell is always set to on
     * Else, cell is randomly chosen using randomFillPercent to dictate the amount cells set to on */
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

    /* "Smooths" the random cellMap array
     * (makes each cell "more like its neighbours", pushing them outwards
     * and creating curved shapes when turned into mesh later on) */
    void SmoothMap(int[,,] cellMapIn, int[,,] cellMapOut) {
        for (int x = 0; x < width; x++) {
            for (int y = 0; y < height; y++) {
                for (int z = 0; z < depth; z++) {
                    int neighbourWallTiles = GetSurroundingWallCount(cellMapIn, x, y, z);
                    if (neighbourWallTiles >= 15) {
                        cellMapOut[x, y, z] = 1;
                    } else if (neighbourWallTiles < 13) {
                        cellMapOut[x, y, z] = 0;
                    }
                }
            }
        }

        cellMap = cellMapOut;
    }

    // Returns the number of cells set to on (1) surrounding a specific cell
    int GetSurroundingWallCount(int[,,] cellMapIn, int gridX, int gridY, int gridZ) {
        int wallCount = 0;
        
        /* --------------------------------------------------------------
         * UNROLLED FOR LOOP TO OPTIMISE PERFORMANCE, ORIGINAL LOOP BELOW
         * -------------------------------------------------------------- */
        
        /*for (int neighbourX = gridX - 1; neighbourX <= gridX + 1; neighbourX++) {
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
        }*/

        IncrementIfValid(ref wallCount, cellMapIn, gridX-1, gridY-1, gridZ);
        IncrementIfValid(ref wallCount, cellMapIn, gridX-1, gridY-1, gridZ+1);
 
        IncrementIfValid(ref wallCount, cellMapIn, gridX-1, gridY, gridZ-1);
        IncrementIfValid(ref wallCount, cellMapIn, gridX-1, gridY, gridZ);
        IncrementIfValid(ref wallCount, cellMapIn, gridX-1, gridY, gridZ+1);

        IncrementIfValid(ref wallCount, cellMapIn, gridX-1, gridY+1, gridZ-1);
        IncrementIfValid(ref wallCount, cellMapIn, gridX-1, gridY+1, gridZ);
        IncrementIfValid(ref wallCount, cellMapIn, gridX-1, gridY+1, gridZ+1);

        IncrementIfValid(ref wallCount, cellMapIn, gridX, gridY-1, gridZ-1);
        IncrementIfValid(ref wallCount, cellMapIn, gridX, gridY-1, gridZ);
        IncrementIfValid(ref wallCount, cellMapIn, gridX, gridY-1, gridZ+1);

        IncrementIfValid(ref wallCount, cellMapIn, gridX, gridY, gridZ-1);
        IncrementIfValid(ref wallCount, cellMapIn, gridX, gridY, gridZ);
        IncrementIfValid(ref wallCount, cellMapIn, gridX, gridY, gridZ+1);

        IncrementIfValid(ref wallCount, cellMapIn, gridX, gridY+1, gridZ-1);
        IncrementIfValid(ref wallCount, cellMapIn, gridX, gridY+1, gridZ);
        IncrementIfValid(ref wallCount, cellMapIn, gridX, gridY+1, gridZ+1);

        IncrementIfValid(ref wallCount, cellMapIn, gridX+1, gridY-1, gridZ-1);
        IncrementIfValid(ref wallCount, cellMapIn, gridX+1, gridY-1, gridZ);
        IncrementIfValid(ref wallCount, cellMapIn, gridX+1, gridY-1, gridZ+1);

        IncrementIfValid(ref wallCount, cellMapIn, gridX+1, gridY, gridZ-1);
        IncrementIfValid(ref wallCount, cellMapIn, gridX+1, gridY, gridZ);
        IncrementIfValid(ref wallCount, cellMapIn, gridX+1, gridY, gridZ+1);

        IncrementIfValid(ref wallCount, cellMapIn, gridX+1, gridY+1, gridZ-1);
        IncrementIfValid(ref wallCount, cellMapIn, gridX+1, gridY+1, gridZ);
        IncrementIfValid(ref wallCount, cellMapIn, gridX+1, gridY+1, gridZ+1);

        return wallCount;
    }

    /* Increments wallCount by 1 if specified cell is the border of the cellMap (always 1)
     * Else, increments by whatever value the specified cell is (1 if on, 0 if off) */
    void IncrementIfValid (ref int wallCount, int[,,] cellMapIn, int nX, int nY, int nZ) {
        if (IsInMapRange(nX, nY, nZ)) {
            wallCount += cellMapIn[nX, nY, nZ];
        } else {
            wallCount++;
        }
    }

    /* Gets all separate regions of the cell map (region is a closed off area unconnected to the others)
     * Then, removes all but the largest room (the play area in the game)
     * This is to ensure that the player is able to access every area of the map */
    void RemoveAllButLargestRoom () {
        List<List<Coord>> mapRegions = GetRegions(0);
        List<Room> mapRooms = new List<Room>();

        foreach (List<Coord> mapRegion in mapRegions) {
            mapRooms.Add(new Room(mapRegion, cellMap));
        }

        mapRooms.Sort();
        for (int i = 1; i < mapRooms.Count; i++) {
            foreach(Coord tile in mapRooms[i].cellCoords) {
                cellMap[tile.xPos, tile.yPos, tile.zPos] = 1;
            }
        }
    }

    // Returns a list of all the separate regions in the map of type cellType (type meaning whether the cell is on or off)
    List<List<Coord>> GetRegions (int cellType) {
        List<List<Coord>> regions = new List<List<Coord>>();

        int[,,] mapFlags = new int[width, height, depth];
        int[,,] tempFlags = new int[width, height, depth];
        Queue<Coord> queue = new Queue<Coord>();

        for (int x = 0; x < width; x++) {
            for (int y = 0; y < height; y++) {
                for (int z = 0; z < depth; z++) {

                    if (mapFlags[x,y,z] == 0 && cellMap[x,y,z] == cellType) {
                        List<Coord> newRegion = GetRegionCellCoords(x, y, z, tempFlags, queue);
                        regions.Add(newRegion);

                        foreach (Coord cellCoord in newRegion) {
                            mapFlags[cellCoord.xPos, cellCoord.yPos, cellCoord.zPos] = 1;
                        }
                    }

                }
            }
        }

        return regions;
    }

    // Returns a list of coordinates of cells that are the same type (on or off) and are situated next to each other (a region)
    List<Coord> GetRegionCellCoords (int startX, int startY, int startZ, int[,,] mapFlags, Queue<Coord> queue) {
        List<Coord> cellCoords = new List<Coord>();

        int cellType = cellMap[startX, startY, startZ];

        queue.Enqueue(new Coord(startX, startY, startZ));
        mapFlags[startX, startY, startZ] = 1;

        while (queue.Count > 0) {
            Coord cellCoord = queue.Dequeue();
            cellCoords.Add(cellCoord);

            for (int x = cellCoord.xPos - 1; x <= cellCoord.xPos + 1; x++) {
                for (int y = cellCoord.yPos - 1; y <= cellCoord.yPos + 1; y++) {
                    for (int z = cellCoord.zPos - 1; z <= cellCoord.zPos + 1; z++) {
                        if (IsInMapRange(x, y, z) && (y == cellCoord.yPos || x == cellCoord.xPos || z == cellCoord.zPos)) {
                            if (mapFlags[x,y,z] == 0 && cellMap[x,y,z] == cellType) {
                                mapFlags[x, y, z] = 1;
                                queue.Enqueue(new Coord(x, y, z));
                            }
                        }
                    }
                }
            }
        }

        for (int i = 0; i < cellCoords.Count; i++) {
            var coord = cellCoords[i];
            mapFlags[coord.xPos, coord.yPos, coord.zPos] = 0;
        }

        return cellCoords;
    }
    
    // Returns whether a 3D coordinate is within the cell map's width, height and depth
    bool IsInMapRange (int x, int y, int z) {
        return x >= 0 && x < width && y >= 0 && y < height && z >= 0 && z < depth;
    }

    // A structure used to contain the X, Y and Z coordinates of cells in a cell map
    struct Coord {
        public int xPos;
        public int yPos;
        public int zPos;

        public Coord (int x, int y, int z) {
            xPos = x;
            yPos = y;
            zPos = z;
        }
    }

    // Sets the cell map
	public void SetCellMap (int[,,] _cellMap) {
		cellMap = _cellMap;
	}

    // Gets the current cell map
	public int[,,] GetCellMap () {
		return cellMap;
	}


    /* ----------
     * SUBCLASSES
     * ---------- */

    /// <summary>
    /// Subclass for defining a room
    /// (a section of the map consisting of off cells (empty space) contained within on cells (walls)) 
    /// </summary>
    class Room : IComparable<Room> {

        // Defining variables
        public List<Coord> cellCoords;
        public int roomSize;


        /* ------------
         * CONSTRUCTORS
         * ------------*/

        public Room() {
        }

        public Room(List<Coord> roomCellCoords, int[,,] map) {
            cellCoords = roomCellCoords;
            roomSize = cellCoords.Count;
        }


        /* ------------------
         * BUILT-IN FUNCTIONS
         * ------------------ */

        // Part of the IComparable interface, compares size of this Room instance with size of another (otherRoom)
        public int CompareTo(Room otherRoom) {
            return otherRoom.roomSize.CompareTo(roomSize);
        }
    }
}
