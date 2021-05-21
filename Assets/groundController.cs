using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class groundController : MonoBehaviour
{
    public Mesh surface;


    private int currentPathIndex = 0;
    private PolygonCollider2D polygonCollider;
    private List<Edge> edges = new List<Edge>();
    private List<Vector2> points = new List<Vector2>();
    public PolygonCollider2D[] polygonColliders;
    private Vector3[] vertices;
    // public Mesh colliders;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Mouse0))
        {
            if (Input.GetKey(KeyCode.LeftShift))
            {
                Vector3 pointedArea = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                pointedArea.z = 0;

                //Debug.Log(pointedArea);

                int[] triangles = surface.triangles;
              //  int[] trianglesC = colliders.triangles;
                int[] trianglesNew = new int[triangles.Length];

            //    int[] trianglesCNew = new int[trianglesC.Length];

                Vector3[] vertices = surface.vertices;
                List<int> erased = new List<int>();
                for (int i = 0; i < vertices.Length; i++)
                {
                    if (vertices[i].x != -100 && Vector3.Distance(vertices[i], pointedArea) < 1)
                    {
                        vertices[i].x = -100;
                        erased.Add(i);
                    }
                }
                Debug.Log(pointedArea+"; usuniete: "+erased.Count);
                int newCount = 0;
                for (int j = 0; j < triangles.Length; j += 3)
                {
                    bool inRange=false;
                    for (int i = 0; i < erased.Count; i++)
                    {

                        if (triangles[j] == erased[i] || triangles[j + 1] == erased[i] || triangles[j + 2] == erased[i])
                        {
                            inRange = true;
                            break;
                        }

                    }
                    if (!inRange)
                    {
                        trianglesNew[newCount] = triangles[j];
                        trianglesNew[newCount + 1] = triangles[j + 1];
                        trianglesNew[newCount + 2] = triangles[j + 2];

                        polygonColliders[newCount / 3] = polygonColliders[j/3];

                        newCount += 3;
                    }
                    else 
                    {
                        polygonColliders[j / 3].enabled = false;
                    }
                }




                Debug.Log(pointedArea + "; usuniete: " + erased.Count+"; newmesh size: "+newCount);

                surface.triangles = trianglesNew;

                gameObject.GetComponent<MeshFilter>().mesh = surface;


            //    colliders.triangles = trianglesCNew;
                //generate();

                //gameObject.GetComponent<PolygonCollider2D>(). = colliders;

            }

        }

    }

    public void AddColliders()
    {

    }


    public void generate()
    {
        // Get the polygon collider (create one if necessary)
        polygonCollider = GetComponent<PolygonCollider2D>();


        // Get the mesh's vertices for use later
        vertices = surface.vertices;

        // Get all edges from triangles
        int[] triangles = surface.triangles;
        for (int i = 0; i < triangles.Length; i += 3)
        {
            edges.Add(new Edge(triangles[i], triangles[i + 1]));
            edges.Add(new Edge(triangles[i + 1], triangles[i + 2]));
            edges.Add(new Edge(triangles[i + 2], triangles[i]));
        }
        /*
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
        */
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

public class Edge
{
    public int vert1;
    public int vert2;

    public Edge(int Vert1, int Vert2)
    {
        vert1 = Vert1;
        vert2 = Vert2;
    }
}
