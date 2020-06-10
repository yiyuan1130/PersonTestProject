using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChanController : MonoBehaviour {
	public GameObject head;
	Animator animator;
	void Awake() {
		animator = gameObject.GetComponent<Animator>();
	}

	void Start() {

	}

	float clickTime;
	void OnMouseDown(){
		clickTime = Time.time;
	}

	void OnMouseDrag(){
		
	}

	void OnMouseUp(){
		if (Time.time - clickTime <= 0.15f){
			int v = Random.Range(1, 12);
			animator.SetTrigger("ani");
			animator.SetInteger("value", v);
		}
	}

	void Update(){
		// Vector3 worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		// Debug.Log(worldPos);
		// worldPos = new Vector3(worldPos.x, worldPos.y, head.transform.position.z);
		// GameObject go = new GameObject(Time.time.ToString());
		// go.transform.position = worldPos;
		// head.transform.LookAt(worldPos);

	}
}
