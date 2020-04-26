using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestMonoBehaviour : MonoBehaviour {
	void Awake() {
		Debug.Log("Awake");
	}

	void OnEnable() {
		Debug.Log("OnEnable");
	}

	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnDisable() {
		Debug.Log("OnDisable");
	}

	void OnDestroy() {
		Debug.Log("OnDestroy");
	}

}
