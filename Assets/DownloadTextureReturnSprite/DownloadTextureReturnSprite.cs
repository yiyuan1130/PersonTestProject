using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using UnityEngine.Events;
using UnityEngine.Networking;

public class DownloadTextureReturnSprite : MonoBehaviour {

	public Image targetImage;

	public void OnClickDownloadButton(){
		Debug.Log ("Download");
//		DownloadOrGetLocalImage ();
	}

	// Use this for initialization
	void Start () {
		StartCoroutine(DownLoadImageWebRequest());

	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void ChangeSprite(Sprite sprite){
		targetImage.sprite = sprite;
	}

	IEnumerator DownLoadImageWebRequest(){
		
		UnityWebRequest uwr = UnityWebRequestTexture.GetTexture ("http://i2.hdslb.com/bfs/archive/5dd37ef4b64b8abeef13126512ace25c5db2df09.jpg");
		uwr.timeout = 5;
		yield return uwr.SendWebRequest();
		byte[] bytes = uwr.downloadHandler.data;
		Texture2D tex = new Texture2D (1, 1);
		tex.LoadImage (bytes);
		targetImage.sprite = Sprite.Create (tex, new Rect(0, 0, tex.width, tex.height), Vector2.zero);
		targetImage.GetComponent<RectTransform> ().sizeDelta = new Vector2 (tex.width, tex.height) * 0.5f;
	}

	void DownloadOrGetLocalImage(){
		string url = "http://miaokids.oss-cn-qingdao.aliyuncs.com/website/news/2ada8f89ffd750b43362889d27df0a0a.jpg";
		string savePath = Application.persistentDataPath + "/teacher_wechat_qrcode.png";
		Debug.Log ("savePath:" + savePath);
		StartCoroutine (DownloadTexture(url, savePath, ChangeSprite));
	}

	IEnumerator DownloadTexture(string url, string savePath, UnityAction<Sprite> callback){
		print ("StartCoroutine DownloadTexture");
		Texture2D texture = null;
		if (File.Exists (savePath)) {
			Debug.Log ("从本地拿");
			FileStream fs = new FileStream(savePath, FileMode.Open);
			byte[] buffer = new byte[fs.Length];
			fs.Read(buffer, 0, buffer.Length);
			fs.Close();
			texture = new Texture2D(2, 2);
			var iSLoad = texture.LoadImage(buffer);
			texture.Apply();
		} else {
			Debug.Log ("从网络获取链接 并下载");
			WWW www = new WWW (url);
			yield return www;
			if (www.isDone) {
				texture = www.texture;
				byte[] dataBytes = texture.EncodeToPNG();
				FileStream fs = File.Open(savePath, FileMode.CreateNew);
				fs.Write(dataBytes, 0, dataBytes.Length);
				fs.Flush ();
				fs.Close ();
			}
		}
		Sprite sprite = Sprite.Create (texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero);
		callback (sprite);
	}
}
