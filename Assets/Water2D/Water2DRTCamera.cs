using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Water2DRTCamera : MonoBehaviour {

	public RenderTexture renderTexture;
	public Material waterMiMaterial;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnRenderImage(RenderTexture src, RenderTexture dest){
		// waterMiMaterial.SetTexture("_MainTex", renderTexture);
		Graphics.Blit(src, dest, waterMiMaterial);
		// Graphics.Blit(src, dest);
	}
}
