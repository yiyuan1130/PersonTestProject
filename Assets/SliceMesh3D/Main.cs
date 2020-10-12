using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main : MonoBehaviour {

	public GameObject meshObject;
	public Transform panel;
	public LineRenderer lineRenderer;
	private Mesh mesh;

	void Start () {
		mesh = meshObject.GetComponent<MeshFilter>().mesh;
		Debug.Log(Vector3.Cross(new Vector3(1, 0, 0), new Vector3(0, 1, 0)));
	}
	
	Vector3 downPos = Vector3.one * 1000;
	Vector3 upPos = Vector3.one * 1000;
	bool drawing = false;
	void Update () {
		if (Input.GetMouseButtonDown(0)){
			downPos = Utility.MousePostionToWorld(Input.mousePosition, meshObject.transform);
			drawing = true;
			ActiveOthers(true);
		}
		if (Input.GetMouseButtonUp(0)){
			drawing = false;
			ActiveOthers(false);
			Mesh[] meshes = SliceMesh.DoSlice(new Panel(panel.up, panel.position), meshObject);
			OnSliced(meshes);
		}
		if (drawing) {
			upPos = Utility.MousePostionToWorld(Input.mousePosition, meshObject.transform);
			lineRenderer.startWidth = 0.05f;
			lineRenderer.endWidth = 0.05f;
			lineRenderer.SetPositions(new Vector3[]{
				downPos, upPos
			});
			DrawPanel();
		}
	}

	void DrawPanel(){
		panel.transform.position = new Vector3((downPos.x + upPos.x)/2, (downPos.y + upPos.y) / 2, meshObject.transform.position.z);
		panel.transform.right = Vector3.Normalize(upPos - downPos);
	}

	void OnSliced(Mesh[] meshes){
		CreateMeshObject(meshes[0], "part1");
		CreateMeshObject(meshes[1], "part2");
	}

	void CreateMeshObject(Mesh mesh, string name){
		GameObject obj = new GameObject(name);
		obj.AddComponent<MeshFilter>().mesh = mesh;
		obj.AddComponent<MeshRenderer>().material = meshObject.GetComponent<MeshRenderer>().material;
		obj.transform.position = meshObject.transform.position;
		obj.transform.rotation = meshObject.transform.rotation;
		obj.transform.localScale = meshObject.transform.localScale;
		obj.AddComponent<MeshCollider>().convex = true;
		obj.AddComponent<Rigidbody>();
	}

	void ActiveOthers(bool active){
		lineRenderer.gameObject.SetActive(active);
		panel.gameObject.SetActive(active);
		meshObject.gameObject.SetActive(active);
		if (active){
			GameObject part1 = GameObject.Find("part1");
			if (part1)
				GameObject.Destroy(part1);
			GameObject part2 = GameObject.Find("part2");
			if (part2)
				GameObject.Destroy(part2);
		}
	}
}
