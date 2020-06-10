using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class UIController : MonoBehaviour {

	public Text timeText;

	void Update(){
		timeText.text = DateTime.Now.ToString("yyyy/MM/dd\nhh:mm:ss"); 
	}
}
