using UnityEngine;
using System.Collections;
using System.IO;

public class CreatePrefabInRunTime : MonoBehaviour {
	void RunTimeCreatePrefab(GameObject obj, string prefabName) 
	{
		string path = "Assets/Resources/CreatePrefabInRunTime/Prefabs/" + prefabName + ".prefab";
		if (File.Exists (path)) {
			Debug.Log ("is have");
		}

		// 如果不做路径判断，则会覆盖原路径中的文件
//		UnityEditor.PrefabUtility.CreatePrefab (path, obj);
	}

	void OnGUI(){
		if (GUILayout.Button ("create prefab")) {
			GameObject obj = GameObject.Find ("TestPanel");
			RunTimeCreatePrefab (obj, "test_panel_prefab");
		}
	}
}
