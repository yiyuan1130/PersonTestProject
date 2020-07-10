using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using UnityEngine;
using UnityEditor;

public class ReplaceMaterialWindow : EditorWindow {

	[MenuItem("DevTool/ReplaceMaterialsShader")]

    static void Init()
    {
        ReplaceMaterialWindow replaceMaterialWindow = (ReplaceMaterialWindow)EditorWindow.GetWindow(typeof(ReplaceMaterialWindow), false, "ReplaceMaterialWindow", true);//创建窗口
        replaceMaterialWindow.Show();//展示
    }

	ReplaceMaterialWindow(){
		this.titleContent = new GUIContent("替换L0材质的Shader");
	}

	[SerializeField]
    private List<UnityEngine.Object> AssetList = new List<UnityEngine.Object>();

    private SerializedObject serObj;
    private SerializedProperty serPty;


    private void OnEnable()
    {
        serObj = new SerializedObject(this);
        serPty = serObj.FindProperty("AssetList");
    }

    private void OnGUI()
    {
        serObj.Update();
        EditorGUI.BeginChangeCheck();

		GUILayout.Space(10);
        GUI.skin.label.fontSize = 12;
		GUI.skin.label.fontStyle = FontStyle.Bold;
        GUI.skin.label.alignment = TextAnchor.MiddleCenter;
        GUILayout.Label("替换L0材质的Shader");

        EditorGUILayout.PropertyField(serPty, true);
        if (EditorGUI.EndChangeCheck())
        {
            serObj.ApplyModifiedProperties();
        }

		if (GUILayout.Button("确定")){
			ReplaceShader(GetMaterials());
		}
    }

	private List<Material> GetMaterials(){
		List<Material> materialList = new List<Material>();
		for (int i = 0; i < AssetList.Count; i++)
		{
			UnityEngine.Object obj = AssetList[i];
			Material material = GetMaterial(obj);
			if (material != null){
				materialList.Add(material);
			}
		}
		return materialList;
	}

	private Material GetMaterial(UnityEngine.Object obj){
		if (obj is UnityEditor.SceneAsset){
			SceneAsset sceneAsset = obj as SceneAsset;
			UnityEngine.SceneManagement.Scene scene = UnityEngine.SceneManagement.SceneManager.GetSceneByPath(AssetDatabase.GetAssetPath(obj));
			GameObject[] rootGameObjects = scene.GetRootGameObjects();
			List<GameObject> allGameObjects = new List<GameObject>();
			for (int i = 0; i < rootGameObjects.Length; i++)
				GetChild(rootGameObjects[i], allGameObjects);
			for (int i = 0; i < allGameObjects.Count; i++)
			{
				GameObject gameObject = allGameObjects[i];
				return GetMaterial(gameObject);
			}
		}else if (obj is GameObject){
			GameObject go = obj as GameObject;
			Renderer renderer = go.GetComponent<Renderer>();
			if (renderer){
				if (renderer.material){
					return renderer.material;
				}
			}
		}else if (obj is Material){
			return obj as Material;
		}
		return null;
	}

	private void GetChild(GameObject gameObject, List<GameObject> allGameObjects){
		allGameObjects.Add(gameObject);
		Transform transform = gameObject.transform;
		if (transform.childCount > 0){
			for (int i = 0; i < transform.childCount; i++)
			{
				GameObject subGameObject = transform.GetChild(i).gameObject;
				GetChild(subGameObject, allGameObjects);
			}
		}
	}



	private void ReplaceShader(List<Material> materials){
		for (int i = 0; i < materials.Count; i++)
		{
			Material mat = materials[i];
			mat.shader = Shader.Find("iHuman/Transparent2D");
			EditorUtility.SetDirty(mat);
		}	
	}

	// string path;
	// Rect rect;
	// void OnGUI()
    // {
    //     EditorGUILayout.LabelField("路径");
    //     rect = EditorGUILayout.GetControlRect(GUILayout.Width(400));
 
    //     path = EditorGUI.TextField(rect, path);
 
    //     if(true)
    //     {
    //         DragAndDrop.visualMode = DragAndDropVisualMode.Generic;
    //         if(DragAndDrop .paths != null && DragAndDrop .paths .Length > 0)
    //         {
    //             path = DragAndDrop.paths[0];
    //         }
    //     }
    // }

}
