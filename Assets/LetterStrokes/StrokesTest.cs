using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StrokesTest : MonoBehaviour {
	public RectTransform rt;
	public Camera rtCamera;

    void Start()
    {
        // rt = GetComponent<RectTransform>();
        DisplayWorldCorners();
    }

    void DisplayWorldCorners()
    {
        Vector3[] v = new Vector3[4];
        rt.GetWorldCorners(v);
		Vector3 pos = (v[0] + v[2]) * 0.5f;
		rtCamera.transform.position = new Vector3(pos.x, pos.y, -10);
		rtCamera.orthographicSize = Mathf.Abs((v[1] - v[0]).y) * 0.5f;
    }
}
