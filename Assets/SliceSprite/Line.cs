using UnityEngine;
using System.Collections;
using System.Collections.Generic;
namespace MiaoKids
{
    public class Line
    {
        public Vector2 startPos;
        public Vector2 endPos;
        public Line(Vector2 startPos, Vector2 endPos){
            this.startPos = startPos;
            this.endPos = endPos;
        }
        // 计算直线方程参数 k b
        public static void GetLineEquation(Line line, out float k, out float b){
            k = 0;
            b = 0;
            k = (line.startPos.y - line.endPos.y) / (line.startPos.x - line.endPos.x);
            b = line.startPos.y - k * line.startPos.x;
        }

        // 获取两条直线的交点
        // lin1,line2 : 
        public static bool GetIntersectionPoint(Line line1, Line line2, out Vector2 intersectionPoint){
            intersectionPoint = Vector2.one; 

            // float k_line1 = (line1.startPos.y - line1.endPos.y) / (line1.startPos.x - line1.endPos.x);
            // float b_line1 = line1.startPos.y - k_line1 * line1.startPos.x;
            float k_line1 = 0;
            float b_line1 = 0;
            GetLineEquation(line1, out k_line1, out b_line1);

            // float k_line2 = (line2.startPos.y - line2.endPos.y) / (line2.startPos.x - line2.endPos.x);
            // float b_line2 = line2.startPos.y - k_line2 * line2.startPos.x;
            float k_line2 = 0;
            float b_line2 = 0;
            GetLineEquation(line2, out k_line2, out b_line2);

            if (k_line1 == b_line1){
                // 直线平行
                return false;
            }
            
            float y = 0;
            float x = 0;
            if (k_line2 == 0 && k_line1 != 0){
                y = b_line2;
                x = (y - b_line1) / k_line1;
            }
            else if (k_line1 == 0 && k_line2 != 0){
                y = b_line1;
                x = (y - b_line2) / k_line2;
            }
            else{
                y = (k_line2 * b_line1 - k_line1 * b_line2) / (k_line2 - k_line1);
                x = (y - b_line1) / k_line1;
            }

            float line1MaxX = Mathf.Max(line1.startPos.x, line1.endPos.x);
            float line1MinX = Mathf.Min(line1.startPos.x, line1.endPos.x);
            float line1MaxY = Mathf.Max(line1.startPos.y, line1.endPos.y);
            float line1MinY = Mathf.Min(line1.startPos.y, line1.endPos.y);

            float line2MaxX = Mathf.Max(line2.startPos.x, line2.endPos.x);
            float line2MinX = Mathf.Min(line2.startPos.x, line2.endPos.x);
            float line2MaxY = Mathf.Max(line2.startPos.y, line2.endPos.y);
            float line2MinY = Mathf.Min(line2.startPos.y, line2.endPos.y);

            // 交点坐标是否在line1线段坐标范围内
            bool isInLineArea = x >= line1MinX && x <= line1MaxX && y >= line1MinY && y <= line1MaxY;
            // 交点坐标是否在line2线段坐标范围内
            bool isInThisArea = x >= line2MinX && x <= line2MaxX && y >= line2MinY && y <= line2MaxY;

            if (isInThisArea && isInLineArea){
                intersectionPoint = new Vector2(x, y);
                return true;
            }

            return false;
        }

        public static Dictionary<string, List<Vector2>> GetPositionOfPointWithLine(Line line, Vector2[] points){
            Dictionary<string, List<Vector2>> pointPosDic = new Dictionary<string, List<Vector2>>();
            pointPosDic["up"] = new List<Vector2>();
            pointPosDic["on"] = new List<Vector2>();
            pointPosDic["down"] = new List<Vector2>();
            for (int i = 0; i < points.Length; i++)
            {
                Vector2 point = points[i];
                float k = 0;
                float b = 0;
                GetLineEquation(line, out k, out b);
                float realY = k * point.x + b;
                // 保留两位小数判断位置
                if (System.Math.Round(realY, 2) == System.Math.Round(point.y, 2)){
                    pointPosDic["on"].Add(point);
                }
                else if (System.Math.Round(realY, 2) < System.Math.Round(point.y, 2)){
                    pointPosDic["up"].Add(point);
                }
                else if (System.Math.Round(realY, 2) > System.Math.Round(point.y, 2)) {
                    pointPosDic["down"].Add(point);
                }
            }
            return pointPosDic;
        }
    }
}