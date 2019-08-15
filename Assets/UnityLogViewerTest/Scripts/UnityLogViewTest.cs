using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnityLogViewTest : MonoBehaviour {

	// Use this for initialization
	void Start () {
		Debug.Log ("MonoBehaviour Start");
		Debug.LogWarning ("MonoBehaviour Start");
		Debug.LogError ("MonoBehaviour Start");
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void TestClick(){
		Debug.Log ("Test Log -------> "+ Time.deltaTime);
	}
}
