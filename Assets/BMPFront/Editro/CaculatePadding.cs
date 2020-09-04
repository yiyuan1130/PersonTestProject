using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class CaculatePadding {
	[MenuItem("Assets/Font/ChangePadding", false, 1)]
	public static void ChangePadding(){
		Object[] objs = Selection.objects;
		if (objs.Length != 1){
			Debug.LogWarning("Need choose one object");
			return;
		}
		Object obj = objs[0];
		if (obj.GetType() != typeof(Font)){
			Debug.LogWarning("object is not a Font type");
			return;
		}
		Font font = AssetDatabase.LoadAssetAtPath(AssetDatabase.GetAssetPath(obj), typeof(Font)) as Font;
		CharacterInfo[] characterInfos = font.characterInfo;
		int atlasPadding = 20;
		int padding = 10;
		for (int i = 1; i < characterInfos.Length; i++)
		{
			CharacterInfo info = characterInfos[i];
			Debug.LogFormat("index{0},minX{1},maxX{2},minY{3},maxY{4},glyphWidth{5},glyphHeight{6}", info.index, info.minX, info.maxX, info.minY, info.maxY, info.glyphWidth, info.glyphHeight);
			

			info.minX = -atlasPadding + padding;
			info.advance = info.glyphWidth - atlasPadding * 2 + padding * 2;
			info.glyphWidth = info.glyphWidth + padding;
			characterInfos[i] = info;
		}
		font.characterInfo = characterInfos;
		EditorUtility.SetDirty(obj);
		AssetDatabase.SaveAssets();
	}
}
