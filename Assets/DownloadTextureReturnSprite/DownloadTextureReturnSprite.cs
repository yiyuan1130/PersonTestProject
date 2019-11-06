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
		targetImage.sprite = null;
		DownloadTexture();
	}

	// Use this for initialization
	void Start () {
		DownloadTexture();
	}
	
	void DownloadTexture(){
		string url = "https://miaokids.oss-cn-qingdao.aliyuncs.com/website/pops/image/784f734ae1723a594296cefc7e82c4ce.jpg";
		DownloadTexture(url, (Texture2D tex) => {
			Sprite sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), Vector2.zero);
			targetImage.sprite = sprite;
		});
	}

	public void DownloadTexture(string url, System.Action<Texture2D> callback){
			StartCoroutine (_DownloadTexture (url, callback));
		}
		IEnumerator _DownloadTexture(string url, System.Action<Texture2D> callback){
			UnityWebRequest uwr = UnityWebRequestTexture.GetTexture (url);
			uwr.timeout = 1;
			#if SUPPORT_SSL
			if (url.ToLower().StartsWith("https://"))
			{
				ServicePointManager.ServerCertificateValidationCallback = MyRemoteCertificateValidationCallback;
			}
			#endif
			yield return uwr.SendWebRequest();
			byte[] bytes = uwr.downloadHandler.data;
			Texture2D tex = new Texture2D (1, 1, TextureFormat.ARGB4444, false);
			byte[] tmp = new byte[bytes.Length / 16];
			for (int i = 0; i < tmp.Length; i++)
			{
				tmp[i] = bytes[i];
			}
			tex.LoadImage (tmp);
			// tex.LoadImage (bytes);
			Debug.LogFormat("bytes Lenght -> {0}", bytes.Length);
			if (callback != null) {
				callback(tex);
			}
		}
}
