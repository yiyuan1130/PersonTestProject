using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnderLine : MonoBehaviour {
	Text mText;
	public Color lineColor = Color.black;
	public float lineWidth = 2.0f;

	public Canvas canvas;
	public Vector2 underLineOffset;

	RectTransform underLineRectTrans;

	void Awake (){
		mText = gameObject.GetComponent<Text>();
		if (underLineRectTrans == null) {
			GameObject go = new GameObject("underLineImage");
			Image img = go.AddComponent<Image>();
			img.color = lineColor;
			underLineRectTrans = go.GetComponent<RectTransform>();
			underLineRectTrans.SetParent(transform);
			underLineRectTrans.sizeDelta = Vector3.zero;
			underLineRectTrans.localScale = Vector3.one;
		}

		int textAlignmentNum = (int)mText.alignment;
		switch (textAlignmentNum % 3)
		{
			case 0: // left
				underLineRectTrans.pivot = new Vector2(0.0f, 0.5f);
				underLineRectTrans.anchorMax = new Vector2(0.0f, 0.5f);
				underLineRectTrans.anchorMin = new Vector2(0.0f, 0.5f);
			break;
			case 1: // center
				underLineRectTrans.pivot = new Vector2(0.5f, 0.5f);
				underLineRectTrans.anchorMax = new Vector2(0.5f, 0.5f);
				underLineRectTrans.anchorMin = new Vector2(0.5f, 0.5f);
			break;
			case 2: // right
				underLineRectTrans.pivot = new Vector2(1.0f, 0.5f);
				underLineRectTrans.anchorMax = new Vector2(1.0f, 0.5f);
				underLineRectTrans.anchorMin = new Vector2(1.0f, 0.5f);
			break;
		}
	}
	
    void RefreshUnderLine(Canvas targetCanvas, Text targetText)
    {
        string calculateStr = targetText.text;

        // TextGenerator textGen = new TextGenerator(calculateStr.Length);
        // Vector2 extents = targetText.GetComponent<RectTransform>().rect.size;
        // textGen.Populate(calculateStr, targetText.GetGenerationSettings(extents));

		// int startPointIndex = 3;
		// Vector3 startPos = textGen.verts[startPointIndex].position / targetCanvas.scaleFactor;
		// Debug.Log(startPos);
		// Debug.Log(transform.position);
		// int endPointIndex = (calculateStr.Length - 1) * 4 + 2;
		// Vector3 endPos = textGen.verts[endPointIndex].position / targetCanvas.scaleFactor;

		// float lineLength = Mathf.Abs(startPos.x - endPos.x);

		// mText.preferredWidth
		// Debug.Log(mText.preferredWidth + "____" + lineLength);
		
		underLineRectTrans.anchoredPosition = new Vector3(0, 0 - mText.preferredHeight / 2.0f, 0) + new Vector3(underLineOffset.x, underLineOffset.y, 0);
		underLineRectTrans.sizeDelta = new Vector2(mText.preferredWidth, lineWidth);
    }

	void RefreshUnderLine(){
		string str = mText.text;
		Debug.Log(str.Split('\n').Length);
		underLineRectTrans.anchoredPosition = new Vector3(0, 0 - mText.preferredHeight / 2.0f, 0) + new Vector3(underLineOffset.x, underLineOffset.y, 0);
		underLineRectTrans.sizeDelta = new Vector2(mText.preferredWidth, lineWidth);
	}
	

	void Start(){
		Test();
	}

	void Test(){
		InputField inputField = GameObject.Find("InputField").GetComponent<InputField>();
		inputField.onEndEdit.AddListener((string val) => {
			mText.text = val;
			RefreshUnderLine();
		});
	}

}
