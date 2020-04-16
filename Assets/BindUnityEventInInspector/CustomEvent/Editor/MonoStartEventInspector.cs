using UnityEngine.Events;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(TestGameObject))]
public class MonoStartEventInspector : Editor{
    public override void OnInspectorGUI(){
        DrawDefaultInspector();
        TestGameObject myTarget = (TestGameObject)target;

        // myTarget.startMoveEvent = EditorGUILayout.IntField("Experience", myTarget.startMoveEvent);
    }
}
