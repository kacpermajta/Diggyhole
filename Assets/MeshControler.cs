using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshControler : MonoBehaviour
{
    public Material material;
    public float a = 0.4f;
    float h;
    public int szer, wys;
    public int amount;

    // Start is called before the first frame update
    void Start()
    {
        h = a * Mathf.Sqrt(3) / 2;
        float hStart;
        float aStart = 0;
        
        for (int k = 0; k < amount; k++)
        {
            hStart = 0;
            for (int l = 0; l < amount; l++)
            {
                Vector3[] vertices = new Vector3[szer * wys];
                Vector2[] uv = new Vector2[szer * wys];
                int cellNumber = 6 * (szer - 1) * (wys - 1) * 2;
                int[] triangles = new int[cellNumber];
                Vector3[] verticesC = new Vector3[szer * wys * 2];
                Vector2[] uvC = new Vector2[szer * wys * 2];
                int[] trianglesC = new int[cellNumber * 6];
                int indeksVer = 0;
                int indeksFace = 0;

                GameObject gameObject = new GameObject("Mesh", typeof(MeshFilter), typeof(MeshRenderer), typeof(groundController));
                gameObject.GetComponent<groundController>().polygonColliders = new PolygonCollider2D[cellNumber];

                for (int i = 0; i < wys; i++)
                    for (int j = 0; j < szer; j++)
                    {
                        vertices[indeksVer] = new Vector3(aStart + a * (j + 0.5f * (i % 2)), hStart + h * i);


                        uv[indeksVer] = new Vector2(a * (j + 0.5f * (i % 2)), h * i);

                        int first, second, third;
                        Vector2 firstpath, secondpath, thirdpath;


                        if (j < szer - 1 && i < wys - 1)
                        {
                            first = indeksVer;

                            if (i % 2 == 1)
                            {
                                second = indeksVer + szer + 1;
                            }
                            else
                            {
                                second = indeksVer + szer;
                            }

                            third = indeksVer + 1;

                            triangles[indeksFace] = first;
                            triangles[indeksFace + 1] = second;
                            triangles[indeksFace + 2] = third;




                            indeksFace += 3;
                            //Debug.Log(indeksFace + ", " + triangles.Length);


                        }
                        if (j < szer - 1 && i > 0)
                        {
                            triangles[indeksFace] = indeksVer;



                            triangles[indeksFace + 1] = indeksVer + 1;



                            if (i % 2 == 1)
                            {
                                triangles[indeksFace + 2] = indeksVer - szer + 1;


                            }
                            else
                            {
                                triangles[indeksFace + 2] = indeksVer - szer;


                            }
                            indeksFace += 3;
                            //Debug.Log(indeksFace + ", " + triangles.Length);

                        }

                        indeksVer++;
                    }

                Mesh mesh = new Mesh();
                mesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;
                mesh.vertices = vertices;
                mesh.uv = uv;
                mesh.triangles = triangles;

                for (int i = 0; i < triangles.Length; i += 3)
                {
                    PolygonCollider2D polygonCollider2d = gameObject.AddComponent<PolygonCollider2D>();
                    polygonCollider2d.enabled = false;
                    polygonCollider2d.pathCount = 1;
                    polygonCollider2d.SetPath(0, new Vector2[3] { vertices[triangles[i]], vertices[triangles[i + 1]], vertices[triangles[i + 2]] });
                    polygonCollider2d.enabled = true;
                    gameObject.GetComponent<groundController>().polygonColliders[i / 3] = polygonCollider2d;
                }

                //GameObject gameObject = new GameObject("Mesh", typeof(MeshFilter), typeof(MeshRenderer), typeof(PolygonCollider2D), typeof(groundController));
                gameObject.GetComponent<MeshFilter>().mesh = mesh;
                gameObject.GetComponent<MeshRenderer>().material = material;
                //gameObject.GetComponent<MeshCollider>().sharedMesh = meshC;

                gameObject.GetComponent<groundController>().surface = mesh;
                gameObject.GetComponent<groundController>().triangles = mesh.triangles;
                gameObject.GetComponent<groundController>().vertices = mesh.vertices;
                hStart += h * (wys - 1);
            }
            aStart += a * (szer - 1);
        }
        //gameObject.GetComponent<groundController>().generate();


    }

    // Update is called once per frame
    void Update()
    {

    }
}
