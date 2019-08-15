using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AnimationCurveDemo : MonoBehaviour {

	public Button testButton;


	public AnimationCurve curveScale;
	public AnimationCurve curveX;
	public AnimationCurve curveY;
	public AnimationCurve curveZ;

	public void SetCurves(AnimationCurve xC, AnimationCurve yC, AnimationCurve zC)
	{
		curveX = xC;
		curveY = yC;
		curveZ = zC;
	}

	Vector3 odlPos;

	void Start(){
		testButton.onClick.AddListener (() => {
			Debug.Log(Time.time);
//			StartCoroutine (JumpAnimate(this.gameObject, 60, curveY, new Vector3(0, 2, 0)));
			StartCoroutine (ScaleAnimate(this.gameObject, 60, curveScale, new Vector3(1, 1, 1)));
		});
//		odlPos = gameObject.transform.position;
	}


//	void Update()
//	{
//		float eva = curveY.Evaluate (Time.time);
//		Debug.Log ("eva is " + eva);
//		Vector3 rangePos = new Vector3 (0, 2, 0) * eva;
//		gameObject.transform.position = odlPos + rangePos;
//	}

	IEnumerator JumpAnimate(GameObject go, int frameCount, AnimationCurve curve, Vector3 range, System.Action callback = null){
		Vector3 startPos = go.transform.position;
		int count = 1;
		float value_x = 0;
		while (value_x < 1) {
			value_x = (float)count / frameCount;
			float eva = curve.Evaluate (value_x);
			Vector3 rangePos = range * eva;
			Debug.Log ("count is " + count + "        eva is " + eva);
			gameObject.transform.position = startPos + rangePos;
			count++;
			yield return null;
		}
		if (callback != null) {
			callback ();
		}
	}

	IEnumerator ScaleAnimate(GameObject go, int frameCount, AnimationCurve curve, Vector3 range, System.Action callback = null){
		Vector3 startScale = go.transform.localScale;
		int count = 1;
		float value_x = 0;
		while (value_x < 1) {
			value_x = (float)count / frameCount;
			float eva = curve.Evaluate (value_x);
			Vector3 rangeScale = range * eva;
			Debug.Log ("count is " + count + "        eva is " + eva);
			gameObject.transform.localScale = startScale + rangeScale;
			count++;
			yield return null;
		}
		if (callback != null) {
			callback ();
		}
	}

}
