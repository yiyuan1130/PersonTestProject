using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeMove : MonoBehaviour {
	void OnMouseDrag(){
		Vector3 target = Camera.main.ScreenToViewportPoint(Input.mousePosition);
		transform.position = new Vector3(target.x, target.y, transform.position.z);
	}
}
