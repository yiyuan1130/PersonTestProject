using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshDrawLine : MonoBehaviour {

	public MeshFilter meshFliter;

	Vector3 downPos;
	Vector3 upPos;

	// Use this for initialization
	void Start () {
		Vector3[] points = new Vector3[8];
		points [0] = new Vector3 (0, 0, 0);
		points [1] = new Vector3 (0, 0.1f, 0);
		points [2] = new Vector3 (1, 0.1f, 0);
		points [3] = new Vector3 (1, 0, 0);

		points [4] = new Vector3 (2, 0, 0);
		points [5] = new Vector3 (2.1f, 0, 0);
		points [6] = new Vector3 (2.1f, -1, 0);
		points [7] = new Vector3 (2, -1, 0);
		CreateMesh (points);
	}

	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButtonDown (0)) {
			downPos = Input.mousePosition;
		}
		if (Input.GetMouseButtonUp (0)) {
			upPos = Input.mousePosition;
		}
	}

	void CreateMesh(Vector3[] pointArray){
		Mesh mesh = new Mesh ();
		mesh.vertices = pointArray;
		mesh.triangles = new int[]{0, 1, 2, 0, 2, 3, 4, 5, 6, 4, 6, 7};
		meshFliter.mesh = mesh;
	}

	void CreateMesh2(Vector3 startPos, Vector3 endPos){
		
	}
}
