using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AStartController : MonoBehaviour {
	public GameObject pointObj;

	Vector3 startPos = new Vector3(-20, -11, 0);
	int column = 41;
	int row = 23;
	// Use this for initialization
	void Start () {
		CreatePoints();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void CreatePoints(){
		for (int i = 0; i < column; i++)
		{
			for (int j = 0; j < row; j++)
			{
				GameObject point = GameObject.Instantiate(pointObj);
				point.transform.localScale = Vector3.one * 0.3f;
				point.transform.position = startPos + new Vector3(i, j, 0);
				point.gameObject.name = point.transform.position.ToString();
			}
		}
	}
}
