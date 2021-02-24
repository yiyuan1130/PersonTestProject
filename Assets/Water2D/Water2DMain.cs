using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Water2DMain : MonoBehaviour {

	public GameObject water;
	public Transform waterEnrty;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetMouseButton(0)){
			GameObject waterObj = GameObject.Instantiate(water);
			waterObj.transform.position = waterEnrty.position;
			waterObj.GetComponent<Rigidbody2D>().AddForce(new Vector2(10, 0), ForceMode2D.Impulse);
		}
	}
}
