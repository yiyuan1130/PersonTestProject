using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class CreateMesh {

	[MenuItem("GameObject/3D Object/SDFRenderer", false, 0)]
	public static void Create3DSDFRenderer(){
		Create<SDFRenderer>();
	}
	[MenuItem("GameObject/2D Object/SDFRenderer", false, 0)]
	public static void Create2DSDFRenderer(){
		Create<SDFRenderer>();
	}

	[MenuItem("GameObject/UI/SDFImage", false, 0)]
	public static void CreateSDFImage(){
		Create<SDFImage>();
	}


	static bool cleared = false;
	public static void Create<T>() where T : MonoBehaviour{
		if (Selection.objects.Length <= 0) {
			if (cleared){
				cleared = false;
				return;
			}
			GameObject obj = new GameObject();
			obj.AddComponent<T>();
			obj.transform.localPosition = Vector3.zero;
			obj.transform.localScale = Vector3.one;
			obj.transform.localRotation = Quaternion.identity;
		}
		else
		{
			foreach(GameObject item in Selection.objects){
				GameObject obj = new GameObject();
				obj.AddComponent<T>();
				obj.transform.SetParent(item.transform);
				obj.transform.localPosition = Vector3.zero;
				obj.transform.localScale = Vector3.one;
				obj.transform.localRotation = Quaternion.identity;
			}
			Selection.objects = new UnityEngine.Object[]{};
			cleared = true;
		}
	}

}
