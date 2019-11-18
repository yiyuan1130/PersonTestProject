using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EamonnLi.LoopScrollView;

public class TestUseLoopScrollView : MonoBehaviour {

	public LoopScrollView loopScrollView;
	void Awake(){
		loopScrollView.InitLoopScrollView(100);
	}
}
