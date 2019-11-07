using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MiaoKids {

	/*
		图片切割解决方案 20191106 @EamonnLi

		1. 通过 PolygonCollider 获取图像顶点坐标 vertices
		2. 用过 vertices 获取图像外围线段 lines
		3. 计算画的线 drawLine 和 lines 的所有交点 points
		4. 把原图顶点按照 drawLine 进行分块
		5. 把分好区域的点顺时针顺序排序
		6. 创建mesh
		7. 调整贴图UV
	 */
	public class SliceSprite : MonoBehaviour {

		// public GameObject go;
		LineRenderer lr;

		[Header("制定的材质球，名字：SliceSprite_Mesh")]
		public Material material;

		[Header("偏移量：越大小孩子切割时候容错率越高，自动吸附到顶点")]
		[Range(0, 1)]
		public float offset = 0.05f;

		[Header("画的线宽度")]
		[Range(0, 1)]
		public float lineWidth = 0.01f;

		int sortingLayer = 1;

		public int orderInLayer = 0;

		public Material lineMaterial;

		

		// 画的线起点
		Vector3 startPos;
		// 画的线终点
		Vector3 endPos;
		// 画的线段点数组
		Vector3[] points;
		// 是否正在画线
		bool drawingLine;

		// 图片最小Y值
		float textureMinY;
		// 图片最大Y值
		float textureMaxY;
		// 图片最小X值
		float textureMinX;
		// 图片最大X值
		float textureMaxX;
		void Awake() {
			SpriteRenderer spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
			lr = new GameObject("line").AddComponent<LineRenderer>();
			sortingLayer = spriteRenderer.sortingLayerID;
			orderInLayer = spriteRenderer.sortingOrder + 1;
		}

		void Update(){
			DrawLine();
			if (Input.GetMouseButtonDown(0)){
				startPos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10));
				drawingLine = true;
			}
			if (Input.GetMouseButtonUp(0)){
				endPos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10));
				drawingLine = false;
				DoSlice();
				GameObject.DestroyImmediate(lr.gameObject);
			}
			endPos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10));
		}

		void DrawLine(){
			if (!drawingLine || startPos == null || endPos == null)
				return;
			points = new Vector3[2]{startPos, endPos};		
			if (lr){
				lr.startWidth = lineWidth; 
				lr.endWidth = lineWidth;
				lr.useWorldSpace = false;
				lr.sortingLayerID = sortingLayer;
				lr.sortingOrder = orderInLayer;
				lr.positionCount = points.Length;
				lr.material = lineMaterial;
				lr.SetPositions (points);
			}
		}

		void DoSlice(){
			// 获取去碰撞体勾勒出得顶点
			Vector2[] vertices = GetVerticesFromPolygonCollider();

			// 通过图像顶点计算图像UV外框大小，为uv坐标 0-1 标准化准别
			GetUVAttrubit(vertices);

			// 获取图像顶点组成的外线段
			List<Line> lines = GetLinesFromVertices(vertices);

			// 计算画的线和图像线段的交点
			List<Vector2> points = CaculateIntersectionPoints(lines);
			if (points.Count != 2){
				Debug.Log("<color=green> 交点个数不是2，无法切割 </color>");
				return;
			}
			
			// 合并交点、顶点距离相近的点
			points = MergePoint(points, vertices);

			// 合并交点和原图像顶点
			Vector2[] allPoints = new Vector2[vertices.Length + points.Count];
			vertices.CopyTo(allPoints, 0);
			points.ToArray().CopyTo(allPoints, vertices.Length);

			// 去重
			List<Vector2> allPointsList = new List<Vector2>();
			for (int i = 0; i < allPoints.Length; i++)
			{
				Vector2 point = allPoints[i];
				int sameIndex = allPointsList.FindIndex((Vector2 v) => {
 					return point == v;
				});
				if (sameIndex < 0){
					allPointsList.Add(point);
				}
			}

			// 按照画的线段将点分区域，点和线关系：1.点在线上 2.点在线上方 3.点在线下方
			// Dictionary<string, List<Vector2>> posOfPoints = Line.GetPositionOfPointWithLine(new Line(startPos, endPos), allPoints);
			Dictionary<string, List<Vector2>> posOfPoints = Line.GetPositionOfPointWithLine(new Line(startPos, endPos), allPointsList.ToArray());

			// 根据点线关系点集把点分两部分，每部分的点都是即将组成mesh的点集
			List<Vector2> part1Vertices = new List<Vector2>();
			List<Vector2> part2Vertices = new List<Vector2>();
			SeparatePointsAsVertices(posOfPoints, out part1Vertices, out part2Vertices);

			// 两个点集分别顺时针排序，为创建mesh准备
			Vector2[] srotedVertiesPart1 = SortPointsClockwise(part1Vertices.ToArray());
			Vector2[] srotedVertiesPart2 = SortPointsClockwise(part2Vertices.ToArray());

			// 通过两个点集创建新的GameObject并添加Mesh、UV切图
			GameObject part1Obj = CreateMeshGameObject(srotedVertiesPart1);
			GameObject part2Obj = CreateMeshGameObject(srotedVertiesPart2);

			float k = 0;
			if (startPos.x != endPos.x){
				k = (endPos.y - startPos.y) / (endPos.x - startPos.x);
			}
			float direct = 1.0f;
			if (k > 0){
				direct = -1.0f;
			} else {
				direct = 1.0f;
			}
			part1Obj.transform.position = part1Obj.transform.position + new Vector3(0.1f * direct, 0.1f, 0);
			part2Obj.transform.position = part2Obj.transform.position - new Vector3(0.1f * direct, 0.1f, 0);
			gameObject.SetActive(false);
		}

		void Swap(ref GameObject go1, ref GameObject go2){
			Debug.Log("swap");
			GameObject go = go1;
			go1 = go2;
			go2 = go;
		}

		// 通过Collider获取顶点坐标
		Vector2[] GetVerticesFromPolygonCollider(){
			SpriteRenderer sr = gameObject.GetComponent<SpriteRenderer>();
			Sprite sprite = sr.sprite;
			PolygonCollider2D collider = gameObject.GetComponent<PolygonCollider2D>();
			Vector2[] vertices = collider.points;
			return vertices;
		}

		void GetUVAttrubit(Vector2[] vertices){
			List<Vector2> tmpVertices = new List<Vector2>(vertices);
			tmpVertices.Sort((Vector2 a, Vector2 b) => {
				if (a.x < b.x)
					return -1;
				else if (a.x > b.x)
					return 1;
				else
					return 0;
			});
			float minX = tmpVertices[0].x;
			float maxX = tmpVertices[tmpVertices.Count - 1].x;
			tmpVertices.Sort((Vector2 a, Vector2 b) => {
				if (a.y < b.y)
					return -1;
				else if (a.y > b.y)
					return 1;
				else
					return 0;
			});
			float minY = tmpVertices[0].y;
			float maxY = tmpVertices[tmpVertices.Count - 1].y;
			textureMinY = minY;
			textureMaxY = maxY;
			textureMinX = minX;
			textureMaxX = maxX;
		}

		// 通过顶点获取所有线段
		List<Line> GetLinesFromVertices(Vector2[] vertices){
			List<Line> lines = new List<Line>();
			for (int i = 0; i < vertices.Length; i++)
			{
				int startIndex = i;
				int endIndex = i + 1;
				if (endIndex == vertices.Length){
					endIndex = 0;
				}
				Line line = new Line(vertices[startIndex], vertices[endIndex]);
				lines.Add(line);
			}
			return lines;
		}

		// 获取画的线和所有线段的交点
		List<Vector2> CaculateIntersectionPoints(List<Line> lines){
			Line drawLine = new Line(startPos, endPos);
			List<Vector2> points = new List<Vector2>();
			for (int i = 0; i < lines.Count; i++)
			{
				Vector2 point = new Vector2(0, 0);
				bool havePoint = Line.GetIntersectionPoint(lines[i], drawLine, out point);
				if (havePoint){
					points.Add(point);
				}
			}
			return points;
		}

		// 合并和交点相近的顶点，将符合条件的交点坐标置为顶点坐标
		// 将画的线startPos和endPos置为两个交点坐标，保证交点在线上
		List<Vector2> MergePoint (List<Vector2> points, Vector2[] vertices){
			for (int i = 0; i < points.Count; i++)
			{
				for (int j = 0; j < vertices.Length; j++)
				{
					if (Vector2.Distance(points[i], vertices[j]) <= offset){
						points[i] = vertices[j];
						break;
					}
				}
			}
			startPos = points[0];
			endPos = points[1];
			return points;
		}


		// 根据线分离顶点为两块区域
		void SeparatePointsAsVertices(Dictionary<string, List<Vector2>> posOfPoints, out List<Vector2> vertiesPart1, out List<Vector2> vertiesPart2){
			// 画的线上半部分部分
			vertiesPart1 = new List<Vector2>();
			// // 画的线下半部分部分
			vertiesPart2 = new List<Vector2>();
			foreach (var item in posOfPoints)
			{
				string key = item.Key;
				for (int i = 0; i < item.Value.Count; i++)
				{
					Vector2 point = item.Value[i];
					if (key == "up"){
						vertiesPart1.Add(point);
					}
					else if (key == "down"){
						vertiesPart2.Add(point);
					}
					else{
						vertiesPart1.Add(point);
						vertiesPart2.Add(point);
					}
				}
			}
		}

		// 点集顺时针排序
		Vector2[] SortPointsClockwise(Vector2[] vertices){
			List<Vector2> verticesList = new List<Vector2>(vertices);
			verticesList.Sort((Vector2 a, Vector2 b) => {
				if (a.x < b.x)
					return -1;
				else if (a.x == b.x) {
					if (a.y < b.y)
						return -1;
					else
						return 1;
				}
				else if (a.x > b.x)
					return 1;
				else
					return 1;
			});
			Vector2 startPoint = verticesList[0];
			// 得到初第一个点后的其他点，用作tan排序
			List<Vector2> otherPoints = new List<Vector2>(verticesList);
			otherPoints.RemoveAt(0);
			otherPoints = CaculatePointsTan(startPoint, otherPoints);
			otherPoints.Insert(0, startPoint); // 把第一个点加回去
			Vector2[] sotredVertices = otherPoints.ToArray();
			return sotredVertices;
		}

		// 计算tan值进行排序
		List<Vector2> CaculatePointsTan(Vector2 sartPoint, List<Vector2> otherPoints){
			otherPoints.Sort((Vector2 a, Vector2 b) => {
				float durXA = a.x - sartPoint.x;
				float durYA = a.y - sartPoint.y;
				if (durXA == 0 && durYA > 0)
					return -1;
				else if (durXA == 0 && durYA < 0)
					return 1;

				float durXB = b.x - sartPoint.x;
				float durYB = b.y - sartPoint.y;
				if (durXB == 0 && durYB > 0)
					return -1;
				else if (durXB == 0 && durYB < 0)
					return 1;

				float tanA = (a.y - sartPoint.y) / (a.x - sartPoint.x);
				float tanB = (b.y - sartPoint.y) / (b.x - sartPoint.x);

				if (tanA > tanB)	
					return -1;
				else if (tanA < tanB)
					return 1;
				else
					return 0;
			});
			return otherPoints;
		}

		// 通过顶点创建创建mesh生成GameObject
		GameObject CreateMeshGameObject(Vector2[] vertices){
			GameObject partObj = new GameObject("part");
			MeshRenderer meshRenderer = partObj.AddComponent<MeshRenderer>();
			MeshFilter meshFilter = partObj.AddComponent<MeshFilter>();
			Texture2D mainTexture = gameObject.GetComponent<SpriteRenderer>().sprite.texture;
			material.SetTexture("_MainTex", mainTexture);
			meshRenderer.material = material;
			Mesh mesh = new Mesh();
			meshFilter.mesh = mesh;

			Vector3[] meshVertices = new Vector3[vertices.Length];
			Vector3[] meshNormals = new Vector3[vertices.Length];
			Vector2[] meshUv = new Vector2[vertices.Length];
			float width = textureMaxX - textureMinX;
			float height = textureMaxY - textureMinY;
			for (int i = 0; i < vertices.Length; i++)
			{
				Vector2 vertice = vertices[i];
				float x = (vertice.x - textureMinX) / width;
				float y = (vertice.y - textureMinY) / height;
				meshUv[i] = new Vector2(x, y);

				meshNormals[i] = new Vector3(0, 0, -1);

				meshVertices[i] = vertice;
			}

			int[] meshTriangles = new int[(meshVertices.Length - 2) * 3];
			for (int i = 0; i < meshVertices.Length - 2; i++)
			{
				int startIndex = i * 3;
				meshTriangles[startIndex] = 0;
				meshTriangles[startIndex + 1] = i + 1;
				meshTriangles[startIndex + 2] = i + 2;
			}
			
			mesh.vertices = meshVertices;
			mesh.uv = meshUv;
			mesh.normals = meshNormals;
			mesh.triangles = meshTriangles;
			return partObj;
		}

		void DebugShowPoint(Vector2[] points){
			StartCoroutine(CreatePointGo(points));
		}

		IEnumerator CreatePointGo(Vector2[] points){
			for (int i = 0; i < points.Length; i++)
			{
				CreateSign(points[i]);
				yield return new WaitForSeconds(0.5f);
			}
		}

		IEnumerator ShowPoints(Dictionary<string, List<Vector2>> posOfPoints){
			foreach (var item in posOfPoints)
			{
				Debug.LogFormat("Points Pos : {0}, count : {1}", item.Key, item.Value.Count);
				for (int i = 0; i < item.Value.Count; i++)
				{
					yield return new WaitForSeconds(0.5f);
					CreateSign(item.Value[i], string.Format("{0}_{1}", item.Key, i));
				}
			}
		}

		void CreateSign(Vector2 pos, string name = null){
			// GameObject o = GameObject.Instantiate(go);
			// o.transform.localPosition = pos;
			// o.transform.localScale = Vector3.one * 0.05f;
			// if (name != null){
			// 	o.name = name;
			// }
		}
	}
}
