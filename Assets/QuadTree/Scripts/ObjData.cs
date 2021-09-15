using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace QuadTree { 
    public class ObjData
    {
        public GameObject gameObject;
        public Vector3 position;
        public Quaternion rotation;
        public ObjData(GameObject gameObject) {
            this.gameObject = gameObject;
            position = this.gameObject.transform.position;
            rotation = this.gameObject.transform.rotation;
        }
    }
}
