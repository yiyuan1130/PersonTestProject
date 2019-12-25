using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace EamonnLi.LoopScrollView{
	public class LoopItemController : MonoBehaviour {
		public Image background;

		public Image icon;

		public Text title;

		public void UpdateUI(ItemData data){
			icon.gameObject.SetActive(false);
			title.gameObject.SetActive(false);
			background.sprite = data.bgSprite;
			icon.color = new Color(data.index / 100.0f, 1 - data.index / 100.0f, 1, 1);
			title.text = data.index.ToString();
		}
	}
}
