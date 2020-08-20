using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class DynamicCreateSprite : MonoBehaviour {

	// Use this for initialization
	void Start () {
		string path = "Assets/DynamicCreateSprite/a_upper_sdf.png";
        Texture texture = AssetDatabase.LoadAssetAtPath(path, typeof(Texture)) as Texture;
        Sprite sprite = Sprite.Create(texture as Texture2D, new Rect(0, 0, 1100, 1100), Vector2.zero);
		
        GameObject letterGO = new GameObject("A");
        letterGO.transform.position = Vector3.zero;
        letterGO.transform.localScale = Vector3.one;
        letterGO.transform.localRotation = Quaternion.identity;
        GameObject render = new GameObject("render_A");
        SpriteRenderer spriteRenderer = render.AddComponent<SpriteRenderer>();
        spriteRenderer.sprite = sprite;
        render.transform.SetParent(letterGO.transform);
        render.transform.position = Vector3.zero;
        render.transform.localScale = Vector3.one;
        render.transform.localRotation = Quaternion.identity;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
