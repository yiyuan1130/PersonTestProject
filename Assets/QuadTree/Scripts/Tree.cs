using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace QuadTree { 
    public class Tree
    {
        public Bounds bounds;
        public int maxDepth;
        public int maxChildCounut { get; }
        private Node root;

        public Tree(Bounds bounds, int maxDepth) {
            this.bounds = bounds;
            this.maxDepth = maxDepth;
            this.maxChildCounut = 4;
            root = new Node(this.bounds, 0, this);
        }
        public void InsertObj(ObjData obj) {
            root.InsertObj(obj);
        }

        public void TriggerMove(Camera camera) {
            root.TriggerMove(camera);
        }

        public void DrawBound() {
            root.DrawBound();
        }

        public void CreateChild() {
            root.CreateChild();
        }
    }
}
