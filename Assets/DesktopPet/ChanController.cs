using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChanController : MonoBehaviour {

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
		if (Time.time - clickTime <= 0.25f){
			int v = Random.Range(1, 10);
			animator.SetTrigger("ani");
			animator.SetInteger("value", v);
		}
	}
}
