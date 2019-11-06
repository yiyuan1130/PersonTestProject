using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reset : MonoBehaviour {

	public GameObject spriteObj;

	GameObject spriteSliceObj;
	// Use this for initialization
	void Start () {
		ResetSprite();
	}
	
	public void ResetSprite(){
		DestroyPart();
		DestroyLine();
		DestroySprite();
		InstantiateObj();
	}

	void InstantiateObj(){
		spriteSliceObj = GameObject.Instantiate(spriteObj);
		spriteSliceObj.name = "SpriteSliceObj";
		spriteSliceObj.gameObject.SetActive(true);
		spriteSliceObj.transform.localScale = Vector3.one;
		spriteSliceObj.transform.position = Vector3.zero;
	}
	void DestroySprite(){
		if (spriteSliceObj) {
			GameObject.DestroyImmediate(spriteSliceObj);
		}
	}
	void DestroyLine(){
		GameObject line = GameObject.Find("line");
		if (line){
			GameObject.DestroyImmediate(line);
		}
	}
	void DestroyPart(){
		GameObject part = GameObject.Find("part");
		if (part){
			GameObject.DestroyImmediate(part);
			DestroyPart();
		}else{
			return;
		}
	}
}
