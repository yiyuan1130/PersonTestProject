using UnityEngine;

namespace MiaoKids {
	public enum FigureType
	{
		triangle, // 三角形
		quadrangle, // 四边形
	}
	public static class JudgeFigure{
		public static bool IsTargetFigure(GameObject gameObject, int figureType){
			return IsTargetFigure(gameObject, (FigureType)figureType);
		}
		public static bool IsTargetFigure(GameObject gameObject, FigureType figureType){
			Mesh mesh = CheckMesh(gameObject);
			if (mesh != null){
				return CheckFigure(mesh, figureType);
			}
			else{
				return false;
			}
		}

		static Mesh CheckMesh(GameObject gameObject){
			MeshFilter meshFilter = gameObject.GetComponent<MeshFilter>();
			if (!meshFilter){
				Debug.LogFormat("<color=red>GameObject {0} 未找到 MeshFliter 组件</color>", gameObject.name);
				return null;
			}
			Mesh mesh = meshFilter.mesh;
			if (!mesh){
				Debug.LogFormat("<color=red>GameObject {0} 未找到 MeshFliter 的 mesh</color>", gameObject.name);
				return null;
			}
			return mesh;
		}

		static bool CheckFigure(Mesh mesh, FigureType figureType){
			bool ret = false;
			switch (figureType)
			{
				case FigureType.triangle:
					ret = CheckTriangle(mesh);
					break;
				case FigureType.quadrangle:
					ret = CheckQuadrangle(mesh);
					break;
				default:
					break;
			}
			return ret;
		}

		static bool CheckTriangle(Mesh mesh){
			int vertexCount = mesh.vertexCount;
			Debug.Log("+++++++ " + vertexCount);
			return vertexCount == 3;
		}

		static bool CheckQuadrangle(Mesh mesh){
			int vertexCount = mesh.vertexCount;
			Debug.Log("+++++++ " + vertexCount);
			return vertexCount == 4;
		}
	}
}
