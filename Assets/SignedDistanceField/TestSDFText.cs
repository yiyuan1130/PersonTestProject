using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class TestSDFText : MonoBehaviour {

	public SDFStyle style = SDFStyle.PURE;
	public Color color = Color.red;

	SDFText sDFText;

	void Start(){
		SDFText sdfText1 = new SDFText("PerfectWorldiHuman");
		sdfText1.gameObject.transform.localPosition = Vector3.zero;
		sDFText = sdfText1;
		// SDFText sdfText2 = new SDFText("dWbdD");
		// sdfText2.gameObject.transform.localPosition = Vector3.zero - new Vector3(0, 60, 0);
	}

	void Update(){
		sDFText.color = color;
		sDFText.style = style;
		sDFText.Applay();
	}
}
