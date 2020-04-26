using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoad : MonoBehaviour {
	void Awake(){
		DontDestroyOnLoad(gameObject);
		GameObject.Find("Canvas").transform.Find("Button").GetComponent<UnityEngine.UI.Button>().onClick.AddListener(LoadScene);
	}

	void LoadScene(){
		SceneManager.LoadScene("Scene2");
	}
}
