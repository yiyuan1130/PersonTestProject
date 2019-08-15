using UnityEngine;
using UnityEditor;
using System;
using System.IO;
using System.Text;
using UnityEditor.ProjectWindowCallback;
using System.Text.RegularExpressions;
namespace CreateLua
{
	public class CreateLuaScripts : AssetModificationProcessor{
		[MenuItem("Assets/Create/Lua Sctipt", false, 80)]
		public static void CreatNewLua() {
			int instanceId = 0;
			EndNameEditAction endNameEditAction = ScriptableObject.CreateInstance<CreateLuaAsset> ();
			string pathName = Path.Combine (GetSelectedPathOrFallback(), "NewLuaScript.lua");
			Texture2D texture2D = null;
//			Texture2D texture2D = AssetDatabase.LoadAssetAtPath("Assets/CreateLuaScripts/Editor/luaicon.png", typeof(Texture2D)) as Texture2D;
			string resourceFile = "Assets/CreateLuaScripts/Editor/TemplateLua.lua";
			ProjectWindowUtil.StartNameEditingIfProjectWindowExists (instanceId, endNameEditAction, pathName, texture2D, resourceFile);
		}

		public static string GetSelectedPathOrFallback() {
			string path = "Assets";
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

	class CreateLuaAsset:EndNameEditAction{
		public override void Action(int instanceId, string pathName, string resourceFile) {
			UnityEngine.Object o = CreateScriptAssetFromTemplate(pathName, resourceFile);
			ProjectWindowUtil.ShowCreatedAsset(o);
		}
		internal static UnityEngine.Object CreateScriptAssetFromTemplate(string pathName, string resourceFile) {
			string fullPath = Path.GetFullPath(pathName);
			StreamReader streamReader = new StreamReader(resourceFile);
			string text = streamReader.ReadToEnd();
			streamReader.Close();
			string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(pathName);
			text = Regex.Replace(text, "#Name#", fileNameWithoutExtension);

			bool encoderShouldEmitUTF8Identifier = true;
			bool throwOnInvalidBytes = false;
			UTF8Encoding encoding = new UTF8Encoding(encoderShouldEmitUTF8Identifier, throwOnInvalidBytes);
			bool append = false;
			StreamWriter streamWriter = new StreamWriter(fullPath, append, encoding);
			streamWriter.Write(text);
			streamWriter.Close();
			AssetDatabase.ImportAsset(pathName);
			return AssetDatabase.LoadAssetAtPath(pathName,typeof(UnityEngine.Object));
		}
	}
}