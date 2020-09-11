using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine;
using Spine.Unity;

public class SpineTestPause : MonoBehaviour {

	public GameObject spineObject;

	void Start () {

	}
	
	void Update () {
		if (Input.GetKeyDown(KeyCode.Space)){
			StopOnFirstFrame();
		}
	}

	void StopOnFirstFrame(){
		SkeletonGraphic skeletonGraphic = spineObject.GetComponent<SkeletonGraphic>();
		TrackEntry track = skeletonGraphic.AnimationState.SetAnimation(0, "xie_await01", true);
		track.TrackTime = 1f / 30;
		skeletonGraphic.AnimationState.TimeScale = 0;
	}
}
