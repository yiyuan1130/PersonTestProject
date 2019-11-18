using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace EamonnLi.LoopScrollView{
	public class LoopItemController : MonoBehaviour {
		public Image background;

		public Image icon;

		public Text title;

		public void UpdateUI(Sprite bgSprite, Sprite iconSprite, string tex){
			background.sprite = bgSprite;
			icon.sprite = iconSprite;
			title.text = tex;
		}
	}
}
