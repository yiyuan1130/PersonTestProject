using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshFilter))]

public class BloodSlider : MonoBehaviour
{
    private MeshRenderer meshRenderer;
    private MeshFilter meshFilter;
    private Mesh mesh;
    public Material material;
    [Range(-1f, 1f)]
    public float top = 0;
    [Range(-1f, 1f)]
    public float bottom = 0;
    void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        meshFilter = GetComponent<MeshFilter>();
        GenerateMesh();
        meshRenderer.material = material;
    }

    // Update is called once per frame
    void Update()
    {
        GenerateMesh();
    }

    void GenerateMesh(){
        if (mesh == null) { 
            mesh = new Mesh();
        }
        /*
            1       2       3    
            
            0       5       4

         */
        Vector3[] vertices = new Vector3[6];
        vertices[0] = new Vector3(-1.0f, -0.5f, 0.0f);
        vertices[1] = new Vector3(-1.0f, 0.5f, 0.0f);
        vertices[2] = new Vector3(0.0f + top, 0.5f, 0.0f);
        vertices[3] = new Vector3(1.0f, 0.5f, 0.0f);
        vertices[4] = new Vector3(1.0f, -0.5f, 0.0f);
        vertices[5] = new Vector3(0.0f + bottom, -0.5f, 0.0f);

        int[] triangles = new int[] { 
            0, 1, 2,
            0, 2, 5,
            5, 3, 2,
            5, 4, 3,
            //5, 2, 3,
            //5, 3, 4,
        };

        Vector2[] uv = new Vector2[6];
        uv[0] = new Vector2(0.0f, 0.0f);
        uv[1] = new Vector2(0.0f, 1.0f);
        //uv[2] = new Vector2(0.5f, 1.0f);
        uv[2] = new Vector2((top - (-1.0f)) / 2.0f, 1.0f);
        uv[3] = new Vector2(1.0f, 1.0f);
        uv[4] = new Vector2(1.0f, 0.0f);
        //uv[5] = new Vector2(0.5f, 0.0f);
        uv[5] = new Vector2((bottom - (-1.0f)) / 2.0f, 0.0f);

        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.uv = uv;

        meshFilter.mesh = mesh;
    }
}
