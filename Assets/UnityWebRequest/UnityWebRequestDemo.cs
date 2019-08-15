using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using System;

public class UnityWebRequestDemo : MonoBehaviour {
	public Image downloadedImage;
	public Text downloadText;
	public Image processImage;
	public Text processText;

	void RestProcess(){
		processImage.fillAmount = 0f;
		processText.text = "0/100";
	}

	void UpdateProcess(float process){
		RestProcess ();
		processImage.fillAmount = process;
		processText.text = string.Format ("{0}/100", process * 100);
	}
		

	public void DownloadImage(){
		StartCoroutine (_DownloadImage((Texture2D texture2D) => {
			Sprite sprite = Sprite.Create (texture2D, new Rect(0, 0, texture2D.width, texture2D.height), Vector2.zero);
			downloadedImage.sprite = sprite;
		}));
	}

	IEnumerator _DownloadImage(Action<Texture2D> callback){
		string url = "http://attach.bbs.miui.com/forum/201505/27/172736r8qcystlxcil9s9l.jpg";
		UnityWebRequest uwRequest = UnityWebRequestTexture.GetTexture (url);
		uwRequest.timeout = 5;
		UnityWebRequestAsyncOperation operation = uwRequest.SendWebRequest ();
//		operation.priority;
		Debug.Log ("222 uwRequest.downloadProgress is " + uwRequest.downloadProgress);
		yield return operation;
		Debug.Log ("111 uwRequest.downloadProgress is " + uwRequest.downloadProgress);
		UpdateProcess (uwRequest.downloadProgress);
		if (operation.isDone) {
			byte[] buffer = uwRequest.downloadHandler.data;
			Texture2D texture2D = new Texture2D (1, 1);
			texture2D.LoadImage (buffer);
			if (callback != null) {
				callback(texture2D);
			}
		}
	}

	public void DownloadText(){
		RestProcess ();
		StartCoroutine (_DownloadText ((string text) => {
			downloadText.text = text;
		}));
	}

	IEnumerator _DownloadText(Action<string> callback){
		string url = "https://yiyuan1130.github.io/unity/2019/04/23/Unity-Memory-Manage-and-Optimize.html";
		UnityWebRequest uwRequest = UnityWebRequest.Get (url);
		uwRequest.timeout = 5;
		UnityWebRequestAsyncOperation operation = uwRequest.SendWebRequest ();
		yield return operation;
		if (operation.isDone) {
			string text = uwRequest.downloadHandler.text;
			if (callback != null) {
				callback(text);
			}
		}
	}

	public void DownloadHandlerFile(){
		RestProcess ();
	}

	IEnumerator _DownloadFile(){
		yield return 0;
	}
}
