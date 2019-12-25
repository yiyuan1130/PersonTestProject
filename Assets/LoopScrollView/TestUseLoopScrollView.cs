using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EamonnLi.LoopScrollView;

public class ItemData{
	public int index;
	public Sprite iconSprite;
	public Sprite bgSprite;

	public ItemData(int index, Sprite iconSprite, Sprite bgSprite){
		this.index = index;
		this.iconSprite = iconSprite;
		this.bgSprite = bgSprite;
	}
}

public class TestUseLoopScrollView : MonoBehaviour {

	public LoopScrollView loopScrollView;
	void Awake(){
		int count = 100;
		List<ItemData> datas = new List<ItemData>();
		for (int i = 0; i < count; i++)
		{
			datas.Add(new ItemData(i, null, null));
		}
		loopScrollView.InitLoopScrollView(count, datas);
	}
}
