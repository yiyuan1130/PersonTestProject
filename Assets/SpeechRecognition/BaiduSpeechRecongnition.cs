using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaiduSpeechRecongnition : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnGUI(){
		if (ShowGUIButton("百度"))
		{
			SpeechRecognizeBaidu();
		}
	}

	bool ShowGUIButton(string buttonName)
    {
        return GUILayout.Button(buttonName, GUILayout.Height(Screen.height / 20), GUILayout.Width(Screen.width / 5));
    }

	void SpeechRecognizeBaidu(){
		string audioPath = "E:/audio/dd.wav";

	}
}
