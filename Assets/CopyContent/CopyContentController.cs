using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CopyContentController : MonoBehaviour {

	InputField inputBeforeCopy;
	InputField inputAfterCopy;
	Button copyBtn;

	void Awake(){
		inputBeforeCopy = transform.Find ("InputFieldBeforeCopy").GetComponent<InputField>();
		inputAfterCopy = transform.Find ("InputFieldAfterCopy").GetComponent<InputField>();
		copyBtn = transform.Find ("CopyButton").GetComponent<Button> ();
		copyBtn.onClick.AddListener (OnClickCopy);
	}
		
	void OnClickCopy(){
		GUIUtility.systemCopyBuffer = inputBeforeCopy.text;
		Debug.LogFormat ("复制的文本：{0}", GUIUtility.systemCopyBuffer);
	}

}
