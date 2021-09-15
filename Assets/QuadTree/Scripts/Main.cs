using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace QuadTree {
    public class Main : MonoBehaviour
    {
        public GameObject testObj;
        public int maxObjectCount;
        public Transform plane;
        public float mapRange;
        public int maxDepth;
        private List<GameObject> testObjects;
        private Tree tree;
        void Start()
        {
            plane.localScale = Vector3.one * mapRange / 5;
            RandomCreateObjects();
            GenerateQuadTree();
        }

        void GenerateQuadTree() {
            Bounds bounds = new Bounds(Vector3.zero, new Vector3(mapRange * 2, 0, mapRange * 2));
            tree = new Tree(bounds, maxDepth);
            //tree.CreateChild();
            for (int i = 0; i < testObjects.Count; i++)
            {
                tree.InsertObj(new ObjData(testObjects[i]));
            }

        }

        void OnDrawGizmos()
        {
            if (tree != null)
            {
                tree.DrawBound();
            }
        }

        private void Update()
        {
            if (tree != null) {
                tree.TriggerMove(Camera.main);
            }
        }

        void RandomCreateObjects() {
            testObjects = new List<GameObject>();
            for (int i = 0; i < maxObjectCount; i++)
            {
                GameObject go = GameObject.Instantiate(testObj);
                go.transform.localScale = new Vector3(Random.Range(0.1f, 0.9f), Random.Range(0.1f, 0.9f) * 3, Random.Range(0.1f, 0.9f));
                go.transform.position = new Vector3(Random.Range(-mapRange, mapRange), 0, Random.Range(-mapRange, mapRange));
                testObjects.Add(go);
                //go.SetActive(false);
            }
        }
    }
}
