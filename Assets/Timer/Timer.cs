using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using MiaoKids;

public class Timer : MonoBehaviour {

	public Text showText;
	string _showText = "";
	string ShowText {
		get {
			return _showText;
		}
		set {
			_showText = showText.text + "\n" + value;
			showText.text = _showText;
		}
	}

	void OnApplicationPause(bool pauseStatus)
	{	
		if (pauseStatus) {
//			ShowText = "暂停" + Convert.ToInt64(Time.realtimeSinceStartup);
			ShowText = "暂停 utc " + MiaoKidsTime.UtcTime;
			ShowText = "暂停 inApp " + MiaoKidsTime.InAppTime;
		} else {
			ShowText = "开始 utc " + MiaoKidsTime.UtcTime;
			ShowText = "开始 inApp " + MiaoKidsTime.InAppTime;
		}
	}

	public void ShowTime()
	{
		ShowText = "utc " + MiaoKidsTime.UtcTime;
		ShowText = "inApp  " + MiaoKidsTime.InAppTime;
	}

	public void ClearTex()
	{
		showText.text = "";
	}

	void OnApplicationQuit()
	{
		ShowText = "退出";
	}
}

//static class MiaoKidsTime {
//	static MiaoKidsTime instance;
//	public static MiaoKidsTime Instance{
//		get { 
//			if (instance == null) {
//				instance = new MiaoKidsTime ();
//			}
//			return instance;
//		}
//	}
//	public static int utcTime;
//	public static int inAppTime;
//}
