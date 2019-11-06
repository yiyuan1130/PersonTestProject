using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MiaoKids {

	/*
		图片切割解决方案 20191106 @EamonnLi

		1. 通过 PolygonCollider 获取图像顶点坐标 vertices
		2. 用过 vertices 获取图像外围线段 lines
		3. 计算画的线 drawLine 和 lines 的所有交点 points
	 */
	public class SliceSprite : MonoBehaviour {

		public GameObject go;

		Vector3 startPos;

		Vector3 endPos;

		LineRenderer lr;
		Vector3[] points;
		bool drawingLine;
		bool endLine;
		void Awake() {
			lr = new GameObject("line").AddComponent<LineRenderer>();
		}

		void Start(){
			// GetVerticesFromPolygonCollider();
		}

		void Update(){
			if (Input.GetMouseButtonDown(0)){
				startPos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10));
				drawingLine = true;
			}
			if (Input.GetMouseButtonUp(0)){
				endPos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10));
				drawingLine = false;
				DoSlice();
			}
			endPos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10));
			DrawLine();

		}

		void DrawLine(){
			if (!drawingLine || startPos == null || endPos == null)
				return;
			points = new Vector3[2]{startPos, endPos};		
			lr.startWidth = 0.05f; 
			lr.endWidth = 0.05f;
			lr.useWorldSpace = false;
			lr.positionCount = points.Length;
			lr.SetPositions (points);
		}

		void DoSlice(){
			Vector2[] vertices = GetVerticesFromPolygonCollider();
			List<Line> lines = GetLinesFromVertices(vertices);
			List<Vector2> points = CaculateIntersectionPoints(lines);
			DebugShowPoint(points.ToArray());
		}

		// 通过Collider获取顶点坐标
		Vector2[] GetVerticesFromPolygonCollider(){
			SpriteRenderer sr = gameObject.GetComponent<SpriteRenderer>();
			Sprite sprite = sr.sprite;
			PolygonCollider2D collider = gameObject.GetComponent<PolygonCollider2D>();
			Vector2[] vertices = collider.points;
			return vertices;
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
		
		void CreateMesh(Vector2[] spriteVertices, Vector2[] intersectionVertices){

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

		void CreateSign(Vector2 pos){
			GameObject o = GameObject.Instantiate(go);
			o.transform.localPosition = pos;
			o.transform.localScale = Vector3.one * 0.05f;
		}
	}
}
