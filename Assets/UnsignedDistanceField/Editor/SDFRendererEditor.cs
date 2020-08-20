using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(SDFRenderer))]
public class SDFRendererEditor : Editor {

	SDFRenderer target;

	void Awake(){
		target = (SDFRenderer)(serializedObject.targetObject);
	}

	public override void OnInspectorGUI(){

		GUI_Sprite();

		GUI_Style();
		
		GUI_Ramp();

		GUI_Color();

		GUI_Step();
	}
	
	void GUI_Sprite(){
		target._sprite = EditorGUILayout.ObjectField("sprite - 距离场字母图", target._sprite, typeof(Sprite), false) as Sprite;
		if (GUI.changed){
			if (target._sprite != null){
				target.meshRenderer.material.SetTexture("_MainTex", target._sprite.texture);
			}else{
				target.meshRenderer.material.SetTexture("_MainTex", null);
			}
		}
	}

	void GUI_Style(){
		target._style = (Style)EditorGUILayout.EnumPopup("style - 字母样式", target._style);
		if (GUI.changed){
			target.meshRenderer.material.SetInt("_Style", (int)target._style);
		}
	}

	void GUI_Ramp(){
		if (target._style != Style.RIM)
			return;
		target._ramp = EditorGUILayout.ObjectField("ramp - 发光ramp图", target._ramp, typeof(Texture), false) as Texture;
		if (GUI.changed){
			if (target._ramp != null){
				target.meshRenderer.material.SetTexture("_RampTex", target._ramp);
			}
			else
			{
				target.meshRenderer.material.SetTexture("_RampTex", null);
			}
		}
	}

	void GUI_Color(){
		if (target._style == Style.RIM)
			return;
		target._color = EditorGUILayout.ColorField("color - 边缘或者纯色 颜色", target._color);
		if (GUI.changed){
			target.meshRenderer.material.SetVector("_Color", target._color);
		}
	}

	void GUI_Step(){
		target._step = (Step)EditorGUILayout.EnumPopup("step - 笔画", target._step);
		if (GUI.changed){
			target.meshRenderer.material.SetInt("_Step", (int)target._step);
		}
	}
	
}
