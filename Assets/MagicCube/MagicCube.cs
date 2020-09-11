using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEditor;

public class MagicCube : MonoBehaviour {
	public Material material;


	// Use this for initialization
	void Start () {
		Mesh mesh = GenMesh();
		GameObject obj = new GameObject("cube");
		obj.AddComponent<MeshFilter>().mesh = mesh;
		obj.AddComponent<MeshRenderer>().material = material;
		SaveMesh(mesh);
		SavePrefab(obj);
	}
	
	void CreateCube(){

	}

	// Update is called once per frame
	void Update () {
		
	}

	Mesh GenMesh(){
		Mesh mesh = new Mesh();

		Vector3 front_left_down  = new Vector3(-3, -3, -3) * 0.5f;
		Vector3 front_left_up    = new Vector3(-3, 3, -3) * 0.5f;
		Vector3 front_right_down = new Vector3(3, -3, -3) * 0.5f;
		Vector3 front_right_up   = new Vector3(3, 3, -3) * 0.5f;
		Vector3 back_left_down   = new Vector3(-3, -3, 3) * 0.5f;
		Vector3 back_left_up     = new Vector3(-3, 3, 3) * 0.5f;
		Vector3 back_right_down  = new Vector3(3, -3, 3) * 0.5f;
		Vector3 back_right_up    = new Vector3(3, 3, 3) * 0.5f;


		mesh.vertices = new Vector3[]{
			// front
			front_left_down,
			front_left_up,
			front_right_up,
			front_right_down,

			// up
			front_left_up,
			back_left_up,
			back_right_up,
			front_right_up,

			// right
			front_right_down,
			front_right_up,
			back_right_up,
			back_right_down,

			//back
			back_right_down,
			back_right_up,
			back_left_up,
			back_left_down,

			//down
			back_left_down,
			front_left_down,
			front_right_down,
			back_right_down,

			//left
			back_left_down,
			back_left_up,
			front_left_up,
			front_left_down
		};

		mesh.triangles = new int[]{
			0,1,2, 0,2,3,
			4,5,6, 4,6,7,
			8,9,10, 8,10,11,
			12,13,14, 12,14,15,
			16,17,18, 16,18,19,
			20,21,22, 20,22,23
		};

		mesh.normals = new Vector3[]{
			Vector3.back, Vector3.back, Vector3.back, Vector3.back, 
			Vector3.up, Vector3.up, Vector3.up, Vector3.up,
			Vector3.right, Vector3.right, Vector3.right, Vector3.right,
			Vector3.forward, Vector3.forward, Vector3.forward, Vector3.forward, 
			Vector3.down, Vector3.down, Vector3.down, Vector3.down,
			Vector2.left, Vector2.left, Vector2.left, Vector2.left
		};


		Color white = Color.white;
		Color green = Color.green;
		Color blue = Color.blue;
		Color red = Color.red;
		Color yellow = Color.yellow;
		Color orange = new Color(255, 100, 0, 255) / 255.0f;
		mesh.colors = new Color[]{
			red, red, red, red,
			yellow, yellow, yellow, yellow,
			green, green, green, green,
			orange, orange, orange, orange, 
			white, white, white, white,
			blue, blue, blue, blue
		};

		return mesh;
	}

	void CutMesh(){
		
	}


	void SaveMesh(Mesh mesh){
		string path = "Assets/MagicCube/magic_cube_mesh.asset";
		if (!File.Exists(path)) {
			AssetDatabase.CreateAsset(mesh, path);
		}
	}
	void SavePrefab(GameObject obj){
		string path = "Assets/MagicCube/magic_cube.prefab";
		if (!File.Exists(path)) {
			PrefabUtility.CreatePrefab (path, obj);
		}
	}
}
