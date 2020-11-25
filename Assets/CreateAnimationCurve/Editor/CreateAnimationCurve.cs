using UnityEngine;
using UnityEditor;
using System;
using System.IO;
using System.Text;
using UnityEditor.ProjectWindowCallback;
public class CreateLuaScripts : AssetModificationProcessor{
    [MenuItem("Assets/Create/Animation Curve", false, 80)]
    public static void CreatNewAnimationCurveAsset() {
        int instanceId = 0;
        EndNameEditAction endNameEditAction = ScriptableObject.CreateInstance<CreateCurveAssetAction> ();
        string pathName = Path.Combine (GetSelectedPathOrFallback(), "NewAnimation.curve.asset");
        ProjectWindowUtil.StartNameEditingIfProjectWindowExists (instanceId, endNameEditAction, pathName, null, null);
    }

    public static string GetSelectedPathOrFallback() {
        string path = "";
        foreach (UnityEngine.Object obj in Selection.GetFiltered(typeof(UnityEngine.Object), SelectionMode.Assets)) {
            path = AssetDatabase.GetAssetPath(obj);
            if (!string.IsNullOrEmpty(path)&&File.Exists(path)) {
                path = Path.GetDirectoryName(path);
                break;
            }
        }
        return path;
    }
}

class CreateCurveAssetAction:EndNameEditAction{
    public override void Action(int instanceId, string pathName, string resourceFile) {
        AnimationCurveAsset asset = ScriptableObject.CreateInstance<AnimationCurveAsset>();
        AssetDatabase.CreateAsset(asset, pathName);
        UnityEngine.Object o = AssetDatabase.LoadAssetAtPath(pathName, typeof(UnityEngine.Object));
        ProjectWindowUtil.ShowCreatedAsset(o);
    }
}