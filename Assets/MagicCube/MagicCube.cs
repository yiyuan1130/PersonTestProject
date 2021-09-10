using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEditor;

public class MagicCube : MonoBehaviour {
	public Material material;
	public GameObject cube;

	//Dictionary<Transform, Vector3> cubeMap = new Dictionary<Transform, Vector3>();

	List<Transform> cubeList = new List<Transform>();

	// Use this for initialization
	void Start() {
		CreateMagicCube();
	}

	void CreateMagicCube() {
		for (int x = 0; x < 3; x++)
		{
			for (int y = 0; y < 3; y++)
			{
				for (int z = 0; z < 3; z++)
				{
					float pos_x = (x - 1) * 3;
					float pos_y = (y - 1) * 3;
					float pos_z = (z - 1) * 3;
					Transform cubeTransform = GameObject.Instantiate(cube).transform;
					cubeTransform.position = new Vector3(pos_x, pos_y, pos_z);
					cubeList.Add(cubeTransform);
				}
			}
		}
	}

	Vector3 clickDownPos;
	Vector3 offset;
	Transform selectTransform;
	List<Transform> rotateTransforms;
	float curAngle = 0f;
	int dir = 1;
	bool rotating = false;
	Vector3 aix;
	private void Update()
    {
		if (rotating) {
			Debug.Log("============== curAngle ===> " + curAngle);
			if (Mathf.Abs(curAngle) >= 90f) {
				rotating = false;
			}
			float speed = Time.deltaTime * 200f * dir;
			curAngle = curAngle + speed;
			if (Mathf.Abs(Mathf.Abs(curAngle) - 90f) <= 1f) {
				curAngle = 90f * dir;
			}
			for (int i = 0; i < rotateTransforms.Count; i++) {
				rotateTransforms[i].RotateAround(Vector3.zero, aix, speed);
			}
		}
		if (!rotating && Input.GetMouseButtonDown(0)) {
			rotateTransforms.Clear();
			selectTransform = null;
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit hit;
			if (Physics.Raycast(ray, out hit)) {
                for (int i = 0; i < cubeList.Count; i++)
                {
					if (cubeList[i] == hit.transform) {
						selectTransform = hit.transform;
						clickDownPos = Input.mousePosition;
						break;
					}
				}
			}
		}
		if (!rotating && Input.GetMouseButtonUp(0)) {
			rotateTransforms = new List<Transform>();
			offset = (Input.mousePosition - clickDownPos);
			if (Mathf.Abs(offset.x) > Mathf.Abs(offset.y)) {
                foreach (var cube in cubeList)
                {
					Vector3 pos = cube.position;
					if (Mathf.Abs(pos.y - selectTransform.position.y) < 0.5f) {
						rotateTransforms.Add(cube);
					}
				}
				curAngle = 0;
				aix = Vector3.up;
				rotating = true;
				if (offset.x < 0)
				{
					dir = 1;
				}
				else
				{
					dir = -1;
				}
				Debug.Log("横向 ==> " + offset.x);
				// 横向
			} else {
				Debug.Log("纵向");
				foreach (var cube in cubeList)
				{
					Vector3 pos = cube.position;
					if (Mathf.Abs(pos.x - selectTransform.position.x) < 0.5f)
					{
						rotateTransforms.Add(cube);
					}
				}
				curAngle = 0;
				rotating = true;
				aix = Vector3.right;
				if (offset.y < 0)
				{
					dir = -1;
				}
				else {
					dir = 1;
				}
			}
		}
    }


    void CreateCube(){
		Mesh mesh = GenMesh();
		GameObject obj = new GameObject("cube");
		obj.AddComponent<MeshFilter>().mesh = mesh;
		obj.AddComponent<MeshRenderer>().material = material;
		SaveMesh(mesh);
		SavePrefab(obj);
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
