using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.Events;
using System;

// [CustomEditor(typeof(SDFImage))]
[CanEditMultipleObjects]
public class SDFImageEditor : Editor {

	SDFImage target;

	void OnEnable(){
        Undo.undoRedoPerformed += UpdateView;
	}

	public override void OnInspectorGUI(){
		base.OnInspectorGUI();
		if (GUI.changed){
			target = (SDFImage)serializedObject.targetObject;
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