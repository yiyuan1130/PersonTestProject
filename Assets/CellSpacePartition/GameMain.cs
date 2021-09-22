using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CellSpacePartition
{
    public class GameMain : MonoBehaviour
    {
        public GameObject agentPrefab;
        public Vector2 cellSize;
        float spaceX;
        float spaceZ;
        private void Awake()
        {
            Transform plane = GameObject.Find("Plane").transform;
            spaceX = plane.localScale.x * 10;
            spaceZ = plane.localScale.z * 10;
            SpacePartition.Init(spaceX, spaceZ, new Vector3(cellSize.x, 10, cellSize.y));
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0)) {
                CreateAgent();
            }
        }

        void CreateAgent() {
            GameObject agentGO = Instantiate(agentPrefab);
            agentGO.SetActive(true);
            agentGO.transform.position = new Vector3(Random.Range(-spaceX, spaceX) * 0.5f, 0, Random.Range(-spaceZ, spaceZ) * 0.5f);
            Agent agent = agentGO.GetComponent<Agent>();
        }

        private void OnDrawGizmos()
        {
            List<Cell> cellList = SpacePartition.GetCellList();
            if (cellList == null) {
                return;
            }
            for (int i = 0; i < cellList.Count; i++)
            {
                Cell cell = cellList[i];
                if (cell.agentList.Count > 0)
                {
                    Gizmos.color = new Color(0, 1f, 0, 0.2f);
                    Gizmos.DrawCube(cell.bounds.center, cell.bounds.size);
                }
                else {
                    Gizmos.color = Color.white;
                    Gizmos.DrawWireCube(cell.bounds.center, cell.bounds.size);
                }
            }
        }


    }
}
