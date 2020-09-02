using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;

public class CreateSDF {

	[MenuItem("GameObject/3D Object/SDFRenderer", false, 0)]
	public static void Create3DSDFRenderer(){
		Create<SDFRenderer>("SDFRenderer");
	}
	[MenuItem("GameObject/2D Object/SDFRenderer", false, 0)]
	public static void Create2DSDFRenderer(){
		Create<SDFRenderer>("SDFRenderer");
	}

	[MenuItem("GameObject/UI/SDFImage", false, 0)]
	public static void CreateSDFImage(){
		Create<SDFImage>("SDFImage");
	}
	static bool cleared = false;
	public static void Create<T>(string name) where T : MonoBehaviour{
		if (Selection.objects.Length <= 0) {
			if (cleared){
				cleared = false;
				return;
			}
			GameObject obj = new GameObject(name);
			obj.AddComponent<T>();
			obj.transform.localPosition = Vector3.zero;
			obj.transform.localScale = Vector3.one;
			obj.transform.localRotation = Quaternion.identity;
		}
		else
		{
			foreach(GameObject item in Selection.objects){
				GameObject obj = new GameObject(name);
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

	[MenuItem("Tools/SDFText/CreateSDFImages")]
	public static void CreateSDFImgs(){
		Scene scene = EditorSceneManager.GetActiveScene();
		GameObject[] objs = scene.GetRootGameObjects();
		for (int i = 0; i < objs.Length; i++)
		{
			GameObject obj = objs[i];
			char[] letterList = "abcdefghijklmnopqrstuvwxyz".ToCharArray();
			if (obj.name == "Canvas"){
				Vector3 pos = Vector3.zero;
				for (int j = 0; j < letterList.Length; j++)
				{
					Material material = AssetDatabase.LoadAssetAtPath("Assets/SignedDistanceField/sdf_mat_ui.mat", typeof(Material)) as Material;

					string letter = letterList[j].ToString();

					string upperName = letter.ToUpper();
					string upperSpritePath = "Assets/SignedDistanceField/sdf/" + letter + "_upper_sdf.png";
					Sprite upperSprite = AssetDatabase.LoadAssetAtPath(upperSpritePath, typeof(Sprite)) as Sprite;

					GameObject upperObj = new GameObject(upperName);
					SDFImage upperSDFImg = upperObj.AddComponent<SDFImage>();
					upperSDFImg.material = material;
					upperSDFImg.color = Color.white;
					upperSDFImg.style = SDFStyle.PURE;
					upperSDFImg.sprite = upperSprite;
					upperObj.transform.SetParent(obj.transform);
					upperObj.GetComponent<RectTransform>().sizeDelta = new Vector2(64, 64);
					upperObj.transform.localScale = Vector3.one;
					upperObj.transform.localRotation = Quaternion.identity;
					upperObj.transform.localPosition = pos;

					pos = pos + new Vector3(64, 0, 0);

					string lowerName = letter.ToLower();
					string lowerSpritePath = "Assets/SignedDistanceField/sdf/" + letter + "_lower_sdf.png";
					Sprite lowerSprite = AssetDatabase.LoadAssetAtPath(lowerSpritePath, typeof(Sprite)) as Sprite;

					GameObject lowerObj = new GameObject(lowerName);
					SDFImage lowerSDFImg = lowerObj.AddComponent<SDFImage>();
					lowerSDFImg.material = material;
					lowerSDFImg.color = Color.white;
					lowerSDFImg.style = SDFStyle.PURE;
					lowerSDFImg.sprite = lowerSprite;
					lowerObj.transform.SetParent(obj.transform);
					lowerObj.GetComponent<RectTransform>().sizeDelta = new Vector2(64, 64);
					lowerObj.transform.localScale = Vector3.one;
					lowerObj.transform.localRotation = Quaternion.identity;
					lowerObj.transform.localPosition = pos;

					pos = pos + new Vector3(64, 0, 0);
				}
			}
		}
	}

	[MenuItem("Tools/SDFText/RecordOffsetY")]
	public static void RecordOffsetY(){
		char[] letterList = "abcdefghijklmnopqrstuvwxyz".ToCharArray();
		Transform parent = GameObject.Find("Canvas").transform;
		JObject jObject = new JObject();
		
		for (int j = 0; j < letterList.Length; j++){
			string upper = letterList[j].ToString().ToUpper();
			Transform upperTrans = parent.Find(upper);
			JObject upperJObject = new JObject();
			upperJObject["offset_y"] = upperTrans.localPosition.y;
			jObject[upper] = upperJObject;


			string lower = letterList[j].ToString().ToLower();
			Transform lowerTrans = parent.Find(lower);
			JObject lowerJObject = new JObject();
			lowerJObject["offset_y"] = lowerTrans.localPosition.y;
			jObject[lower] = lowerJObject;
		}
		Debug.Log(jObject.ToString());

		StreamWriter sw = new StreamWriter(Application.dataPath + "/SignedDistanceField/text_config.json");
		sw.Write(jObject.ToString());
		sw.Close();
		sw.Dispose();
	}
}
