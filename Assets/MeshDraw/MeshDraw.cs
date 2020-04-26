 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MeshDraw : MonoBehaviour {

	public Button DrawTriangleButton;
	public Button DrawSquareButton;
	public Button DrawPolygonButton;
	public Button DrawClicleButton;
	public Button CleanButton;
	public InputField countInput;

	public Material mat;

	[HideInInspector]
	public GameObject drawGo;

	[HideInInspector]
	public MeshFilter meshFilter;

	[HideInInspector]
	public MeshRenderer meshRenderer;

	int count;

	void Awake(){
		DrawTriangleButton.transform.GetChild (0).GetComponent<Text> ().text = "DrawTriangle";
		DrawTriangleButton.onClick.AddListener (DrawtTriangle);

		DrawSquareButton.transform.GetChild (0).GetComponent<Text> ().text = "DrawSquare";
		DrawSquareButton.onClick.AddListener (DrawSquare);

		DrawPolygonButton.transform.GetChild (0).GetComponent<Text> ().text = "DrawPolygon";
		DrawPolygonButton.onClick.AddListener (DrawPolygon);

		DrawClicleButton.transform.GetChild (0).GetComponent<Text> ().text = "DrawClicle";
		DrawClicleButton.onClick.AddListener (DrawClicle);

		CleanButton.transform.GetChild (0).GetComponent<Text> ().text = "Clean";
		CleanButton.onClick.AddListener (Clean);

		countInput.onValueChanged.AddListener ((string a) => {
			if (!string.IsNullOrEmpty(a)){
				count = System.Convert.ToInt32(a);
			}
		});
	}

	Mesh PrepareMesh(){
		drawGo = new GameObject ("DrawWithMesh");
		drawGo.transform.SetParent (transform);
		drawGo.transform.localPosition = Vector3.zero;
		drawGo.transform.localScale = Vector3.one;

		meshRenderer = drawGo.AddComponent<MeshRenderer> ();
		meshRenderer.material = mat;

		meshFilter = drawGo.AddComponent<MeshFilter> ();
		Mesh mesh = meshFilter.mesh;
		mesh.Clear();

		return mesh;
	}

	void DrawtTriangle(){
		Mesh mesh = PrepareMesh ();
		mesh.vertices = new Vector3[]{ new Vector3 (0, 0, 0), new Vector3 (0, 1, 0), new Vector3 (1, 1, 0)};
		mesh.triangles = new int[]{ 0, 1, 2};
	}

	void DrawSquare(){
		Mesh mesh = PrepareMesh ();
		mesh.vertices = new Vector3[]{ new Vector3 (0, 0, 0), new Vector3 (0, 1, 0), new Vector3 (1, 1, 0), new Vector3(1, 0, 0)};
		mesh.triangles = new int[]{ 0, 1, 2, 0, 2, 3};
	}

	void DrawClicle(){
		int circle_count = 360;
		Mesh mesh = PrepareMesh ();
		Vector3[] vertices = new Vector3[circle_count + 1];
		vertices[0] = Vector3.zero;
		float pre_rad = Mathf.Deg2Rad * 360 / circle_count;
		for (int i = 0; i < circle_count; i++) {
			float deg = -i * pre_rad;
			float x = Mathf.Cos (deg);
			float y = Mathf.Sin (deg);
			vertices [i + 1] = new Vector3 (x, y, 0) * 3;
		}
		mesh.vertices = vertices;	

		int[] triangles = new int[circle_count * 3];
		for (int i = 0; i < triangles.Length; i+=3) {
			int first = 0;
			int second = i / 3 + 1;
			int third = second + 1;
			if (third > circle_count) {
				third = 1;
			}
			triangles [i] = first;
			triangles [i + 1] = second;
			triangles [i + 2] = third;
		}
		mesh.triangles = triangles;
	}

	void DrawPolygon(){
		int circle_count = count;
		Mesh mesh = PrepareMesh ();
		Vector3[] vertices = new Vector3[circle_count + 1];
		vertices[0] = Vector3.zero;
		float pre_rad = Mathf.Deg2Rad * 360 / circle_count;
		for (int i = 0; i < circle_count; i++) {
			float deg = -i * pre_rad;
			float x = Mathf.Cos (deg);
			float y = Mathf.Sin (deg);
			vertices [i + 1] = new Vector3 (x, y, 0) * 3;
		}
		mesh.vertices = vertices;	

		int[] triangles = new int[circle_count * 3];
		for (int i = 0; i < triangles.Length; i+=3) {
			int first = 0;
			int second = i / 3 + 1;
			int third = second + 1;
			if (third > circle_count) {
				third = 1;
			}
			triangles [i] = first;
			triangles [i + 1] = second;
			triangles [i + 2] = third;
		}
		mesh.triangles = triangles;
	}

	void Clean(){
		while (transform.childCount > 0) {
			DestroyImmediate (transform.GetChild(0).gameObject);
		}
	}

}
