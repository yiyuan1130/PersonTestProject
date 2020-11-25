using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTrans : MonoBehaviour {
	public Material material;
	public Camera camera1;
	public Camera camera2;
	public GameObject scene1;
	public GameObject scene2;

	public RenderTexture rt;

	public AnimationCurve animationCurve;
	void Start () {
		
	}
	
	void Update () {
		if (Input.GetMouseButtonDown(0)){
			camera1.targetTexture = rt;
			scene1.SetActive(false);
			scene2.SetActive(true);
			camera2.Render();
			DoRender doRender = camera1.gameObject.GetComponent<DoRender>();
			material.SetTexture("_SubMainTex", rt);
			doRender.startBilt = true;
			doRender.material = material;
			doRender.rt = rt;
		}
	}

	
}
