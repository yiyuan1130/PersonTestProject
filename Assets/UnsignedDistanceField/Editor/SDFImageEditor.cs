using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.Events;
using System;

[CustomEditor(typeof(SDFImage))]
[CanEditMultipleObjects]
public class SDFImageEditor : Editor {

	SDFImage target;
    UnityEngine.Object[] targets;

	void Awake(){
        targets = serializedObject.targetObjects;
        Undo.undoRedoPerformed += onUndo;
	}

    void onUndo(){
        ForeachTargets(() => {
            target.sprite = target._sprite;
            target.ramp = target._ramp;
            target.color = target._color;
            target.style = target._style;
            target.step = target._step;
        });
    }

	public override void OnInspectorGUI(){
        targets = serializedObject.targetObjects;
        if (!target)
            target = (SDFImage)serializedObject.targetObject;

		GUI_Sprite();

		GUI_Style();
		
		GUI_Ramp();

		GUI_Color();

		GUI_Step();
	}
	
	void GUI_Sprite(){
        Sprite sprite = EditorGUILayout.ObjectField("sprite - 距离场字母图", target._sprite, typeof(Sprite), false) as Sprite;
        if (GUI.changed){
            ForeachTargets(() => {
                Undo.RecordObject(target, "Changed");
                target.sprite = sprite;
            });
        }
	}

	void GUI_Style(){
		Style style = (Style)EditorGUILayout.EnumPopup("style - 字母样式", target._style);
		if (GUI.changed){
            ForeachTargets(() => {
                Undo.RecordObject(target, "Changed");
                target.style = style;
            });
		}
	}

	void GUI_Ramp(){
		if (target._style != Style.RAMP)
			return;
		Texture ramp = EditorGUILayout.ObjectField("ramp - 发光ramp图", target._ramp, typeof(Texture), false) as Texture;
		if (GUI.changed){
            ForeachTargets(() => {
                Undo.RecordObject(target, "Changed");
                target.ramp = ramp;
            });
		}
	}

	void GUI_Color(){
		if (target._style == Style.RAMP)
			return;
		Color color = EditorGUILayout.ColorField("color - 边缘或者纯色 颜色", target._color);
		if (GUI.changed){
            ForeachTargets(() => {
                Undo.RecordObject(target, "Changed");
                target.color = color;
            });
		}
	}

	void GUI_Step(){
		Step step = (Step)EditorGUILayout.EnumPopup("step - 笔画", target._step);
		if (GUI.changed){
            ForeachTargets(() => {
                Undo.RecordObject(target, "Changed");
                target.step = step;
            });
		}
	}

    void ForeachTargets(Action fun){
        for (int i = 0; i < targets.Length; i++)
        {
            target = (SDFImage)targets[i];
            fun();
        }
    }
	
}