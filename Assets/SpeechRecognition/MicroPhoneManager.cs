using UnityEngine;
using System.Collections;

//挂载在空物体上，用来实现录音和播放
public class MicroPhoneManager : MonoBehaviour
{
    //声音片段
    private AudioClip clip;
    //声音组件
    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource> ();
		StartRecord ();
    }

    void Update()
    {
        // if (Input.GetKeyDown(KeyCode.Space)) {
        //     StartRecord ();
        // }
        // if (Input.GetKeyUp(KeyCode.Space)) {
        //     PlayRecord ();
        // }
    }

    //开始录音
    void StartRecord ()
    {
        //参数1:null,默认麦克风
        //参数2: 是否循环录制
        //参数3: 录制时长
        //参数4: 频率
        clip = Microphone.Start (null,false,30,8000);
    }

    //播放录音
    void PlayRecord ()
    {
        Microphone.End (null);
        //播放一个声音片段
        audioSource.PlayOneShot (clip);
    }

	// public void StartRecord(){
	// 	Microphone.Start (null,false,30,8000);
	// }

	// public void EndRecord(){
	// 	Microphone.End (null);
	// }
}