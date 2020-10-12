using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public static class DebugPoint {
	static GameObject pointPrefab;
	static GameObject meshObject;
	static List<GameObject> pointList;
	public static void Init(GameObject go){
		pointPrefab = AssetDatabase.LoadAssetAtPath("Assets/point.prefab", typeof(GameObject)) as GameObject;
		meshObject = go;
		Hide();
		if (pointList == null)
			pointList = new List<GameObject>();
	}
	public static void Hide(){
		if(pointList == null)
			return;
		foreach (var item in pointList)
			GameObject.Destroy(item);
	}
	public static void Show(Vector3[] points, Color? color, float? scale){
		for (int i = 0; i < points.Length; i++)
			_Show(points[i], color, scale);
	}

	public static void Show(List<Vector3> points, Color? color, float? scale){
		for (int i = 0; i < points.Count; i++)
			_Show(points[i], color, scale);
	}

	public static void Show(Vector3 point, Color? color, float? scale){
		_Show(point, color, scale);
	}

	static void _Show(Vector3 point, Color? color, float? scale){
		GameObject go = GameObject.Instantiate(pointPrefab);
		go.SetActive(true);
		pointList.Add(go);
		go.transform.localScale = Vector3.one * (float)scale;
		go.transform.position = Utility.ObjectToWorldPoint(point, meshObject.transform);
		go.GetComponent<MeshRenderer>().material.SetColor("_Color", (Color)color);
	}
}
