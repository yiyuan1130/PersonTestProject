using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshTest : MonoBehaviour {

	public GameObject meshTest;
	public Material material;

	void MeshRenderTest(){
		CanvasRenderer cr = meshTest.AddComponent<CanvasRenderer>();
		Mesh mesh = new Mesh ();
		mesh.vertices = new Vector3[]{
			new Vector3(0, 0, 0), 
			new Vector3(0, 100, 0), 
			new Vector3(100, 100, 0), 
			new Vector3(100, 0, 0) 
		};


		mesh.triangles = new int[]{ 0, 1, 2, 2, 3, 0};
//		mesh.colors = new Color[]{
//			Color.black,
//			Color.blue,
//			Color.cyan,
//			Color.gray
//		};
		cr.Clear();
		cr.SetMaterial(material, null);
//		cr.SetMesh (mesh);
		Vector3[] vertices = mesh.vertices;
		int[] triangles = mesh.triangles;
		Vector3[] normals = mesh.normals;
		Vector2[] uv = mesh.uv;
		UIVertex vertex;
//		Debug.Log (uv.Length);
		List<UIVertex> vertexList = new List<UIVertex> ();
		for (int i = 0; i < triangles.Length; i++)
		{
			vertex = new UIVertex();
			int triangle = triangles[i];

			vertex.position = ((vertices[triangle] - mesh.bounds.center) * 1);
			vertex.uv0 = Vector2.one;
			vertex.normal = Vector3.forward;

			vertexList.Add(vertex);

			if (i % 3 == 0)
				vertexList.Add(vertex);
		}
		cr.SetVertices (vertexList);
	}

	// Use this for initialization
	void Start () {
		MeshRenderTest ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
