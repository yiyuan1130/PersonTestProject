using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class TestGameObject : MonoBehaviour {

	public StartMoveEvent startMoveEvent = new StartMoveEvent();
	public OnMoveEvent onMoveEvent = new OnMoveEvent();
	public EndMoveEvent endMoveEvent = new EndMoveEvent();

	static float startTime = 0f;

	void Awake(){
		startMoveEvent.AddListener((GameObject go, string str) => {
			Debug.Log("<color=yellow>Start Move, GameObject is -> " + "  string content is -> " + str + "</color>");
		});
		onMoveEvent.AddListener((GameObject go, string str) => {
			Debug.Log("<color=orange>On Move, GameObject is -> " + "  string content is -> " + str + "</color>");
		});
		endMoveEvent.AddListener((GameObject go, string str) => {
			Debug.Log("<color=green>End Move, GameObject is -> " + gameObject.name + "  string content is -> " + str + "</color>");
		});
	}

	void Start(){
		StartCoroutine(_CubeMove());
	}

	IEnumerator _CubeMove(){
		startMoveEvent.Invoke(gameObject, "开始移动");
		startTime = Time.time;
		while (Time.time - startTime < 2f)
		{
			transform.position += new Vector3(0.03f, 0, 0);
			yield return null;
			onMoveEvent.Invoke(gameObject, "移动中");
		}
		endMoveEvent.Invoke(gameObject, "移动结束");
	}
}
