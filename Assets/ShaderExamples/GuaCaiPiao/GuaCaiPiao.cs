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

	void Start () {
		renderTexture = rtCamera.targetTexture;
		renderMaterial.SetTexture("BlitTex", renderTexture);
		renderMaterial.SetMatrix("paintCameraVP", rtCamera.nonJitteredProjectionMatrix * rtCamera.worldToCameraMatrix);
	}
	

	Vector3 prePos;
	void OnMouseDrag(){
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit hitInfo;
		if (Physics.Raycast(ray, out hitInfo)){
			brush.position = hitInfo.point;
			if (prePos == null) {
				prePos = hitInfo.point;
			}
			lineBrush.SetPositions(new Vector3[]{prePos, hitInfo.point});
			lineBrush.startWidth = 1f;
			lineBrush.endWidth = 1f;
			prePos = hitInfo.point;
		}

		rtCamera.clearFlags = CameraClearFlags.Color;
		rtCamera.backgroundColor = Color.black;
		rtCamera.targetTexture = currentRT;
		rtCamera.Render();

		rtCamera.clearFlags = CameraClearFlags.Nothing;
		rtCamera.Render();
		blitMaterial.SetTexture("CurrentRT", currentRT);
		blitMaterial.SetTexture("Previrous", renderTexture);
		Graphics.Blit(currentRT, renderTexture, blitMaterial);
		Graphics.Blit(currentRT, previrousRT);
	}
}
