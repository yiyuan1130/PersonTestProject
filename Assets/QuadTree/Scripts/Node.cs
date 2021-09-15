using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace QuadTree
{
    public class Node
    {
        public Bounds bounds { get; set; }
        private int depth;
        private Tree beloneTree;
        private Node[] childs;
        private List<ObjData> objDataList;

        public Node(Bounds bounds, int depth, Tree beloneTree) {
            this.bounds = bounds;
            this.depth = depth;
            this.beloneTree = beloneTree;
            objDataList = new List<ObjData>();
        }

        public void InsertObj(ObjData objData) {
            if (depth < beloneTree.maxDepth && childs == null) {
                CreateChild();
            }
            bool objBeloneChild = false;
            if (childs != null) {
                for (int i = 0; i < childs.Length; i++)
                {
                    Node childNode = childs[i];
                    if (childNode != null) {
                        if (childNode.bounds.Contains(objData.position)) {
                            objBeloneChild = true;
                            childNode.InsertObj(objData);
                            break;
                        }
                    }
                }
            }
            if (!objBeloneChild){
                this.objDataList.Add(objData);
            }
        }

        public void TriggerMove(Camera camera) {
            bool showObject = false;
            if (childs != null) {
                for (int i = 0; i < childs.Length; i++)
                {
                    childs[i].TriggerMove(camera);
                }
            }
            else {
                CameraBound cameraBound = camera.gameObject.GetComponent<CameraBound>();
                if (cameraBound.bounds != null) {
                    if (this.bounds.Intersects(cameraBound.bounds)) {
                        showObject = true;
                    }
                }
            }
            for (int i = 0; i < this.objDataList.Count; i++)
            {
                objDataList[i].gameObject.SetActive(showObject);
            }
            isShowing = showObject;
        }

        public void CreateChild() {
            if (this.depth >= this.beloneTree.maxDepth) {
                return;
            }
            childs = new Node[4];
            Vector3 childSize = this.bounds.size * 0.5f;
            float dx = childSize.x * 0.5f;
            float dz = childSize.z * 0.5f;
            Vector3[] centers = new Vector3[] {
                this.bounds.center + new Vector3(-dx, 0, dz),
                this.bounds.center + new Vector3(dx, 0, dz),
                this.bounds.center + new Vector3(dx, 0, -dz),
                this.bounds.center + new Vector3(-dx, 0, -dz),
            };
            for (int i = 0; i < centers.Length; i++)
            {
                Node childNode = new Node(new Bounds(centers[i], childSize), depth + 1, this.beloneTree);
                childs[i] = childNode;
            }
        }

        bool isShowing = false;
        public void DrawBound() {
            if (childs != null) { 
                for (int i = 0; i < childs.Length; i++)
                {
                    childs[i].DrawBound();
                }
            }
            if (this.objDataList.Count > 0 || this.depth < this.beloneTree.maxDepth)
            {
                Gizmos.color = Color.blue;
                if (isShowing) {
                    Gizmos.color = Color.red;
                }
                Gizmos.DrawWireCube(this.bounds.center, this.bounds.size);
            }
        }

    }
}
