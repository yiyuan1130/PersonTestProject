using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reset : MonoBehaviour {

	public GameObject spriteObj;

	GameObject spriteSliceObj;
	// Use this for initialization

	public void InstantiateSpriteObj(){
		spriteSliceObj = GameObject.Instantiate(spriteObj);
		spriteSliceObj.transform.localScale = Vector3.one;
		spriteSliceObj.transform.localPosition = Vector3.zero;
		MiaoKids.SliceManager spriteSliceManager = spriteSliceObj.GetComponent<MiaoKids.SliceManager>();
		spriteSliceManager.onStartSlice.AddListener(() => {
			Debug.Log("start slice");
		});
		spriteSliceManager.onEndSlice.AddListener((GameObject go1, GameObject go2, GameObject line) => {
			Debug.Log("end slice");
			Debug.LogFormat("go1 是否是三角形 {0}", MiaoKids.JudgeFigure.IsTargetFigure(go1, MiaoKids.FigureType.triangle));
			Debug.LogFormat("go2 是否是三角形 {0}", MiaoKids.JudgeFigure.IsTargetFigure(go2, MiaoKids.FigureType.triangle));
			StartCoroutine(OnSliceEnd(go1, go2, line));
		});
	}

	void Start(){
		InstantiateSpriteObj();
	}

	IEnumerator OnSliceEnd(GameObject go1, GameObject go2, GameObject line){
		DestroyImmediate(line);
		yield return new WaitForSeconds(1f);
		DestroyImmediate(spriteSliceObj);
		DestroyImmediate(go1);
		DestroyImmediate(go2);
		yield return new WaitForSeconds(0.5f);
		InstantiateSpriteObj();
	}

}
