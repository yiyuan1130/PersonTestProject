using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace MiaoKids{

	public class MiaoKidsTime : MonoBehaviour {
		static DateTime dateFrom1970 = new DateTime(1970, 1, 1, 0, 0, 0, 0);
		static TimeSpan currentTs = new TimeSpan();
			
		static int utcTime;
		public static int UtcTime{
			get { 
				currentTs = DateTime.UtcNow - dateFrom1970;
				return Convert.ToInt32(currentTs.TotalSeconds);
			}
		}

		static int inAppTime;
		public static int InAppTime{
			get { 
				return Convert.ToInt32 (Time.unscaledTime);
			}
		}

		static int pauseTime = 0;
		static int lastPauseTime = 0;
		void OnApplicationPause(bool pauseStatus)
		{	
			if (pauseStatus) {
				
			} else {
				
			}
		}

		void OnDisable(){
			// 认为退出app了
		}
	}
}
