using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CellSpacePartition
{
    public class Cell {
        public int id;
        public Bounds bounds;
        public List<Agent> agentList;
        public Cell(int id, Vector3 center, Vector3 size) {
            this.id = id;
            this.agentList = new List<Agent>();
            this.bounds = new Bounds(center, size);
        }
        public void RemoveAgent(Agent agent) {
            if (agentList.Contains(agent)) {
                agentList.Remove(agent);
            }
        }
        public void AddAgent(Agent agent) {
            if (!agentList.Contains(agent))
            {
                agentList.Add(agent);
            }
        }
    }
    public static class SpacePartition
    {
        static float spaceX;
        static float spaceZ;
        static int cellCountX;
        static int cellCountZ;
        static List<Cell> cellList;
        static Vector3 cellSize;
        static Vector3 cellStartPos;
        public static void Init(float spaceX, float spaceZ, Vector3 cellSize) {
            cellList = new List<Cell>();
            SpacePartition.spaceX = spaceX;
            SpacePartition.spaceZ = spaceZ;
            SpacePartition.cellSize = cellSize;
            GenerateCell();
        }

        static void GenerateCell() {
            cellCountX = (int)(spaceX / cellSize.x);
            cellCountZ = (int)(spaceZ / cellSize.z);
            cellStartPos = new Vector3(-spaceX * 0.5f + cellSize.x * 0.5f, 0, -spaceZ * 0.5f + cellSize.z * 0.5f);
            int index = 0;
            for (int i = 0; i < cellCountX; i++)
            {
                for (int j = 0; j < cellCountZ; j++)
                {
                    Vector3 center = cellStartPos + new Vector3(cellSize.x * i, 0, cellSize.z * j);
                    Cell cell = new Cell(index, center, cellSize);
                    cellList.Add(cell);
                }
            }
        }

        static int PositionIntoIndex(Vector3 pos) {
            float dx = pos.x - (spaceX * -1 * 0.5f);
            float dz = pos.z - (spaceZ * -1 * 0.5f);
            int x = (int)(dx / cellSize.x);
            int z = (int)(dz / cellSize.z);
            return x * cellCountX + z;
        }

        public static void UpdateAgentCell(Agent agent) {
            int index = PositionIntoIndex(agent.transform.position);
            if (index >= cellList.Count) {
                return;
            }
            Cell tarCell = cellList[index];
            if (agent.beloneCell == null)
            {
                agent.beloneCell = tarCell;
                tarCell.AddAgent(agent);
            }
            else {
                if (agent.beloneCell != tarCell) {
                    agent.beloneCell.RemoveAgent(agent);
                    agent.beloneCell = tarCell;
                    tarCell.AddAgent(agent);
                }
            }
        }

        public static List<Cell> GetCellList() {
            return cellList;
        }
    }

}
