using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

[ExecuteInEditMode]
public class ShowAll : MonoBehaviour {

	public Material material;
	List<GameObject> objects;
	public Transform parent;
	public Vector3 offset = new Vector3(1.5f, 1.5f);
	void Start () {
		for (int i = parent.childCount - 1; i >= 0; i--)
		{
			GameObject.DestroyImmediate(parent.GetChild(i).gameObject);
		}
		string base_path = Application.dataPath + "/UnsignedDistanceField/udf";
		string[] paths = Directory.GetFiles(base_path, "*.png", SearchOption.TopDirectoryOnly);
		for (int i = 0; i < paths.Length; i++)
		{
			string path = paths[i].Replace(Application.dataPath, "Assets");
			// Debug.Log(path);
			Sprite sprite = AssetDatabase.LoadAssetAtPath(path, typeof(Sprite)) as Sprite;
			string name = path.Split(new char[]{'/', '\\', '.'})[3];
			GameObject obj = new GameObject(name);
			obj.transform.SetParent(parent.transform);
			SpriteRenderer sr = obj.AddComponent<SpriteRenderer>();
			sr.sharedMaterial = material;
			sr.sprite = sprite;
			obj.transform.localScale = Vector3.one;
			obj.transform.position = Vector3.zero + new Vector3(i / 2 * 1.5f, i % 2 * 1.5f, 0);
		}
	}
	
	// Update is called once per frame
	void Update () {
	}

	void OnDestroy(){
		GameObject.DestroyImmediate(parent);
	}
}
