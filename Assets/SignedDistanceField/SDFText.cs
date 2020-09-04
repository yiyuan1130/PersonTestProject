using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;
using UnityEditor;


public class SDFText {
	public GameObject gameObject;
	public List<SDFImage> characters;

	public Color color;
	public SDFStyle style;
	public float spacing;

	public SDFText (string s, float spacing = 0f){
		this.spacing = spacing;
		characters = new List<SDFImage>();
		char[] arr = s.ToCharArray();
		GameObject sdfText = new GameObject("SdfText");

		StreamReader file = File.OpenText(Application.dataPath + "/SignedDistanceField/text_config.json");
		JsonTextReader reader = new JsonTextReader(file);
		JObject jsonObject = (JObject)JToken.ReadFrom(reader);
		file.Close();

		Vector3 pos = Vector3.zero;
		for (int i = 0; i < arr.Length; i++)
		{
			string letter = arr[i].ToString();
			int asc = (int)arr[i];
			Debug.Log("=================== " + letter + asc);
			JObject offsetObj = (JObject)jsonObject.GetValue(letter);
			float offsetX = (float)offsetObj.GetValue("offset_x");
			float offsetY = (float)offsetObj.GetValue("offset_y");
			GameObject letterObj = new GameObject(letter);
			SDFImage sDFImage = letterObj.AddComponent<SDFImage>();
			RectTransform tran = letterObj.GetComponent<RectTransform>();
			letterObj.transform.SetParent(sdfText.transform);
			tran.sizeDelta = new Vector2(64, 64);
			Vector3 position = pos + new Vector3(-offsetX, offsetY, 0);
			tran.localPosition = position;
			pos = pos + new Vector3(-offsetX * 2 + 64 + spacing, 0, 0);
			tran.localScale = Vector3.one;

			sDFImage.material = AssetDatabase.LoadAssetAtPath("Assets/SignedDistanceField/sdf_mat_ui.mat", typeof(Material)) as Material;


			string endWith = "_lower_sdf.png";
			if (asc >= 97 && asc <= 122){
				endWith = "_lower_sdf.png";
			}
			else if (asc >= 65 && asc <= 90) {
				endWith = "_upper_sdf.png";
			}

			string lowerSpritePath = "Assets/SignedDistanceField/sdf/" + letter + endWith;
			Sprite lowerSprite = AssetDatabase.LoadAssetAtPath(lowerSpritePath, typeof(Sprite)) as Sprite;
			sDFImage.sprite = lowerSprite;
			sDFImage.ramp = AssetDatabase.LoadAssetAtPath("Assets/SignedDistanceField/ramp.png", typeof(Texture)) as Texture;
			sDFImage.style = SDFStyle.PURE;
			sDFImage.color = Color.yellow;

			characters.Add(sDFImage);
		}

		sdfText.transform.SetParent(GameObject.Find("Canvas").transform);
		sdfText.transform.localPosition = Vector3.zero;
		gameObject = sdfText;
	}

	public void Applay(){
		for (int i = 0; i < characters.Count; i++)
		{
			characters[i].style = style;
			characters[i].color = color;
		}
	}
}
