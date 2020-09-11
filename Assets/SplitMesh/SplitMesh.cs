using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplitMesh : MonoBehaviour {

	public LineRenderer lineRenderer;
	public GameObject cube;

	void Start () {
		
	}
	

	Vector3 panelStartPos;
	Vector3 panelEndPos;
	bool drawingPanel = false;
	void Update () {
		if (Input.GetMouseButtonDown(0)){
			panelStartPos = MousPostionToWorld(Input.mousePosition, cube.transform);
			panelEndPos = panelStartPos;
			drawingPanel = true;
		}
		if (Input.GetMouseButtonUp(0)){
			drawingPanel = false;
		}
		if (drawingPanel) {
			panelEndPos = MousPostionToWorld(Input.mousePosition, cube.transform);
			DrawLine();
		}
	}

	void DrawLine(){
		lineRenderer.positionCount = 2;
		lineRenderer.startWidth = 0.1f;
		lineRenderer.endWidth = 0.1f;
		lineRenderer.SetPositions(new Vector3[]{
			panelStartPos, panelEndPos,
		});
	}

	public Vector3 MousPostionToWorld(Vector3 mousePos, Transform targetTransform)
    {
        Vector3 dir = targetTransform.position - Camera.main.transform.position;
        Vector3 normardir = Vector3.Project(dir, Camera.main.transform.forward);
        return Camera.main.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, normardir.magnitude));
    }

	public void CreatePanel(){
		Vector3 p1 = panelStartPos;
		Vector3 p2 = panelEndPos;
		Vector3 dir = p2 - Camera.main.transform.position;
        Vector3 normardir = Vector3.Project(dir, Camera.main.transform.forward);
		Vector3 p3 = p2 + normardir;

		
	}
}
