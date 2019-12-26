using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZXing;
using ZXing.QrCode;
using UnityEngine.UI;

public class CodeScanController : MonoBehaviour {

	WebCamTexture texture;
	public RawImage rawImage;

	BarcodeReader barcodeReader;

	public Text text;

	bool IsScanning = false;

	public void OpenCamera(){
		StartCoroutine(_OpenCamera());
	}

	IEnumerator _OpenCamera(){
		yield return Application.RequestUserAuthorization(UserAuthorization.WebCam);
		if (Application.HasUserAuthorization(UserAuthorization.WebCam))
        {
            //先获取设备
            WebCamDevice[] device = WebCamTexture.devices;
 
            string deviceName = device[0].name;
            //然后获取图像
            texture = new WebCamTexture(deviceName);
			
            //开始实施获取
            texture.Play();

			rawImage.texture = texture;

			IsScanning = true;
			barcodeReader = new BarcodeReader();
        }
	}

	Color32[] data;//二维码图片信息以像素点颜色信息数组存放
 
    /// <summary>
    /// 识别摄像机图片中的二维码信息
    /// 打印二维码识别到的信息
    /// </summary>
    void ScanQRCode()
    {
        //7、获取摄像机画面的像素颜色数组信息
        data = texture.GetPixels32();
        //8、获取图片中的二维码信息
        Result result = barcodeReader.Decode(data,texture.width, texture.height);
        //如果获取到二维码信息了，打印出来
        if (result != null)
        {
            Debug.Log(result.Text);//===》==》===》 这是从二维码识别出来的信息
            // text.text/ = result.Text;//显示扫描信息
			text.text = result.Text;
 
            //扫描成功之后的处理
            IsScanning = false;
            texture.Stop();
        }
	}

	void Update(){
		if (IsScanning){
			ScanQRCode();
		}
	}
}
