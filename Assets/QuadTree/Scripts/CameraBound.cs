using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace QuadTree { 
    public class CameraBound : MonoBehaviour
    {
        [HideInInspector]
        public Bounds bounds;
        public float speed = 1f;
        GameObject panel;
        void Start()
        {
            bounds = new Bounds(Vector3.zero, new Vector3(30, 0, 20));
        }

        // Update is called once per frame
        void Update()
        {
            Ray ray = new Ray(transform.position, transform.forward);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                if (bounds != null) { 
                    bounds.center = new Vector3(hit.point.x, 0, hit.point.z);
                }
            }

            if (Input.GetKey(KeyCode.W)) {
                transform.position += new Vector3(0, 0, 1) * Time.deltaTime * speed;
            }
            if (Input.GetKey(KeyCode.S))
            {
                transform.position += new Vector3(0, 0, -1) * Time.deltaTime * speed;
            }
            if (Input.GetKey(KeyCode.A))
            {
                transform.position += new Vector3(-1, 0, 0) * Time.deltaTime * speed;
            }
            if (Input.GetKey(KeyCode.D))
            {
                transform.position += new Vector3(1, 0, 0) * Time.deltaTime * speed;
            }
        }

        void OnDrawGizmos()
        {
            if (bounds != null) { 
                Gizmos.DrawWireCube(this.bounds.center, this.bounds.size);
            }
        }
    }

}
