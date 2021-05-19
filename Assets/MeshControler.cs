using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshControler : MonoBehaviour
{
    public Material material;
    public float a = 0.4f;
    float h;
    public int szer , wys;
    // Start is called before the first frame update
    void Start()
    {
         h= a * Mathf.Sqrt(3) / 2;
        Vector3[] vertices = new Vector3[szer * wys];
        Vector2[] uv = new Vector2[szer * wys];
        int cellNumber = 6 * (szer - 1) * (wys - 1) * 2;
        int[] triangles = new int[cellNumber];
        Vector3[] verticesC = new Vector3[szer * wys*2];
        Vector2[] uvC = new Vector2[szer * wys * 2];
        int[] trianglesC = new int[cellNumber*6];
        int indeksVer = 0;
        int indeksFace = 0;
        for (int i=0; i<wys; i++)
            for (int j = 0; j < szer; j++)
            {
                vertices[indeksVer] = new Vector3(a * (j + 0.5f * (i % 2)), h * i);
                verticesC[indeksVer] = new Vector3(a * (j + 0.5f * (i % 2)), h * i, -1);
                verticesC[indeksVer+ szer * wys] = new Vector3(a * (j + 0.5f * (i % 2)), h * i, 1);

                uv[indeksVer] = new Vector2(a * (j + 0.5f * (i % 2)), h * i);

                uvC[indeksVer] = new Vector2(a * (j + 0.5f * (i % 2)), h * i);
                uvC[indeksVer + szer * wys] = new Vector2(a * (j + 0.5f * (i % 2)), h * i);

                if (j < szer - 1 && i < wys - 1)
                {
                    triangles[indeksFace] = indeksVer;

                    trianglesC[indeksFace] = indeksVer; //1
                    trianglesC[indeksFace + 1] = indeksVer + szer * wys;//1
                    trianglesC[indeksFace + cellNumber] = indeksVer + szer * wys; //2
                    trianglesC[indeksFace + 4 * cellNumber+2] = indeksVer;//5
                    trianglesC[indeksFace + 5 * cellNumber + 1] = indeksVer + szer * wys; //6
                    trianglesC[indeksFace + 5 * cellNumber+2] = indeksVer; //6

                    if (i % 2 == 1)
                    {
                        triangles[indeksFace + 1] = indeksVer + szer + 1;

                        trianglesC[indeksFace + 2] = indeksVer + szer + 1;//1
                        trianglesC[indeksFace + cellNumber+1] = indeksVer + szer + 1+ szer * wys;//2
                        trianglesC[indeksFace + cellNumber + 2] = indeksVer + szer + 1; //2
                        trianglesC[indeksFace + 2* cellNumber] = indeksVer + szer + 1;//3
                        trianglesC[indeksFace + 2 * cellNumber + 1] = indeksVer + szer + 1 + szer * wys;//3
                        trianglesC[indeksFace + 3 * cellNumber] = indeksVer + szer + 1 + szer * wys;//4

                    }
                    else
                    { 
                        triangles[indeksFace + 1] = indeksVer + szer;

                        trianglesC[indeksFace + 2] = indeksVer + szer;//1
                        trianglesC[indeksFace + cellNumber + 1] = indeksVer + szer + szer * wys;//2
                        trianglesC[indeksFace + cellNumber + 2] = indeksVer + szer; //2
                        trianglesC[indeksFace + 2 * cellNumber] = indeksVer + szer ;//3
                        trianglesC[indeksFace + 2 * cellNumber + 1] = indeksVer + szer +  szer * wys;//3
                        trianglesC[indeksFace + 3 * cellNumber] = indeksVer + szer  + szer * wys;//4
                    }
                    triangles[indeksFace+2] = indeksVer+1;

                    trianglesC[indeksFace + 2 * cellNumber + 2] = indeksVer + 1;//3
                    trianglesC[indeksFace + 3 * cellNumber + 1] = indeksVer + szer * wys + 1;//4
                    trianglesC[indeksFace + 3 * cellNumber + 2] = indeksVer + 1;//4
                    trianglesC[indeksFace + 4 * cellNumber] = indeksVer + 1;//5
                    trianglesC[indeksFace + 4 * cellNumber + 1] = indeksVer + szer * wys + 1;//5
                    trianglesC[indeksFace + 5 * cellNumber ] = indeksVer + szer * wys + 1;//6



                    indeksFace += 3;
                    //Debug.Log(indeksFace + ", " + triangles.Length);

                }
                if (j < szer - 1 && i >0)
                {
                    triangles[indeksFace] = indeksVer;

                    trianglesC[indeksFace] = indeksVer; //1
                    trianglesC[indeksFace + 1] = indeksVer + szer * wys;//1
                    trianglesC[indeksFace + cellNumber] = indeksVer + szer * wys; //2
                    trianglesC[indeksFace + 4 * cellNumber + 2] = indeksVer;//5
                    trianglesC[indeksFace + 5 * cellNumber + 1] = indeksVer + szer * wys; //6
                    trianglesC[indeksFace + 5 * cellNumber + 2] = indeksVer; //6

                    triangles[indeksFace + 1] = indeksVer + 1;

                    trianglesC[indeksFace + 2] = indeksVer +  1;//1
                    trianglesC[indeksFace + cellNumber + 1] = indeksVer +  1 + szer * wys;//2
                    trianglesC[indeksFace + cellNumber + 2] = indeksVer +  1; //2
                    trianglesC[indeksFace + 2 * cellNumber] = indeksVer + 1;//3
                    trianglesC[indeksFace + 2 * cellNumber + 1] = indeksVer +  1 + szer * wys;//3
                    trianglesC[indeksFace + 3 * cellNumber] = indeksVer +  1 + szer * wys;//4

                    if (i % 2 == 1)
                    {
                        triangles[indeksFace + 2] = indeksVer - szer + 1;

                        trianglesC[indeksFace + 2 * cellNumber + 2] = indeksVer - szer + 1;//3
                        trianglesC[indeksFace + 3 * cellNumber + 1] = indeksVer - szer + szer * wys + 1;//4
                        trianglesC[indeksFace + 3 * cellNumber + 2] = indeksVer - szer + 1;//4
                        trianglesC[indeksFace + 4 * cellNumber] = indeksVer - szer + 1;//5
                        trianglesC[indeksFace + 4 * cellNumber + 1] = indeksVer - szer + szer * wys + 1;//5
                        trianglesC[indeksFace + 5 * cellNumber] = indeksVer - szer + szer * wys + 1;//6
                    }
                    else
                    {
                        triangles[indeksFace + 2] = indeksVer - szer;

                        trianglesC[indeksFace + 2 * cellNumber + 2] = indeksVer - szer ;//3
                        trianglesC[indeksFace + 3 * cellNumber + 1] = indeksVer - szer + szer * wys ;//4
                        trianglesC[indeksFace + 3 * cellNumber + 2] = indeksVer - szer ;//4
                        trianglesC[indeksFace + 4 * cellNumber] = indeksVer - szer ;//5
                        trianglesC[indeksFace + 4 * cellNumber + 1] = indeksVer - szer + szer * wys ;//5
                        trianglesC[indeksFace + 5 * cellNumber] = indeksVer - szer + szer * wys ;//6
                    }
                    indeksFace += 3;
                    //Debug.Log(indeksFace + ", " + triangles.Length);

                }

                indeksVer++;
            }
        /*
        vertices[0] = new Vector3(0, 0);
        vertices[1] = new Vector3(a, 0);
        vertices[2] = new Vector3(a/2, h);
        vertices[3] = new Vector3(a*3/2, h);

        uv[0] = new Vector2(0, 0);
        uv[1] = new Vector2(a, 0);
        uv[2] = new Vector2(a / 2, h);
        uv[3] = new Vector2(a * 3 / 2, h);
        

        triangles[0] = 0;
        triangles[1] = 2;
        triangles[2] = 1;
        triangles[3] = 1;
        triangles[4] = 2;
        triangles[5] = 3;
        */
        Mesh mesh = new Mesh();
        mesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;
        mesh.vertices = vertices;
        mesh.uv = uv;
        mesh.triangles = triangles;

        Mesh meshC = new Mesh();
        meshC.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;
        meshC.vertices = verticesC;
        meshC.uv = uvC;
        meshC.triangles = trianglesC;

        GameObject gameObject = new GameObject("Mesh", typeof(MeshFilter), typeof(MeshRenderer),typeof(MeshCollider),typeof(groundController));
        gameObject.GetComponent<MeshFilter>().mesh = mesh;
        gameObject.GetComponent<MeshRenderer>().material = material;
        gameObject.GetComponent<MeshCollider>().sharedMesh = meshC;

        gameObject.GetComponent<groundController>().surface = mesh;
        gameObject.GetComponent<groundController>().colliders = meshC;


    }

    // Update is called once per frame
    void Update()
    {
        
    }



}

public class ColliderCreator : MonoBehaviour
{
    private int currentPathIndex = 0;
    private PolygonCollider2D polygonCollider;
    private List<Edge> edges = new List<Edge>();
    private List<Vector2> points = new List<Vector2>();
    private Vector3[] vertices;

    void Start()
    {
        // Get the polygon collider (create one if necessary)
        polygonCollider = GetComponent<PolygonCollider2D>();
        if (polygonCollider == null)
        {
            polygonCollider = gameObject.AddComponent<PolygonCollider2D>();
        }

        // Get the mesh's vertices for use later
        vertices = GetComponent<MeshFilter>().mesh.vertices;

        // Get all edges from triangles
        int[] triangles = GetComponent<MeshFilter>().mesh.triangles;
        for (int i = 0; i < triangles.Length; i += 3)
        {
            edges.Add(new Edge(triangles[i], triangles[i + 1]));
            edges.Add(new Edge(triangles[i + 1], triangles[i + 2]));
            edges.Add(new Edge(triangles[i + 2], triangles[i]));
        }

        // Find duplicate edges
        List<Edge> edgesToRemove = new List<Edge>();
        foreach (Edge edge1 in edges)
        {
            foreach (Edge edge2 in edges)
            {
                if (edge1 != edge2)
                {
                    if (edge1.vert1 == edge2.vert1 && edge1.vert2 == edge2.vert2 || edge1.vert1 == edge2.vert2 && edge1.vert2 == edge2.vert1)
                    {
                        edgesToRemove.Add(edge1);
                    }
                }
            }
        }

        // Remove duplicate edges (leaving only perimeter edges)
        foreach (Edge edge in edgesToRemove)
        {
            edges.Remove(edge);
        }

        // Start edge trace
        edgeTrace(edges[0]);
    }

    void edgeTrace(Edge edge)
    {
        // Add this edge's vert1 coords to the point list
        points.Add(vertices[edge.vert1]);

        // Store this edge's vert2
        int vert2 = edge.vert2;

        // Remove this edge
        edges.Remove(edge);

        // Find next edge that contains vert2
        foreach (Edge nextEdge in edges)
        {
            if (nextEdge.vert1 == vert2)
            {
                edgeTrace(nextEdge);
                return;
            }
        }

        // No next edge found, create a path based on these points
        polygonCollider.pathCount = currentPathIndex + 1;
        polygonCollider.SetPath(currentPathIndex, points.ToArray());

        // Empty path
        points.Clear();

        // Increment path index
        currentPathIndex++;

        // Start next edge trace if there are edges left
        if (edges.Count > 0)
        {
            edgeTrace(edges[0]);
        }
    }
}

class Edge
{
    public int vert1;
    public int vert2;

    public Edge(int Vert1, int Vert2)
    {
        vert1 = Vert1;
        vert2 = Vert2;
    }
}
