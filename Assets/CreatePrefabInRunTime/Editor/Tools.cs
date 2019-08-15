using UnityEngine;
using System.Collections;
using UnityEditor;
using System.IO;
using UnityEngine.UI;

namespace CreatePrefabInRunTime{
	public class Tools {
		[MenuItem("Tools/CreatePrefab/InEditor")]
		public static void RunTimeCreatePrefab() 
		{
			Debug.Log ("auto create a prefab");
			string prefabName = "testPrefab";
			GameObject obj = new GameObject(prefabName);
			obj.transform.position = Vector3.zero;
			Image obj_image = obj.AddComponent <Image>();
			obj_image.color = Color.red;


			GameObject obj_child1 = new GameObject ("child1");
			obj_child1.transform.SetParent (obj.transform);
			obj_child1.transform.localScale = Vector3.one * 0.5f;
			Image obj_child1_image = obj_child1.AddComponent<Image> ();
			obj_child1_image.color = Color.green;

			string path = "Assets/CreatePrefabInRunTime/Prefabs/" + prefabName + ".prefab";
			if (File.Exists (path)) {
				Debug.Log ("is have");
			}

			// 如果不做路径判断，则会覆盖原路径中的文件
			PrefabUtility.CreatePrefab (path, obj);
		}
	}
}
