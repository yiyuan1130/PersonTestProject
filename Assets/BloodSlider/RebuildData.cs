using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct VertexAdjustData
{
    public Vector2 PosAdjust;
};
public class RebuildData : MonoBehaviour
{
    public VertexAdjustData[] VertexAdjustDatas ;
    public bool tiltLeft;
}
