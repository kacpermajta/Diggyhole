using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class groundController : MonoBehaviour
{
    public Mesh surface;
    public Mesh colliders;
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
                int[] trianglesC = colliders.triangles;
                int[] trianglesNew = new int[triangles.Length];

                int[] trianglesCNew = new int[trianglesC.Length];

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

                        trianglesCNew[newCount] = trianglesC[j];
                        trianglesCNew[newCount + triangles.Length] = trianglesC[j + triangles.Length];
                        trianglesCNew[newCount + 2 * triangles.Length] = trianglesC[j + 2 * triangles.Length];
                        trianglesCNew[newCount + 3 * triangles.Length] = trianglesC[j + 3 * triangles.Length];
                        trianglesCNew[newCount + 4 * triangles.Length] = trianglesC[j + 4 * triangles.Length];
                        trianglesCNew[newCount + 5 * triangles.Length] = trianglesC[j + 5 * triangles.Length];

                        trianglesCNew[newCount + 1] = trianglesC[j];
                        trianglesCNew[newCount + 1 + triangles.Length] = trianglesC[j + 1 + triangles.Length];
                        trianglesCNew[newCount + 1 + 2 * triangles.Length] = trianglesC[j + 1 + 2 * triangles.Length];
                        trianglesCNew[newCount + 1 + 3 * triangles.Length] = trianglesC[j + 1 + 3 * triangles.Length];
                        trianglesCNew[newCount + 1 + 4 * triangles.Length] = trianglesC[j + 1 + 4 * triangles.Length];
                        trianglesCNew[newCount + 1 + 5 * triangles.Length] = trianglesC[j + 1 + 5 * triangles.Length];

                        trianglesCNew[newCount + 2] = trianglesC[j];
                        trianglesCNew[newCount + 2 + triangles.Length] = trianglesC[j + 2 + triangles.Length];
                        trianglesCNew[newCount + 2 + 2 * triangles.Length] = trianglesC[j + 2 + 2 * triangles.Length];
                        trianglesCNew[newCount + 2 + 3 * triangles.Length] = trianglesC[j + 2 + 3 * triangles.Length];
                        trianglesCNew[newCount + 2 + 4 * triangles.Length] = trianglesC[j + 2 + 4 * triangles.Length];
                        trianglesCNew[newCount + 2 + 5 * triangles.Length] = trianglesC[j + 2 + 5 * triangles.Length];

                        newCount += 3;
                    }
                }
                /*
                newCount = 0;
                for (int j = 0; j < trianglesC.Length; j += 3)
                {
                    bool inRange = false;
                    for (int i = 0; i < erased.Count; i++)
                    {

                        if (trianglesC[j] == erased[i] || trianglesC[j + 1] == erased[i] || trianglesC[j + 2] == erased[i])
                        {
                            inRange = true;
                            break;
                        }

                    }
                    if (!inRange)
                    {
                        trianglesCNew[newCount] = trianglesC[j];
                        trianglesCNew[newCount + 1] = trianglesC[j + 1];
                        trianglesCNew[newCount + 2] = trianglesC[j + 2];
                        /*
                        trianglesCNew[newCount] = trianglesC[j];
                        trianglesCNew[newCount + 1] = trianglesC[j + 1];
                        trianglesCNew[newCount + 2] = trianglesC[j + 2];
                        trianglesCNew[newCount + triangles.Length] = trianglesC[j + triangles.Length];
                        trianglesCNew[newCount + triangles.Length + 1] = trianglesC[j + triangles.Length + 1];
                        trianglesCNew[newCount + triangles.Length + 2] = trianglesC[j + triangles.Length + 2];
                        trianglesCNew[newCount + 2 * triangles.Length] = trianglesC[j + 2 * triangles.Length];
                        trianglesCNew[newCount + 2 * triangles.Length + 1] = trianglesC[j + 2 * triangles.Length + 1];
                        trianglesCNew[newCount + 2 * triangles.Length + 2] = trianglesC[j + 2 * triangles.Length + 2];
                        trianglesCNew[newCount + 3 * triangles.Length] = trianglesC[j + 3 * triangles.Length];
                        trianglesCNew[newCount + 3 * triangles.Length + 1] = trianglesC[j + 3 * triangles.Length + 1];
                        trianglesCNew[newCount + 3 * triangles.Length + 2] = trianglesC[j + 3 * triangles.Length + 2];
                        trianglesCNew[newCount + 4 * triangles.Length] = trianglesC[j + 4 * triangles.Length];
                        trianglesCNew[newCount + 4 * triangles.Length + 1] = trianglesC[j + 4 * triangles.Length + 1];
                        trianglesCNew[newCount + 4 * triangles.Length + 2] = trianglesC[j + 4 * triangles.Length + 2];
                        trianglesCNew[newCount + 5 * triangles.Length] = trianglesC[j + 5 * triangles.Length];
                        trianglesCNew[newCount + 5 * triangles.Length + 1] = trianglesC[j + 5 * triangles.Length + 1];
                        trianglesCNew[newCount + 5 * triangles.Length + 2] = trianglesC[j + 5 * triangles.Length + 2];
                        

                        newCount += 3;
                    }
                }*/


                Debug.Log(pointedArea + "; usuniete: " + erased.Count+"; newmesh size: "+newCount);

                surface.triangles = trianglesNew;

                gameObject.GetComponent<MeshFilter>().mesh = surface;

                colliders.triangles = trianglesCNew;

                gameObject.GetComponent<MeshCollider>().sharedMesh = colliders;

            }

        }

    }
    /*
    public void OnMouseDown()
    {
        if(Input.GetKey(KeyCode.LeftShift))
        {
            Vector3 pointedArea = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            pointedArea.z = 0;

            Debug.Log(pointedArea);

            int[] triangles = surface.triangles;
            int[] trianglesNew = new int[triangles.Length];

            Vector3[] vertices = surface.vertices;
            List<int> erased=new List<int>();
            for(int i=0; i<vertices.Length; i++)
            {
                if(vertices[i].x!=-100 && Vector3.Distance(vertices[i],pointedArea)<1)
                {
                    vertices[i].x = -100;
                    erased.Add(i);
                }
            }
            int newCount = 0;
            for (int i = 0; i < erased.Count; i++)
            {
                for (int j=0; j<triangles.Length;j+=3)
                {
                    if(triangles[j] !=erased[i]&& triangles[j+1] != erased[i]&&triangles[j+2] != erased[i])
                    {
                        trianglesNew[newCount++] = triangles[j];
                        trianglesNew[newCount++] = triangles[j+1];
                        trianglesNew[newCount++] = triangles[j+2];


                    }
                }
            }

            gameObject.GetComponent<MeshFilter>().mesh.triangles = trianglesNew;



        }

    }
    */
}
