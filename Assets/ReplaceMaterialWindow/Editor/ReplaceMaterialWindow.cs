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
			GetMaterials();
			ReplaceShader();
			// AssetDatabase.SaveAssets();
		}
    }


	List<Material> materialList;
	private void GetMaterials(){
		materialList = new List<Material>();
		for (int i = 0; i < AssetList.Count; i++)
		{
			UnityEngine.Object obj = AssetList[i];
			EditorUtility.SetDirty(obj);
			GetMaterial(obj);
		}
	}

	private void GetMaterial(UnityEngine.Object obj){
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
				GetMaterial(gameObject);
			}
		}else if (obj is GameObject){
			GameObject go = obj as GameObject;
			Renderer renderer = go.GetComponent<Renderer>();
			ParticleSystem particleSystem = go.GetComponent<ParticleSystem>();
			if (renderer && !particleSystem){
				if (renderer.sharedMaterial){
					int instanceID = renderer.sharedMaterial.GetInstanceID();
					string objPath = AssetDatabase.GetAssetPath(instanceID);
					UnityEngine.Object matObj = AssetDatabase.LoadAssetAtPath(objPath, typeof(Material));
					if (matObj)
						materialList.Add(matObj as Material);
				}
			}
		}else if (obj is Material){
			materialList.Add(obj as Material);
		}
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

	private void ReplaceShader(){
		for (int i = 0; i < materialList.Count; i++)
		{
			Material mat = materialList[i];
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
