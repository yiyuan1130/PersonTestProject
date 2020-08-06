using System.Collections.Generic;
using System.Collections;
using UnityEngine;

namespace AStart{
    public class AStarMain{
        Dictionary<string, Point> points;
        Dictionary<string, Point> wall;
        Point startPoint;
        Point endPoint;
        public AStarMain(Dictionary<string, Point> points, Dictionary<string, Point> wall, Point startPoint, Point endPoint){
            this.points = points;
            this.wall = wall;
            this.startPoint = startPoint;
            this.endPoint = endPoint;
        }

        List<Point> openList;
        List<Point> closeList;
        List<Point> pathPoints;
        Point currentPoint;

        public void StartFindPath(){
            
        }

        IEnumerator Caculate(){
            openList = new List<Point>();
            closeList = new List<Point>();
            pathPoints = new List<Point>();
            currentPoint = startPoint;
            // openList.Add(startPoint);
            closeList.Add(startPoint);
            startPoint.G = 0;
            int index = 0;
            WaitForSeconds wait = new WaitForSeconds(0.5f);
            while(!closeList.Contains(endPoint) && index < 1000){
                yield return wait;
                currentPoint = getNext(currentPoint);
                closeList.Add(currentPoint);
                index++;
            }
        }

        Point getNext(Point p){
            openList.Clear();
            string up = string.Format("{0}_{1}", currentPoint.pos.x, currentPoint.pos.y + 1);
            string right = string.Format("{0}_{1}", currentPoint.pos.x + 1, currentPoint.pos.y);
            string left = string.Format("{0}_{1}", currentPoint.pos.x, currentPoint.pos.y - 1);
            string down = string.Format("{0}_{1}", currentPoint.pos.x - 1, currentPoint.pos.y);
            if (points.ContainsKey(up))
                openList.Add(points[up]);
            if (points.ContainsKey(right))
                openList.Add(points[right]);
            if (points.ContainsKey(left))
                openList.Add(points[left]);
            if (points.ContainsKey(down))
                openList.Add(points[down]);
            foreach (var item in openList)
            {
                item.CaculateValue(currentPoint, endPoint);
            }
            openList.Sort((Point a, Point b) => {
                return a.F - b.F;
            });
            return openList[0];
        }
    }
}