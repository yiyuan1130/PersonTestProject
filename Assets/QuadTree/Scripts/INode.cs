using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace QuadTree {
    public interface INode {
        public Bounds bounds { get; set; }
        void InsertObj(ObjData objData);
        void TriggerMove(Camera camera);
        void DrawBound();
    }
}
