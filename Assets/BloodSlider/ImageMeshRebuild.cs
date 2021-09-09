
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(RebuildData))]
public class ImageMeshRebuild : Image
{
    RebuildData _rebuildData;

    UIVertex _vertex3;
    UIVertex _vertex2;
    UIVertex _vertex1;
    protected override void Awake()
    {
        base.Awake();
        _rebuildData = GetComponent<RebuildData>();
        _vertex3 =new UIVertex();
        _vertex2 =new UIVertex();
        _vertex1 =new UIVertex();
    }
    protected override void OnPopulateMesh(VertexHelper toFill)
    {
        base.OnPopulateMesh(toFill);
        if (type != Type.Filled || fillOrigin != 0 || fillMethod != FillMethod.Horizontal) return;
        if (!_rebuildData)
            _rebuildData = GetComponent<RebuildData>();
        if(toFill.currentIndexCount <= 3)
            return;
        toFill.PopulateUIVertex(ref _vertex3,3);
        toFill.PopulateUIVertex(ref _vertex2,2);
        toFill.PopulateUIVertex(ref _vertex1,1);

        float inputAdjustX = _rebuildData.VertexAdjustDatas[0].PosAdjust.x;
        float inputAdjusty = _rebuildData.VertexAdjustDatas[0].PosAdjust.y;

        float length = Mathf.Abs(_vertex1.position.x - _vertex3.position.x);
        float height = Mathf.Abs(_vertex1.position.y - _vertex3.position.y);
        float percentl = inputAdjustX / length;
        float percenth = inputAdjusty / height;
        float uvlength = Mathf.Abs(_vertex1.uv0.x - _vertex3.uv0.x);
        float uvHeight = Mathf.Abs(_vertex1.uv0.y - _vertex3.uv0.y);
        float uChange = percentl * uvlength;
        float vChange = percenth * uvHeight;

        _vertex2.position.x = _vertex2.position.x - inputAdjustX;
        _vertex2.uv0.x = _vertex2.uv0.x - uChange;
        toFill.SetUIVertex(_vertex2, 2);

        _vertex3.position.x = _vertex3.position.x - inputAdjustX;
        _vertex3.uv0.x = _vertex3.uv0.x - uChange;
        toFill.SetUIVertex(_vertex3, 3);

        float adjX = inputAdjusty * inputAdjustX / (height - inputAdjusty);
        float percent2 = adjX / length;
        float uChange2 = percent2 * uvlength;

        if (_rebuildData.tiltLeft)
        {
            _vertex3.position.x = _vertex3.position.x + inputAdjustX;
            _vertex3.uv0.x = _vertex3.uv0.x + uChange;
            toFill.AddVert(_vertex3);

            _vertex3.position.y = _vertex3.position.y + inputAdjusty;
            _vertex3.uv0.y = _vertex3.uv0.y + vChange;
            toFill.AddVert(_vertex3);

            _vertex3.position.y = _vertex3.position.y - inputAdjusty;
            _vertex3.position.x = _vertex3.position.x + adjX;
            _vertex3.uv0.x = _vertex3.uv0.x + uChange2;
            _vertex3.uv0.y = _vertex3.uv0.y - vChange;
            toFill.AddVert(_vertex3);

            toFill.AddTriangle(3, 2, 4);
            toFill.AddTriangle(4, 2, 5);
            toFill.AddTriangle(4, 5, 6);
        }
        else
        {
            _vertex2.position.x = _vertex2.position.x + inputAdjustX;
            _vertex2.uv0.x = _vertex2.uv0.x + uChange;
            toFill.AddVert(_vertex2);

            _vertex2.position.y = _vertex2.position.y - inputAdjusty;
            _vertex2.uv0.y = _vertex2.uv0.y - vChange;
            toFill.AddVert(_vertex2);

            _vertex2.position.y = _vertex2.position.y + inputAdjusty;
            _vertex2.position.x = _vertex2.position.x + adjX;
            _vertex2.uv0.x = _vertex2.uv0.x + uChange2;
            _vertex2.uv0.y = _vertex2.uv0.y + vChange;
            toFill.AddVert(_vertex2);

            toFill.AddTriangle(3, 2, 4);
            toFill.AddTriangle(5, 3, 4);
            toFill.AddTriangle(5, 4, 6);
        }
            
    }
}
