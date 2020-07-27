using UnityEngine;
using UnityEditor;
using System.IO;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;

public class ForeachObjectsInEditor {
	[MenuItem("Tools/ForeachObjectsInEditor")]
	public static void ForeachObjects(){
		string[] sceneArr = {
			"ForeachObjsScene1",
			"ForeachObjsScene2",
			"ForeachObjsScene3",
		};

		string sceneRootPath = "Assets\\ForeachObjectsInEditor";
		for (int i = 0; i < sceneArr.Length; i++)
		{
			string sceneName = sceneArr[i];
			string fullPath = Path.Combine(sceneRootPath, sceneName) + ".unity";
			Scene scene = EditorSceneManager.OpenScene(fullPath, OpenSceneMode.Additive);
			GameObject[] objects = scene.GetRootGameObjects();
			Debug.Log("<color=red>======================================</color>");
			for (int j = 0; j < objects.Length; j++)
			{
				Debug.LogFormat("<color=yellow>Scene : {0}, GameObject : {1}</color>", sceneName, objects[j].name);
			}
			EditorSceneManager.CloseScene(scene, false);
		}
	}
}
