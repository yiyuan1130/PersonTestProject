using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AStarCSSharp
{
    public class ActorMove : MonoBehaviour
    {
        public GameObject linePrefab;
        GameObject line;
        public float speed = 30f;
        Vector3 moveDir;
        List<Vector3> pathList;
        bool isMoving = false;
        Vector3 startPos;
        Vector3 endPos;
        Vector3 tarPos;
        int curIdx = 0;
        public float radius = 10f;
        bool crashed = false;
        AStarFind aStarFind;
        public void DoMove(AStarFind aStarFind, Vector3 startPos, Vector3 endPos)
        {
            GenerateCollider();
            this.endPos = endPos;
            this.startPos = startPos;
            transform.position = this.startPos;
            pathList = new List<Vector3>();
            this.aStarFind = aStarFind;
            pathList = aStarFind.GetPath(startPos, endPos);
            if (pathList != null && pathList.Count > 0)
            {
                pathList.Insert(0, startPos);
                pathList.Add(endPos);
            }
            //DrawPathLine();
            isMoving = true;
        }

        void GenerateCollider()
        {
            Transform collider = transform.Find("collider");
            collider.localScale = new Vector3(radius, 0.1f, radius);
            collider.localPosition = new Vector3(0, 0.45f, 0);
        }

        void DrawPathLine()
        {
            line = Instantiate(linePrefab);
            LineRenderer lineRenderer = line.GetComponent<LineRenderer>();
            lineRenderer.startWidth = 0.5f;
            lineRenderer.endWidth = 0.5f;
            Vector3[] linePosArr = new Vector3[pathList.Count];
            for (int i = 0; i < pathList.Count; i++)
            {
                linePosArr[i] = new Vector3(pathList[i].x, 0.5f, pathList[i].z);
            }
            lineRenderer.positionCount = linePosArr.Length;
            lineRenderer.SetPositions(linePosArr);
        }

        void Update()
        {
            if (isMoving)
            {
                if (pathList != null && pathList.Count > 0 && curIdx < pathList.Count)
                {
                    tarPos = pathList[curIdx];
                    moveDir = (tarPos - transform.position).normalized;
                    //if (crashed) {
                    //    this.moveDir = ((transform.forward + moveDir + (endPos - this.transform.position).normalized) * 0.33f).normalized;
                    //    pathList = aStarFind.GetPath(this.transform.position, endPos);
                    //    curIdx = 0;
                    //}
                    transform.forward = moveDir;
                    transform.position += transform.forward * speed * Time.deltaTime;
                    float dis = Vector3.Distance(transform.position, tarPos);
                    if (dis <= 0.05f)
                    {
                        curIdx++;
                    }
                }
                else
                {
                    isMoving = false;
                    ScallSmallDestroy();
                }
            }

            if (isScallSmall)
            {
                curSmallDur += Time.deltaTime;
                float rate = curSmallDur / smallDur;
                transform.localScale = Vector3.one * Mathf.Lerp(1, 0, rate);
                if (rate > 1)
                {
                    ActorManager.RemoveActorMove(this);
                    GameObject.Destroy(line);
                    GameObject.Destroy(this.gameObject);
                    isScallSmall = false;
                }
            }

            //List<ActorMove> actorMoveList = ActorManager.GetActorMoveList();
            //for (int i = 0; i < actorMoveList.Count; i++)
            //{
            //    ActorMove actorMove = actorMoveList[i];
            //    if (actorMove != this) {
            //        float dx = actorMove.transform.position.x - this.transform.position.x;
            //        float dz = actorMove.transform.position.z - this.transform.position.z;
            //        float dr = actorMove.radius + this.radius;
            //        if (dx * dx + dz * dz < dr * dr)
            //        {
            //            crashed = true;
            //        }
            //        else {
            //            crashed = false;
            //        }
            //    }
            //}
        }

        bool isScallSmall = false;
        float smallDur = 0.5f;
        float curSmallDur = 0f;
        void ScallSmallDestroy()
        {
            isScallSmall = true;
            curSmallDur = 0f;
        }

        private void OnDrawGizmos()
        {
            if (pathList != null)
            {
                for (int i = curIdx; i < pathList.Count; i++)
                {
                    Gizmos.color = Color.green;
                    Gizmos.DrawSphere(pathList[i], 0.5f);
                }
            }
            Gizmos.color = Color.blue;
            Gizmos.DrawSphere(startPos, 0.5f);
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(endPos, 0.5f);
        }
    }
}
