using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

namespace EamonnLi.LoopScrollView{
	public class LoopScrollView : MonoBehaviour {
		public RectTransform content;
		public ScrollRect scrollRect;
		public RectTransform scrollRectRt;
		public GameObject loopObject;
		RectTransform loopRt;

		int count;
		public float spacing = 0;

		int leftIndex = 0;
		int rightIndex = 0;
		float leftPosOfContent = 0f;
		float rightPosOfContent = 0;
		int currentIndex = 0;

		List<RectTransform> loopObjectsList = new List<RectTransform>();
		void Awake(){
			loopRt = loopObject.GetComponent<RectTransform>();
		}

		public void InitLoopScrollView(int count){
			this.count = count;
			CreateContent();
			CaculateMinCount();
			CreateLoopObject();
			scrollRect.onValueChanged.AddListener(OnScrolled);
		}

		float contentWidth = 0f;
		void CreateContent(){
			Vector2 itemSize = loopRt.sizeDelta;
			contentWidth = count * itemSize.x + (count - 1) * spacing;
			content.sizeDelta = new Vector2(contentWidth, content.sizeDelta.y);
		}

		int minCount = 0;
		void CaculateMinCount(){
			float minWidth = scrollRect.GetComponent<RectTransform>().sizeDelta.x;
			Debug.Log("minWidth : " + minWidth);
			minCount = 1;
			minCount = minCount + (int)Mathf.Floor(minWidth / (loopRt.sizeDelta.x + spacing));
			minCount++;
		}

		void CreateLoopObject(){
			int createCount = 0;
			if (count < minCount - 1){
				leftIndex = 0;
				rightIndex = count;
				leftPosOfContent = 0f;
				rightPosOfContent = scrollRectRt.sizeDelta.x;
				createCount = count;
			}else{
				leftIndex = 0;
				rightIndex = minCount;
				leftPosOfContent = 0f;
				rightPosOfContent = scrollRectRt.sizeDelta.x;
				createCount = minCount;
			}
			Debug.Log("minCount : " + minCount);
			Debug.Log("createCount : " + createCount);
			for (int i = 0; i < createCount; i++)
			{
				GameObject loop = Instantiate(loopObject);
				RectTransform loopRectTransform = loop.GetComponent<RectTransform>();
				loopRectTransform.SetParent(content);
				loopRectTransform.localScale = Vector3.one;
				loopObjectsList.Add(loopRectTransform);
				SetPositionOfIndex(loopRectTransform, i);
			}
		}

		void SetPositionOfIndex(RectTransform rt, int index){
			float x = (rt.sizeDelta.x + spacing) * index;
			Vector3 anchoredPosition = new Vector3(x, -300, 0);
			rt.anchoredPosition = anchoredPosition;
		}

		float lastValuX = 0;
		void OnScrolled(Vector2 value){
			leftPosOfContent = - content.anchoredPosition.x;
			rightPosOfContent = leftPosOfContent + scrollRectRt.sizeDelta.x;
			// if (leftIndex <= 0 || rightIndex >= count - 1)
			// 	return;
			float currentValueX = value.x;
			if (currentValueX > lastValuX){
				// 手指往左滑动，content往左走，判断 loopObjectsList[0] 的右边是否出去
				RectTransform rt = loopObjectsList[0];
				float rtRightPos = rt.sizeDelta.x + rt.anchoredPosition.x;
				if (rtRightPos < leftPosOfContent){
					loopObjectsList.RemoveAt(0);
					loopObjectsList.Add(rt);
					SetPositionOfIndex(rt, rightIndex);
					rightIndex += 1;
					leftIndex += 1;
				}
			}
			else if (currentValueX < lastValuX){
				// 手指往右滑动，content往右走，判断 loopObjectsList[loopObjectsList.Count - 1];] 的左边是否出去
				RectTransform rt = loopObjectsList[loopObjectsList.Count - 1];
				float rtLeftPos = rt.anchoredPosition.x;
				if (rtLeftPos > rightPosOfContent){
					loopObjectsList.RemoveAt(loopObjectsList.Count - 1);
					loopObjectsList.Insert(0, rt);
					rightIndex -= 1;
					leftIndex -= 1;
					SetPositionOfIndex(rt, leftIndex);
				}
			}
			lastValuX = currentValueX;
		}
	}
}
