using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeviceInfo : MonoBehaviour {
    public Text text;

	// Use this for initialization
	void Start () {
        string tex = "";
        //设备型号
        tex = tex + string.Format("SystemInfo.deviceModel -> {0} \n", SystemInfo.deviceModel);

        //设备名
        tex = tex + string.Format("SystemInfo.deviceName -> {0} \n", SystemInfo.deviceName);

        //设备id
        tex = tex + string.Format("SystemInfo.deviceUniqueIdentifier -> {0} \n", SystemInfo.deviceUniqueIdentifier);


        tex = tex + string.Format("SystemInfo.graphicsDeviceID -> {0} \n", SystemInfo.graphicsDeviceID);

        //系统版本
        tex = tex + string.Format("SystemInfo.operatingSystem -> {0} \n", SystemInfo.operatingSystem);


        tex = tex + string.Format("SystemInfo.operatingSystemFamily -> {0} \n", SystemInfo.operatingSystemFamily);

        // 运行内存 RAM
        tex = tex + string.Format("SystemInfo.systemMemorySize -> {0} \n", SystemInfo.systemMemorySize);

        text.text = tex;
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
