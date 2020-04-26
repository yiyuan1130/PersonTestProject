using UnityEditor;
using UnityEngine;
using System.IO;

namespace SetBundNameTest{
	public class Tools {

		[MenuItem("Tools/AutoSet AssetBundleName")]
		// 获取目标文件夹目录
		public static void AutoSetBundleName(){
			string path = Path.Combine (Application.dataPath, "SetBundNameTest/BundleFloders");
			DirectoryInfo dir_info = new DirectoryInfo (path);
			// 获取所有子文件夹
			DirectoryInfo[] dir_arr = dir_info.GetDirectories ();

			for (int i = 0; i < dir_arr.Length; i++) {
				DirectoryInfo current_dir = dir_arr [i];

				string dir_name = current_dir.Name;
				// 通过子文件夹名拼接成 assetsbundle 名
				string assetbundle_name = string.Format ("auto_set_{0}", dir_name.ToLower());

				string dir_path = current_dir.FullName;
				string asset_path = dir_path.Replace (Application.dataPath, "Assets");
				// 通过在工程内的路径获取 AssetImporter
				AssetImporter ai = AssetImporter.GetAtPath(asset_path);

				ai.assetBundleName = assetbundle_name;
				ai.assetBundleVariant = "variant"; // 也可以修改 AssetImporter 中 assetBundleVariant 属性
			}
		}
	}
}