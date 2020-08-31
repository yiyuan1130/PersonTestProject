using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SDFText : MonoBehaviour {
	public Sprite sprite;
	public Texture ramp;
	public Color color;
	public SDFStyle style;
	public SDFStep step;
	public Material uiMaterial;
	public Material rendererMaterial;
	void OnGUI(){
		if (GUILayout.Button("create UI")){
			GameObject obj = new GameObject();
			SDFImage sdfImage = obj.AddComponent<SDFImage>();
			sdfImage.material = new Material(Shader.Find("iHuman/SDF/ui"));
			//sdfImage.sprite = sprite;
			sdfImage.ramp = ramp;
			//sdfImage.color = color;
			sdfImage.style = style;
			sdfImage.step = step;
			obj.transform.SetParent(GameObject.Find("Canvas").transform.GetChild(0));
			obj.transform.localScale = Vector3.one;
			obj.transform.localPosition = Vector3.one;
		}
		if (GUILayout.Button("create Renderer")){
			GameObject obj = new GameObject();
			SDFRenderer sdfRenderer = obj.AddComponent<SDFRenderer>();
			sdfRenderer.GetComponent<Renderer>().material = new Material(Shader.Find("iHuman/SDF/normal"));
			sdfRenderer.sprite = sprite;
			sdfRenderer.ramp = ramp;
			sdfRenderer.color = color;
			sdfRenderer.style = style;
			sdfRenderer.step = step;
		}
	}
}
