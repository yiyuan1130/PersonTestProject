using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CellSpacePartition
{
    public class Agent : MonoBehaviour
    {
        public Cell beloneCell;
        public Vector3 moveDir;
        public float speed;
        List<Agent> neighbors;
        public float radius;
        bool isMove = false;
        Vector3 centerPos;
        public float separationRate = 10f;
        public float cohesionRate = 15f;
        public float viewRadius = 20f;
        // FOV
        void Start()
        {
            transform.Find("Cylinder").localScale = new Vector3(radius, 0.2f, radius);
        }

        void Update()
        {
            if (Input.GetMouseButtonDown(1)) {
                isMove = true;
            }
            if (Input.GetMouseButtonUp(1))
            {
                isMove = false;
            }
            if (!isMove) {
                return;
            }
            MoveTarget();
            GetNeighors();
            if (neighbors != null && neighbors.Count > 0)
            {
                Vector3 v1 = Separation();
                Vector3 v2 = Alignment();
                Vector3 v3 = Cohesion();
                moveDir = (v1 + v2 + v3 + moveDir) / 4;
            }
            transform.forward = moveDir;
            transform.position += moveDir * speed * Time.deltaTime;
            SpacePartition.UpdateAgentCell(this);

        }

        void MoveTarget() {
            if (Input.GetMouseButton(1))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit)) { 
                    Vector3 tarPos = new Vector3(hit.point.x, 0, hit.point.z);
                    moveDir = (tarPos - transform.position).normalized;
                }
            }
        }

        Vector3 Separation() {
            Vector3 ret = Vector3.zero;
            int count = 0;
            Vector3 tarDir = Vector3.zero;
            for (int i = 0; i < neighbors.Count; i++)
            {
                Agent agent = neighbors[i];
                Vector3 vec = agent.transform.position - transform.position;
                if (vec.magnitude <= separationRate)
                {
                    count++;
                    tarDir += vec.normalized;
                }
            }
            ret = Vector3.Lerp(moveDir, tarDir.normalized, 0.5f);
            return ret;
        }
        Vector3 Alignment() {
            Vector3 ret = Vector3.zero;
            int count = neighbors.Count;
            Vector3 sumMoveDir = Vector3.zero;
            for (int i = 0; i < count; i++)
            {
                sumMoveDir += neighbors[i].moveDir;
            }
            Vector3 tarDir = sumMoveDir / count;
            ret = Vector3.Lerp(moveDir, tarDir, 0.5f);
            return ret;
        }
        Vector3 Cohesion() {
            Vector3 ret = Vector3.zero;
            int count = neighbors.Count;
            //for (int i = 0; i < count; i++)
            //{
            //    Agent agent = neighbors[i];
            //    }
            //}
            Vector3 vec = transform.position - centerPos;
            if (vec.magnitude > cohesionRate)
            {
                Vector3 tarDir = (moveDir + vec.normalized).normalized;
                ret = Vector3.Lerp(moveDir, tarDir, 0.5f);
            }
            return ret;
        }

        void GetNeighors() {
            if (beloneCell != null) { 
                List<Agent> agents = beloneCell.agentList;
                if (neighbors == null)
                {
                    neighbors = new List<Agent>();
                }
                else {
                    neighbors.Clear();
                }
                for (int i = 0; i < agents.Count; i++)
                {
                    Agent agent = agents[i];
                    if (agent == this) {
                        continue;
                    }
                    if (Vector3.Distance(agent.transform.position, transform.position) <= viewRadius) {
                        neighbors.Add(agent);
                    }
                }
                Vector3 pos = Vector3.zero;
                for (int i = 0; i < neighbors.Count; i++)
                {
                    pos += neighbors[i].transform.position;
                }
                centerPos = pos / neighbors.Count;
            }
        }

    }
}
