using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Class for containing my implementation of the marching cubes algorithm
/// Generates a 3D mesh using an inputted cell grid array (cellular automaton)
/// </summary>

public class MarchingCubes : MonoBehaviour {

    // Defining variables

    private CubeGrid cubeGrid;
    private List<Vector3> vertices;
    private List<int> triangles;


    /* ----------------
     * CUSTOM FUNCTIONS
     * ---------------- */

    // Generates a 3D "cave" mesh using the current cube grid
    public void GenerateMesh() {
        vertices = new List<Vector3>();
        triangles = new List<int>();

        for (int x = 0; x < cubeGrid.cubes.GetLength(0); x++) {
            for (int y = 0; y < cubeGrid.cubes.GetLength(1); y++) {
                for (int z = 0; z < cubeGrid.cubes.GetLength(2); z++) {
                    TriangulateCube(cubeGrid.cubes[x, y, z]);
                }
            }
        }

        Mesh mesh = new Mesh();
        mesh.Clear();
        MeshFilter meshFilter = GetComponent<MeshFilter>();
        
        meshFilter.mesh = mesh;

        mesh.vertices = vertices.ToArray();
        mesh.triangles = triangles.ToArray();
        mesh.RecalculateNormals();

        MeshCollider meshColl = GetComponent<MeshCollider>();
        meshColl.sharedMesh = mesh;
    }

    // Creates a triangle based on a cube's case value (using lookup tables from LookupTables class)
    void TriangulateCube(Cube cube) {
        cube.SetCaseValue();

        // The cube's case value inverted (by subtracting it from 255)
        int caseValue = 255 - cube.caseValue;

        // Will hold the indexes of the cube's edge vertices that will be used as mesh vertices
        int[] cubeEdgeIndexes = new int[12];

        // Will hold the edge nodes that will be used as mesh vertices
        Node[] edgeNodes = new Node[12];

        /* Attempts to find cube case in ambigCases lookup table
         * returns index if found, -1 if not found */
        int ambigCaseIndex = Array.IndexOf(LookupTables.ambigCases, caseValue);

        for (int i = caseValue * 15; i < (caseValue * 15) + 12; i++) {

            /* If case value is found in list of ambiguous cases AND alt. face array's edge node index != -1 at position i,
             * add alt. face edge node index to cube edge indexes
             * 
             * Else, if standard face array's edge node index != -1 at position i,
             * add standard face edge node index to cube edge indexes
             * 
             * Else, add -1 to cube edge indexes */
            if (ambigCaseIndex != -1 && LookupTables.AFaces[i] != -1) {
                cubeEdgeIndexes[i - (caseValue * 15)] = LookupTables.AFaces[i];
            } else if (LookupTables.SFaces[i] != -1) {
                cubeEdgeIndexes[i - (caseValue * 15)] = LookupTables.SFaces[i];
            } else {
                cubeEdgeIndexes[i - (caseValue * 15)] = -1;
            }
        }

        for (int i = 0; i < cubeEdgeIndexes.Length; i++) {
            if (cubeEdgeIndexes[i] != -1)
                edgeNodes[i] = cube.edgeNodes[cubeEdgeIndexes[i]];
        }

        MeshFromPoints(edgeNodes);
    }

    // Takes a list of points (edge nodes) and creates 1 or more mesh triangles using them
    void MeshFromPoints(params Node[] points) {
        AssignVertices(points);

        if (points[0] != null)
            CreateTriangle(points[0], points[1], points[2]);
        if (points[3] != null)
            CreateTriangle(points[3], points[4], points[5]);
        if (points[6] != null)
            CreateTriangle(points[6], points[7], points[8]);
        if (points[9] != null)
            CreateTriangle(points[9], points[10], points[11]);
    }

    // Takes a list of points, assigns them indexes and adds them to a list of mesh vertices
    void AssignVertices(Node[] points) {
        for (int i = 0; i < points.Length; i++) {
            if (points[i] != null) {
                if (points[i].vertexIndex == -1) {
                    points[i].vertexIndex = vertices.Count;
                    vertices.Add(points[i].pos);
                }
            }
        }
    }

    // Takes three edge nodes and adds their vertex indexes to a list of mesh triangle indexes
    void CreateTriangle(Node a, Node b, Node c) {
        triangles.Add(a.vertexIndex);
        triangles.Add(b.vertexIndex);
        triangles.Add(c.vertexIndex);
    }

    // Creates a new cube grid instance using a previously generated cell map (cellular automaton) and sets it as the current cube grid
    public void CreateCubeGrid (int[,,] cellMap, float cubeSize) {
		CubeGrid _cubeGrid = new CubeGrid (cellMap, cubeSize);
		SetCubeGrid (_cubeGrid);
	}

	// Sets the cube grid
	public void SetCubeGrid (CubeGrid _cubeGrid) {
		cubeGrid = _cubeGrid;
	}

	// Gets the current cube grid
	public CubeGrid GetCubeGrid () {
		return cubeGrid;
	}
}

#region Marching Cube related classes

/// <summary>
/// Class for defining and generating a grid of cube objects with control/edge nodes
/// </summary>

public class CubeGrid {

    // Defining variables

    public Cube[,,] cubes;
    public ControlNode[,,] controlNodes;

    private float cubeSize;
    private int[,,] cellMap;

    /* ------------
     * CONSTRUCTORS
     * ------------*/

    public CubeGrid(int[,,] _cellMap, float _cubeSize) {
        cellMap = _cellMap;
        cubeSize = _cubeSize;

        int nodeCountX = cellMap.GetLength(0);
        int nodeCountY = cellMap.GetLength(1);
        int nodeCountZ = cellMap.GetLength(2);

        cubes = new Cube[nodeCountX, nodeCountY, nodeCountZ];
        controlNodes = new ControlNode[nodeCountX + 1, nodeCountY + 1, nodeCountZ + 1];

        // Fills cubes array with empty Cube instances
        for (int x = 0; x < nodeCountX; x++) {
            for (int y = 0; y < nodeCountY; y++) {
                for (int z = 0; z < nodeCountZ; z++) {
                    cubes[x, y, z] = new Cube();
                }
            }
        }

        // Assigns vertices and edge nodes to all cubes in cube grid
        for (int x = 0; x < nodeCountX + 1; x++) {
            for (int y = 0; y < nodeCountY + 1; y++) {
                for (int z = 0; z < nodeCountZ + 1; z++) {

                    AssignControlNode(x, y, z, cellMap, cubeSize);
                    AssignEdgeNodeX(x, y, z, cellMap, cubeSize);
                    AssignEdgeNodeY(x, y, z, cellMap, cubeSize);
                    AssignEdgeNodeZ(x, y, z, cellMap, cubeSize);

                }
            }
        }
    }


    /* ----------------
     * CUSTOM FUNCTIONS
     * ---------------- */

    /* Places a control node at (x, y, z) and sets it to active or inactive based on the surrounding control nodes
     * Then, finds cubes that the control node is touching and sets a reference to itself in those cubes */
    void AssignControlNode(int x, int y, int z, int[,,] map, float cubeSize) {
        Vector3 pos = new Vector3(x, y, z) * cubeSize;
        ControlNode controlNode;

        List<bool> vertexFlags = new List<bool>();
        for (int i=0; i<8; i++) { vertexFlags.Add(false); }

        if (x != map.GetLength(0) && y != map.GetLength(1) && z != map.GetLength(2)) {
            controlNode = new ControlNode(pos, map[x, y, z] == 1);

            if (cubes[x, y, z].controlNodes[0] == null)
                vertexFlags[0] = true;

        } else {
            controlNode = new ControlNode(pos, false);
        }

        if (x != 0 && z != map.GetLength(2) && y != map.GetLength(1)) {
            if (cubes[x - 1, y, z].controlNodes[1] == null)
                vertexFlags[1] = true;

            if (map[x - 1, y, z] == 1)
                controlNode.active = true;
        }

        if (x != 0 && y != 0 && z != map.GetLength(2)) {
            if (cubes[x - 1, y - 1, z].controlNodes[2] == null)
                vertexFlags[2] = true;

            if (map[x - 1, y - 1, z] == 1)
                controlNode.active = true;
        }

        if (y != 0 && z != map.GetLength(2) && x != map.GetLength(0)) {
            if (cubes[x, y - 1, z].controlNodes[3] == null)
                vertexFlags[3] = true;

            if (map[x, y - 1, z] == 1)
                controlNode.active = true;
        }

        if (z != 0 && y != map.GetLength(1) && x != map.GetLength(0)) {
            if (cubes[x, y, z - 1].controlNodes[4] == null)
                vertexFlags[4] = true;

            if (map[x, y, z - 1] == 1)
                controlNode.active = true;
        }

        if (x != 0 && z != 0 && y != map.GetLength(1)) {
            if (cubes[x - 1, y, z - 1].controlNodes[5] == null)
                vertexFlags[5] = true;

            if (map[x - 1, y, z - 1] == 1)
                controlNode.active = true;
        }

        if (x != 0 && y != 0 && z != 0) {
            if (cubes[x - 1, y - 1, z - 1].controlNodes[6] == null)
                vertexFlags[6] = true;

            if (map[x - 1, y - 1, z - 1] == 1)
                controlNode.active = true;
        }

        if (y != 0 && z != 0 && x != map.GetLength(0)) {
            if (cubes[x, y - 1, z - 1].controlNodes[7] == null)
                vertexFlags[7] = true;

            if (map[x, y - 1, z - 1] == 1)
                controlNode.active = true;
        }

        if (vertexFlags[0])
            cubes[x, y, z].controlNodes[0] = controlNode;

        if (vertexFlags[1])
            cubes[x - 1, y, z].controlNodes[1] = controlNode;

        if (vertexFlags[2])
            cubes[x - 1, y - 1, z].controlNodes[2] = controlNode;

        if (vertexFlags[3])
            cubes[x, y - 1, z].controlNodes[3] = controlNode;

        if (vertexFlags[4])
            cubes[x, y, z - 1].controlNodes[4] = controlNode;

        if (vertexFlags[5])
            cubes[x - 1, y, z - 1].controlNodes[5] = controlNode;

        if (vertexFlags[6])
            cubes[x - 1, y - 1, z - 1].controlNodes[6] = controlNode;

        if (vertexFlags[7])
            cubes[x, y - 1, z - 1].controlNodes[7] = controlNode;

        controlNodes[x, y, z] = controlNode;
    }


    // Places an edge node on a cube's edge (offset in x axis)
    void AssignEdgeNodeX(int x, int y, int z, int[,,] map, float cubeSize) {

        Vector3 pos = new Vector3(x * cubeSize + 0.5f, y * cubeSize, z * cubeSize);
        Node edgeNode = new Node(pos);

        if (x != map.GetLength(0)) {

            if (y != map.GetLength(1) && z != map.GetLength(2)) {
                if (cubes[x, y, z].edgeNodes[0] == null) {
                    cubes[x, y, z].edgeNodes[0] = edgeNode;
                }
            }

            if (y != 0 && z != map.GetLength(2)) {
                if (cubes[x, y - 1, z].edgeNodes[2] == null) {
                    cubes[x, y - 1, z].edgeNodes[2] = edgeNode;
                }
            }

            if (z != 0 && y != map.GetLength(1)) {
                if (cubes[x, y, z - 1].edgeNodes[4] == null) {
                    cubes[x, y, z - 1].edgeNodes[4] = edgeNode;
                }
            }

            if (y != 0 && z != 0) {
                if (cubes[x, y - 1, z - 1].edgeNodes[6] == null) {
                    cubes[x, y - 1, z - 1].edgeNodes[6] = edgeNode;
                }
            }
        }
    }


    // Places an edge node on a cube's edge (offset in y axis)
    void AssignEdgeNodeY(int x, int y, int z, int[,,] map, float cubeSize) {

        Vector3 pos = new Vector3(x * cubeSize, y * cubeSize + 0.5f, z * cubeSize);
        Node edgeNode = new Node(pos);

        if (y != map.GetLength(1)) {

            if (x != map.GetLength(0) && z != map.GetLength(2)) {
                if (cubes[x, y, z].edgeNodes[3] == null) {
                    cubes[x, y, z].edgeNodes[3] = edgeNode;
                }
            }

            if (x != 0 && z != map.GetLength(2)) {
                if (cubes[x - 1, y, z].edgeNodes[1] == null) {
                    cubes[x - 1, y, z].edgeNodes[1] = edgeNode;
                }
            }

            if (z != 0 && x != 0) {
                if (cubes[x - 1, y, z - 1].edgeNodes[5] == null) {
                    cubes[x - 1, y, z - 1].edgeNodes[5] = edgeNode;
                }
            }

            if (z != 0 && x != map.GetLength(0)) {
                if (cubes[x, y, z - 1].edgeNodes[7] == null) {
                    cubes[x, y, z - 1].edgeNodes[7] = edgeNode;
                }
            }
        }
    }


    // Places an edge node on a cube's edge (offset in z axis)
    void AssignEdgeNodeZ(int x, int y, int z, int[,,] map, float cubeSize) {

        Vector3 pos = new Vector3(x * cubeSize, y * cubeSize, z * cubeSize + 0.5f);
        Node edgeNode = new Node(pos);

        if (z != map.GetLength(2)) {

            if (x != map.GetLength(0) && y != map.GetLength(1)) {
                if (cubes[x, y, z].edgeNodes[8] == null) {
                    cubes[x, y, z].edgeNodes[8] = edgeNode;
                }
            }

            if (x != 0 && y != map.GetLength(1)) {
                if (cubes[x - 1, y, z].edgeNodes[9] == null) {
                    cubes[x - 1, y, z].edgeNodes[9] = edgeNode;
                }
            }

            if (y != 0 && x != map.GetLength(0)) {
                if (cubes[x, y - 1, z].edgeNodes[10] == null) {
                    cubes[x, y - 1, z].edgeNodes[10] = edgeNode;
                }
            }

            if (x != 0 && y != 0) {
                if (cubes[x - 1, y - 1, z].edgeNodes[11] == null) {
                    cubes[x - 1, y - 1, z].edgeNodes[11] = edgeNode;
                }
            }
        }
    }
}

/// <summary>
/// A class for containing a Cube's control/edge nodes and its case value
/// </summary>

public class Cube {

    // Defining variables

    public ControlNode[] controlNodes = new ControlNode[8];
    public Node[] edgeNodes = new Node[12];
    public int caseValue;


    /* ------------
     * CONSTRUCTORS
     * ------------*/

    public Cube () {

    }

    public Cube(ControlNode v0, ControlNode v1, ControlNode v2, ControlNode v3,
                    ControlNode v4, ControlNode v5, ControlNode v6, ControlNode v7,
                    Node e0, Node e1, Node e2, Node e3, Node e4, Node e5,
                    Node e6, Node e7, Node e8, Node e9, Node e10, Node e11) {

        controlNodes[0] = v0;
        controlNodes[1] = v1;
        controlNodes[2] = v2;
        controlNodes[3] = v3;
        controlNodes[4] = v4;
        controlNodes[5] = v5;
        controlNodes[6] = v6;
        controlNodes[7] = v7;

        edgeNodes[0] = e0;
        edgeNodes[1] = e1;
        edgeNodes[2] = e2;
        edgeNodes[3] = e3;
        edgeNodes[4] = e4;
        edgeNodes[5] = e5;
        edgeNodes[6] = e6;
        edgeNodes[7] = e7;
        edgeNodes[8] = e8;
        edgeNodes[9] = e9;
        edgeNodes[10] = e10;
        edgeNodes[11] = e11;
    }


    /* ----------------
     * CUSTOM FUNCTIONS
     * ---------------- */

    /* Generates an integer case value based on which control nodes are active
     * and sets it as the cube's case value */
    public void SetCaseValue() {
        caseValue = 0;

        if (controlNodes[0].active)
            caseValue += 1;

        if (controlNodes[1].active)
            caseValue += 2;

        if (controlNodes[2].active)
            caseValue += 4;

        if (controlNodes[3].active)
            caseValue += 8;

        if (controlNodes[4].active)
            caseValue += 16;

        if (controlNodes[5].active)
            caseValue += 32;

        if (controlNodes[6].active)
            caseValue += 64;

        if (controlNodes[7].active)
            caseValue += 128;
    }
}


/// <summary>
/// Class for defining a node object (used for edge nodes)
/// </summary>

public class Node {

    // Defining variables
    public Vector3 pos;
    public int vertexIndex = -1;

    /* -----------
     * CONSTRUCTOR
     * -----------*/

    public Node(Vector3 _pos) {
        pos = _pos;
    }
}

/// <summary>
/// Class for defining a node that can be set to active and inactive (inherits from Node class)
/// </summary>

public class ControlNode : Node {

    // Defining variables
    public bool active;

    /* -----------
     * CONSTRUCTOR
     * -----------*/

    public ControlNode(Vector3 _pos, bool _active) : base(_pos) {
        active = _active;
    }
}

#endregion