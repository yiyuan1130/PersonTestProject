using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System.IO;
using Newtonsoft.Json.Linq;
using System;

public class CreateByData : MonoBehaviour
{
    [Serializable]
    public struct prefab {
        public string name;
        public GameObject go;
    }
    public prefab[] prefabs;
    public float scale = 1f;
    List<Transform> allTransform;
    void Start()
    {
        Dictionary<string, GameObject> prefabDict = new Dictionary<string, GameObject>();
        for (int i = 0; i < prefabs.Length; i++)
        {
            string name = prefabs[i].name;
            GameObject go = prefabs[i].go;
            prefabDict.Add(name, go);
        }

        string dataPath = Application.dataPath + "/PlanB/data.json";
        StreamReader sReader = File.OpenText(dataPath);
        JsonTextReader jReader = new JsonTextReader(sReader);
        JArray jarray = (JArray)JToken.ReadFrom(jReader);
        List<Vector3> positionList = new List<Vector3>();
        List<string> nameList = new List<string>();
        int length = jarray.Count;
        for (int i = 0; i < length; i++)
        {
            JArray arr = (JArray)jarray[i];
            Vector3 pos = new Vector3((float)arr[0], (float)arr[1], (float)arr[2]);
            positionList.Add(pos);
            string name = (string)arr[3];
            nameList.Add(name);
        }

        Transform parent = new GameObject("Parent").transform;
        allTransform = new List<Transform>();
        for (int i = 0; i < length; i++)
        {
            Vector3 pos = positionList[i];
            string name = nameList[i];
            GameObject go = Instantiate(prefabDict[name]);
            go.SetActive(true);
            go.name = name;
            go.transform.position = pos;
            go.transform.SetParent(parent);
            allTransform.Add(go.transform);
        }
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < allTransform.Count; i++)
        {
            Transform t = allTransform[i];
            t.localScale = Vector3.one * scale;
        }
    }
}
