using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuaCaiPiao : MonoBehaviour {

	public Camera rtCamera;
	public Transform brush;
	RenderTexture renderTexture;
	public RenderTexture currentRT;
	public RenderTexture previrousRT;
	public Material blitMaterial;
	public Material renderMaterial;

	public LineRenderer lineBrush;

	[Range(1.0f, 5.0f)]
	public float brushWidth = 1.0f;

	void Start () {
		renderTexture = rtCamera.targetTexture;
		renderMaterial.SetTexture("BlitTex", renderTexture);
		renderMaterial.SetMatrix("paintCameraVP", rtCamera.nonJitteredProjectionMatrix * rtCamera.worldToCameraMatrix);
		blitMaterial.SetTexture("_CurrentRT", currentRT);
		blitMaterial.SetTexture("_PrevirousRT", previrousRT);
	}

	void OnMouseDown(){
		rtCamera.clearFlags = CameraClearFlags.Color;
		rtCamera.backgroundColor = Color.black;
		rtCamera.targetTexture = previrousRT;
		rtCamera.Render();
		rtCamera.targetTexture = currentRT;
		rtCamera.Render();
		rtCamera.clearFlags = CameraClearFlags.Nothing;
	}

	Vector3 prePos = Vector3.one * 10000;
	Vector3[] linePosArr = new Vector3[2];
	void OnMouseDrag(){
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit hitInfo;
		if (Physics.Raycast(ray, out hitInfo)){
			// brush.position = hitInfo.point;
			if (prePos == Vector3.one * 10000) {
				prePos = hitInfo.point;
			}
			lineBrush.positionCount = 2;
			linePosArr[0] = prePos;
			linePosArr[1] = hitInfo.point;
			lineBrush.SetPositions(linePosArr);
			lineBrush.startWidth = brushWidth;
			lineBrush.endWidth = brushWidth;
			rtCamera.Render();
			
			Graphics.Blit(currentRT, renderTexture, blitMaterial);
			Graphics.Blit(currentRT, previrousRT);
			prePos = hitInfo.point;
		}
	}

	void OnMouseUp(){
		prePos = Vector3.one * 10000;
	}

	void OnGUI(){
		if (GUI.Button(new Rect(0, 0, 80, 30), "RESET")){
			lineBrush.positionCount = 2;
			linePosArr[0] = Vector3.one * 10000;
			linePosArr[1] = Vector3.one * 10000;
			lineBrush.SetPositions(linePosArr);
			rtCamera.clearFlags = CameraClearFlags.Color;
			rtCamera.backgroundColor = Color.black;
			rtCamera.targetTexture = renderTexture;
			rtCamera.Render();
			rtCamera.targetTexture = previrousRT;
			rtCamera.Render();
			rtCamera.targetTexture = currentRT;
			rtCamera.Render();
			rtCamera.clearFlags = CameraClearFlags.Nothing;
		}
	}
}
