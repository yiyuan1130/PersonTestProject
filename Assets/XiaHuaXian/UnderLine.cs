using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnderLine : MonoBehaviour {
	public RectTransform imgRectTransForm;
	public Text mText;

	public Canvas canvas;

	void Update() {
		
	}

	string str = "";
	public void OnClick(){
		str += Random.Range(0, 10);
		mText.text = str;
		float width =CalculateStrWidth(canvas, mText);
		imgRectTransForm.sizeDelta = new Vector2(width, 2.0f);
	}
	
    float CalculateStrWidth(Canvas targetCanvas, Text targetText)
    {
        string calculateStr = targetText.text;

        TextGenerator textGen = new TextGenerator(calculateStr.Length);
        Vector2 extents = targetText.GetComponent<RectTransform>().rect.size;
        textGen.Populate(calculateStr, targetText.GetGenerationSettings(extents));

		int startPointIndex = 0;
		Vector3 startPos = textGen.verts[startPointIndex].position / targetCanvas.scaleFactor;
		int endPointIndex = (calculateStr.Length - 1) * 4 + 1;
		Vector3 endPos = textGen.verts[endPointIndex].position / targetCanvas.scaleFactor;

		float width = Mathf.Abs(startPos.x - endPos.x);
		return width;
    }

}
