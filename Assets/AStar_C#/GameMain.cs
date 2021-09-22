using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace AStarCSSharp
{
    public class GameMain : MonoBehaviour
    {
        Grid[][] grids;
        Bounds[] obs;
        AStarFind aStarFind;
        GameObject actorPrefab;
        public float gridSizeRate = 1f;
        bool selectStart = false;
        private void Awake()
        {
            ActorManager.Init();
            actorPrefab = GameObject.Find("Actor");
            actorPrefab.SetActive(false);
            CreateOrUpdateAStartFind();
        }

        Vector3 clickStartPos;
        Vector3 clickEndPos;
        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit))
                {
                    if (PosIsInOB(hit.point))
                    {
                        return;
                    }
                    if (!selectStart)
                    {
                        clickStartPos = hit.point;
                        selectStart = true;
                    }
                    else
                    {
                        clickEndPos = hit.point;
                        selectStart = false;
                        AddMoveActor(clickStartPos, clickEndPos);
                        AddMoveActor(clickEndPos, clickStartPos);
                    }
                }
            }
        }

        public void AddMoveActor(Vector3 startPos, Vector3 endPos)
        {
            GameObject actor = Instantiate(actorPrefab);
            actor.SetActive(true);
            ActorMove actorMove = actor.GetComponent<ActorMove>();
            ActorManager.CreateActorMove(actorMove);
            actorMove.DoMove(aStarFind, startPos, endPos);
        }

        void CreateOrUpdateAStartFind()
        {
            GenOBs();
            GenPlane();
            if (aStarFind == null)
            {
                aStarFind = new AStarFind(grids);
            }
            else
            {
                aStarFind.UpdateGrids(grids);
            }
        }


        void GenOBs()
        {
            Transform obsTrans = GameObject.Find("OBs").transform;
            obs = new Bounds[obsTrans.childCount];
            for (int i = 0; i < obsTrans.childCount; i++)
            {
                Transform obTrans = obsTrans.GetChild(i);
                Vector3 center = new Vector3(obTrans.position.x, 0, obTrans.position.z);
                Bounds bounds = new Bounds(center, obTrans.localScale);
                obs[i] = bounds;
            }
        }
        bool PosIsInOB(Vector3 pos)
        {
            for (int i = 0; i < obs.Length; i++)
            {
                if (obs[i].Contains(pos))
                {
                    return true;
                }
            }
            return false;
        }
        bool GridIsInOB(Grid grid)
        {
            for (int i = 0; i < obs.Length; i++)
            {
                if (obs[i].Intersects(grid.bounds))
                {
                    return true;
                }
            }
            return false;
        }

        void GenPlane()
        {
            Transform planeTrans = GameObject.Find("Plane").transform;
            Vector3 mapSize = new Vector3(planeTrans.transform.localScale.x, 0, planeTrans.transform.localScale.z);
            Vector3 gridSize = Vector3.one * gridSizeRate;
            int gridCountX = (int)(mapSize.x / gridSize.x);
            int gridCountZ = (int)(mapSize.z / gridSize.z);
            Vector3 gridStartPos = new Vector3(-mapSize.x / 2 + gridSize.x / 2, 0, -mapSize.z / 2 + gridSize.z / 2);

            grids = new Grid[gridCountX][];
            for (int i = 0; i < gridCountX; i++)
            {
                grids[i] = new Grid[gridCountZ];
                for (int j = 0; j < gridCountZ; j++)
                {
                    Vector3 pos = gridStartPos + new Vector3(gridSize.x * i, 0, gridSize.z * j);
                    Grid grid = new Grid(pos, gridSize);
                    grid.walkAble = !GridIsInOB(grid);
                    grid.i = i;
                    grid.j = j;
                    grids[i][j] = grid;
                }
            }

        }

        private void OnDrawGizmos()
        {
            if (grids == null)
            {
                return;
            }
            for (int i = 0; i < grids.Length; i++)
            {
                for (int j = 0; j < grids[i].Length; j++)
                {
                    Grid grid = grids[i][j];
                    if (grid.walkAble)
                    {
                        Gizmos.color = Color.blue;
                        Gizmos.DrawWireCube(grid.bounds.center, grid.bounds.size);
                    }
                }
            }

        }
    }
}