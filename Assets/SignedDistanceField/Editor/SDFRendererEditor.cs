using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.Events;
using System;

// [CustomEditor(typeof(SDFRenderer))]
[CanEditMultipleObjects]
public class SDFRendererEditor : Editor {

	SDFRenderer target;

	void OnEnable(){
        Undo.undoRedoPerformed += UpdateView;
	}

	public override void OnInspectorGUI(){
		base.OnInspectorGUI();
		if (GUI.changed){
			target = (SDFRenderer)serializedObject.targetObject;
			UpdateView();
		}
	}

	void OnDisable(){
		Undo.undoRedoPerformed -= UpdateView;
	}

	void UpdateView(){
		// if (target != null)
		// 	target.UpdateViewInEditor();
	}
}