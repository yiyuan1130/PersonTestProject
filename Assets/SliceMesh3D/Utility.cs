using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utility {
	public static Vector3 MousePostionToWorld(Vector3 mousePos, Transform targetTransform)
    {
        Vector3 dir = targetTransform.position - Camera.main.transform.position;
        Vector3 normardir = Vector3.Project(dir, Camera.main.transform.forward);
        return Camera.main.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, normardir.magnitude));
    }

	public static Vector3 ObjectToWorldPoint(Vector3 objectPoint, Transform transform){
		return transform.TransformPoint(objectPoint);
	}

	public static Vector3 WorldToObjectPoint(Vector3 worldPoint, Transform transform){
		return transform.InverseTransformPoint(worldPoint);
	}

}
