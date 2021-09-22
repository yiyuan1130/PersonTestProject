using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace AStarCSSharp
{
    public class Grid
    {
        public Grid preGrid;
        public Bounds bounds;
        public bool walkAble = false;
        public int G;
        public int H;
        public int F;
        public int i;
        public int j;
        public Grid(Vector3 pos, Vector3 size, bool walkAble = true)
        {
            this.preGrid = null;
            this.walkAble = walkAble;
            this.bounds = new Bounds(pos, size);
            this.G = 0;
            this.H = 0;
            this.F = 99999;
        }

        public void Reset()
        {
            this.preGrid = null;
            this.G = 0;
            this.H = 0;
            this.F = 99999;
        }
    }

    public class AStarFind
    {
        Grid[][] grids;
        Grid startGrid;
        Grid endGrid;
        Grid curGrid;
        List<Grid> openList;
        List<Grid> closeList;
        public List<Vector3> pathList;
        int maxI;
        int maxJ;
        public AStarFind(Grid[][] grids)
        {
            this.grids = grids;
            this.maxI = grids.Length - 1;
            this.maxJ = grids[0].Length - 1;
        }

        public void UpdateGrids(Grid[][] grids)
        {
            this.grids = grids;
            this.maxI = grids.Length - 1;
            this.maxJ = grids[0].Length - 1;
        }

        public List<Vector3> GetPath(Vector3 startPos, Vector3 endPos)
        {
            for (int i = 0; i < grids.Length; i++)
            {
                for (int j = 0; j < grids[0].Length; j++)
                {
                    grids[i][j].Reset();
                }
            }

            bool posValid = GetStartEndGridByPos(startPos, endPos);
            if (!posValid)
                return null;

            Find();

            pathList = new List<Vector3>();
            Grid grid = endGrid;
            int count = 0;
            while (grid != null && grid != startGrid)// && count <= 20000)
            {
                pathList.Add(grid.bounds.center);
                grid = grid.preGrid;
                count++;
            }
            pathList.Reverse();
            return pathList;
        }

        bool GetStartEndGridByPos(Vector3 startPos, Vector3 endPos)
        {
            bool startPosValid = false;
            bool endPosValid = false;
            Grid grid;
            for (int i = 0; i < grids.Length; i++)
            {
                for (int j = 0; j < grids[i].Length; j++)
                {
                    grid = grids[i][j];
                    if (grid.walkAble)
                    {
                        if (grid.bounds.Contains(startPos))
                        {
                            startGrid = grid;
                            startPosValid = true;
                        }
                        if (grid.bounds.Contains(endPos))
                        {
                            endGrid = grid;
                            endPosValid = true;
                        }
                    }
                }
            }
            if (startGrid == null)
            {
                Debug.LogError("StartPos is INVALID");
            }
            if (endGrid == null)
            {
                Debug.LogError("EndPos is INVALID");
            }
            return startPosValid && endPosValid;
        }

        void Find()
        {
            openList = new List<Grid>();
            closeList = new List<Grid>();
            startGrid.preGrid = null;
            startGrid.G = 0;
            startGrid.H = Mathf.Abs(endGrid.i - startGrid.i) + Mathf.Abs(endGrid.j - startGrid.j);
            startGrid.F = startGrid.G + startGrid.H;
            openList.Add(startGrid);
            curGrid = startGrid;
            int count = 0;
            while (openList.Count > 0 && curGrid != endGrid)// && count <= 20000)
            {
                SortOpenList();
                curGrid = openList[0];
                openList.RemoveAt(0);
                closeList.Add(curGrid);
                GetAroundGrid();
                count++;
            }
        }

        void SortOpenList()
        {
            openList.Sort((a, b) =>
            {
                if (a.F < b.F)
                {
                    return -1;
                }
                else
                {
                    return 1;
                }
            });
        }

        void GetAroundGrid()
        {
            Vector2Int[] idOffsetArr = new Vector2Int[] {
            new Vector2Int(0, 1),
            //new Vector2Int(1, 1),
            new Vector2Int(1, 0),
            //new Vector2Int(1, -1),
            new Vector2Int(0, -1),
            //new Vector2Int(-1, -1),
            new Vector2Int(-1, 0),
            //new Vector2Int(-1, 1),
        };

            for (int i = 0; i < idOffsetArr.Length; i++)
            {
                Vector2Int idOffset = idOffsetArr[i];
                int tarI = curGrid.i + idOffset.x;
                int tarJ = curGrid.j + idOffset.y;
                if (tarI >= 0 && tarI <= maxI && tarJ >= 0 && tarJ <= maxJ)
                {
                    Grid grid = grids[tarI][tarJ];
                    if (!grid.walkAble)
                    {
                        closeList.Add(grid);
                    }
                    else
                    {
                        if (!closeList.Contains(grid))
                        {
                            if (grid != null)
                            {
                                openList.Add(grid);
                                int g = curGrid.G + 1;
                                int h = Mathf.Abs(endGrid.i - grid.i) + Mathf.Abs(endGrid.j - grid.j);
                                int f = g + h;
                                if (grid.F > f)
                                {
                                    grid.preGrid = curGrid;
                                    grid.G = g;
                                    grid.H = h;
                                    grid.F = f;
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}
