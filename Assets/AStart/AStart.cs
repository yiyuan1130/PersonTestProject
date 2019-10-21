using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Point{
	public Vector3 vec;
	public int H;
	public int G;
	public int F;

	public Point(Vector3 vec, int H, int G){
		this.vec = vec;
		this.H = H;
		this.G = G;
		this.F = this.H + this.G;
	}
}

public class AStart : MonoBehaviour {
	public Vector3 startPos;
	public Vector3 targetPos;

	int italic = 14;
	int standard = 10;

	List<Vector3> openList = new List<Vector3>();
	List<Vector3> closeList = new List<Vector3>();
	List<Vector3> resultList = new List<Vector3>();

	void Init(){
		openList.Add(startPos);
	}

	Point NewPoint(Vector3 currentPos, Point lastPos){
		int h = (int)Mathf.Abs(currentPos.x - targetPos.x) + (int)Mathf.Abs(currentPos.y - targetPos.y);
		int g = 0;
		if (currentPos.x == lastPos.vec.x || currentPos.y == lastPos.vec.y){
			g = lastPos.G + 10;
		}else{
			g = lastPos.G + 14;
		}
		Point point = new Point(currentPos, h, g);
		return point;
	}

	List<Point> GetAroundPoint(Vector3 pos){
		List<Vector3> posList = new List<Vector3>();
		posList.Add(pos + new Vector3(-1, 1, 0));
		posList.Add(pos + new Vector3(-1, -1, 0));
		posList.Add(pos + new Vector3(0, 1, 0));
		posList.Add(pos + new Vector3(1, 1, 0));
		posList.Add(pos + new Vector3(1, 0, 0)); 
		posList.Add(pos + new Vector3(1, -1, 0));
		posList.Add(pos + new Vector3(0, -1, 0));
		posList.Add(pos + new Vector3(-1, -1, 0));

		List<Point> points = new List<Point>();
		for (int i = 0; i < posList.Count; i++)
		{
			// points.Add(NewPoint());
		}
		// return posList;
		return null;
	}
}
