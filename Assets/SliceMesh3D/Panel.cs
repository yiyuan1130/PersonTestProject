using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Panel {
	public Vector3 normal;
	public Vector3 point;
	public Panel(Vector3 normal, Vector3 point){
		this.normal = normal;
		this.point = point;
	}

	public float DistanceToPoint(Vector3 worldPos){
		Vector3 posToCenterVec = worldPos - this.point;
		float cosVal = Vector3.Dot(posToCenterVec, this.normal) / (posToCenterVec.magnitude * this.normal.magnitude);
		return posToCenterVec.magnitude * cosVal;
	}

	public bool HavePointWithLine(Vector3[] line){
		float v = DistanceToPoint(line[0]) * DistanceToPoint(line[1]);
		return v < 0;
	}

	public Vector3? PointOfLine(Vector3[] line){
		if (!HavePointWithLine(line)){
			return null;
		}
		Vector3 direct = (line[1] - line[0]).normalized;
        float d = Vector3.Dot(this.point - line[0], this.normal) / Vector3.Dot(direct, this.normal);
        return d * direct.normalized + line[0];
	}

	public bool IsTrianglesBeSliced(Vector3[] triangle){
		float dis0 = this.DistanceToPoint(triangle[0]);
		float dis1 = this.DistanceToPoint(triangle[1]);
		float dis2 = this.DistanceToPoint(triangle[2]);
		return !((dis0 < 0 && dis1 < 0 && dis2 < 0) || (dis0 > 0 && dis1 > 0 && dis2 > 0));
	}
}
