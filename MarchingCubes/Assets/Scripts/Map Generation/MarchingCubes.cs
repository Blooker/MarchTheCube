using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MarchingCubes : MonoBehaviour {

    public CubeGrid cubeGrid;
    List<Vector3> vertices;
    List<int> triangles;

    public void GenerateMesh(int[,,] map, float cubeSize) {
        cubeGrid = new CubeGrid(map, cubeSize);

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
        MeshFilter meshFilter = GetComponent<MeshFilter>();
        
        meshFilter.mesh = mesh;
        Debug.Log(vertices.Count);

        mesh.vertices = vertices.ToArray();
        mesh.triangles = triangles.ToArray();
        mesh.RecalculateNormals();
        mesh.Optimize();

        MeshCollider meshColl = GetComponent<MeshCollider>();
        meshColl.sharedMesh = mesh;
    }

    void TriangulateCube(Cube cube) {
        cube.SetCaseValue();
        int caseValue = 255 - cube.caseValue;
        int[] cubeMeshData = new int[12];
        Node[] edgeNodes = new Node[12];

        for (int i = caseValue * 15; i < (caseValue * 15) + 12; i++) {
            if (LookupTables.SFaces[i] != -1) {
                cubeMeshData[i - (caseValue * 15)] = LookupTables.SFaces[i];
            } else {
                cubeMeshData[i - (caseValue * 15)] = -1;
            }
        }

        for (int i = 0; i < cubeMeshData.Length; i++) {
            if (cubeMeshData[i] != -1)
                edgeNodes[i] = cube.edgeNodes[cubeMeshData[i]];
            //Debug.Log(cubeMeshData[i]);
        }

        MeshFromPoints(edgeNodes);
    }

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

    void AssignVertices(Node[] points) {
        for (int i = 0; i < points.Length; i++) {
            //Debug.Log(vertices.Count);
            if (points[i] != null) {
                if (points[i].vertexIndex == -1) {
                    points[i].vertexIndex = vertices.Count;
                    vertices.Add(points[i].pos);
                }
            }
            //Debug.Log(points[i].vertexIndex);
        }
    }

    void CreateTriangle(Node a, Node b, Node c) {
        triangles.Add(a.vertexIndex);
        triangles.Add(b.vertexIndex);
        triangles.Add(c.vertexIndex);
    }

    #region Marching Cube related classes

    public class CubeGrid {
        public Cube[,,] cubes;
        public ControlNode[,,] controlNodes;

        public CubeGrid(int[,,] map, float cubeSize) {
            int nodeCountX = map.GetLength(0);
            int nodeCountY = map.GetLength(1);
            int nodeCountZ = map.GetLength(2);

            cubes = new Cube[nodeCountX, nodeCountY, nodeCountZ];
            controlNodes = new ControlNode[nodeCountX + 1, nodeCountY + 1, nodeCountZ + 1];

            CreateCubeGrid(nodeCountX, nodeCountY, nodeCountZ);

            for (int x = 0; x < nodeCountX + 1; x++) {
                for (int y = 0; y < nodeCountY + 1; y++) {
                    for (int z = 0; z < nodeCountZ + 1; z++) {

                        AssignVertex(x, y, z, map, cubeSize);
                        AssignEdgeNodeX(x, y, z, map, cubeSize);
                        AssignEdgeNodeY(x, y, z, map, cubeSize);
                        AssignEdgeNodeZ(x, y, z, map, cubeSize);

                    }
                }
            }
        }

        void CreateCubeGrid(int nodeCountX, int nodeCountY, int nodeCountZ) {
            for (int x = 0; x < nodeCountX; x++) {
                for (int y = 0; y < nodeCountY; y++) {
                    for (int z = 0; z < nodeCountZ; z++) {
                        cubes[x, y, z] = new Cube(null, null, null, null, null, null, null, null,
                                                  null, null, null, null, null, null, null, null,
                                                  null, null, null, null);
                    }
                }
            }
        }

        void AssignVertex(int x, int y, int z, int[,,] map, float cubeSize) {
            Vector3 pos = new Vector3(x * cubeSize, y * cubeSize, z * cubeSize);
            ControlNode controlNode;

            bool v0 = false;
            bool v1 = false;
            bool v2 = false;
            bool v3 = false;
            bool v4 = false;
            bool v5 = false;
            bool v6 = false;
            bool v7 = false;

            if (x != map.GetLength(0) && y != map.GetLength(1) && z != map.GetLength(2)) {
                controlNode = new ControlNode(pos, map[x, y, z] == 1);

                if (cubes[x, y, z].controlNodes[0] == null) {
                    //cubes[x, y, z].controlNodes[0] = controlNode;
                    v0 = true;
                }

            } else {
                controlNode = new ControlNode(pos, false);
            }

            if (x != 0 && z != map.GetLength(2) && y != map.GetLength(1)) {
                if (cubes[x - 1, y, z].controlNodes[1] == null)
                    //cubes[x - 1, y, z].controlNodes[1] = controlNode;
                    v1 = true;

                if (map[x - 1, y, z] == 1)
                    controlNode.active = true;
            }

            if (x != 0 && y != 0 && z != map.GetLength(2)) {
                if (cubes[x - 1, y - 1, z].controlNodes[2] == null)
                    //cubes[x - 1, y - 1, z].controlNodes[2] = controlNode;
                    v2 = true;

                if (map[x - 1, y - 1, z] == 1)
                    controlNode.active = true;
            }

            if (y != 0 && z != map.GetLength(2) && x != map.GetLength(0)) {
                if (cubes[x, y - 1, z].controlNodes[3] == null)
                    //cubes[x, y - 1, z].controlNodes[3] = controlNode;
                    v3 = true;

                if (map[x, y - 1, z] == 1)
                    controlNode.active = true;
            }

            if (z != 0 && y != map.GetLength(1) && x != map.GetLength(0)) {
                if (cubes[x, y, z - 1].controlNodes[4] == null)
                    //cubes[x, y, z - 1].controlNodes[4] = controlNode;
                    v4 = true;

                if (map[x, y, z - 1] == 1)
                    controlNode.active = true;
            }

            if (x != 0 && z != 0 && y != map.GetLength(1)) {
                if (cubes[x - 1, y, z - 1].controlNodes[5] == null)
                    //cubes[x - 1, y, z - 1].controlNodes[5] = controlNode;
                    v5 = true;

                if (map[x - 1, y, z - 1] == 1)
                    controlNode.active = true;
            }

            if (x != 0 && y != 0 && z != 0) {
                if (cubes[x - 1, y - 1, z - 1].controlNodes[6] == null)
                    //cubes[x - 1, y - 1, z - 1].controlNodes[6] = controlNode;
                    v6 = true;

                if (map[x - 1, y - 1, z - 1] == 1)
                    controlNode.active = true;
            }

            if (y != 0 && z != 0 && x != map.GetLength(0)) {
                if (cubes[x, y - 1, z - 1].controlNodes[7] == null)
                    //cubes[x, y - 1, z - 1].controlNodes[7] = controlNode;
                    v7 = true;

                if (map[x, y - 1, z - 1] == 1)
                    controlNode.active = true;
            }

            //Debug.Log(controlNode.active);

            if (v0)
                cubes[x, y, z].controlNodes[0] = controlNode;

            if (v1)
                cubes[x - 1, y, z].controlNodes[1] = controlNode;

            if (v2)
                cubes[x - 1, y - 1, z].controlNodes[2] = controlNode;

            if (v3)
                cubes[x, y - 1, z].controlNodes[3] = controlNode;

            if (v4)
                cubes[x, y, z - 1].controlNodes[4] = controlNode;

            if (v5)
                cubes[x - 1, y, z - 1].controlNodes[5] = controlNode;

            if (v6)
                cubes[x - 1, y - 1, z - 1].controlNodes[6] = controlNode;

            if (v7)
                cubes[x, y - 1, z - 1].controlNodes[7] = controlNode;

            controlNodes[x, y, z] = controlNode;
        }

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

    public class Cube {
        public ControlNode[] controlNodes = new ControlNode[8];
        public Node[] edgeNodes = new Node[12];
        public int caseValue;

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

    public class Node {
        public Vector3 pos;
        public int vertexIndex = -1;

        public Node(Vector3 _pos) {
            pos = _pos;
        }
    }

    public class ControlNode : Node {
        public bool active;

        public ControlNode(Vector3 _pos, bool _active) : base(_pos) {
            active = _active;
        }
    }

    #endregion
}

