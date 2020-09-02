using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestSDFText : MonoBehaviour {

	void Start(){
		SDFText sdfText1 = new SDFText("Helloworld");
		sdfText1.gameObject.transform.localPosition = Vector3.zero;
		SDFText sdfText2 = new SDFText("dWbdD");
		sdfText2.gameObject.transform.localPosition = Vector3.zero - new Vector3(0, 60, 0);
	}
}
