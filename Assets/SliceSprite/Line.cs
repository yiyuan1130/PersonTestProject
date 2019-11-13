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
        // return 是否是竖直的线 垂直true 否则false
        public static bool GetLineEquation(Line line, out float k, out float b){
            k = 0;
            b = 0;
            float durX = line.startPos.x - line.endPos.x;
            float durY = line.startPos.y - line.endPos.y;
            if (durX == 0){
                return true;
            }
            else{
                k = durY / durX;
                b = line.startPos.y - k * line.startPos.x;
                // k = (float)System.Math.Round(k, 2);
                // b = (float)System.Math.Round(b, 2);
                return false;
            }
        }

        // 获取两条直线的交点
        // lin1,line2 : 
        public static bool GetIntersectionPoint(Line line1, Line line2, out Vector2 intersectionPoint){
            intersectionPoint = Vector2.one;

            // float k_line1 = (line1.startPos.y - line1.endPos.y) / (line1.startPos.x - line1.endPos.x);
            // float b_line1 = line1.startPos.y - k_line1 * line1.startPos.x;
            float k_line1 = 0;
            float b_line1 = 0;
            bool isVertical1 = GetLineEquation(line1, out k_line1, out b_line1);

            // float k_line2 = (line2.startPos.y - line2.endPos.y) / (line2.startPos.x - line2.endPos.x);
            // float b_line2 = line2.startPos.y - k_line2 * line2.startPos.x;
            float k_line2 = 0;
            float b_line2 = 0;
            bool isVertical2 = GetLineEquation(line2, out k_line2, out b_line2);

            if (isVertical1 && isVertical2){
                // 两直线平行切都垂直于X轴
                return false;
            }
            
            if (k_line1 == k_line2)
                // 直线平行
                return false;

            float y = 0;
            float x = 0;
            if (isVertical1){
                x = line1.startPos.x;
                y = k_line2 * x + b_line2;
            }
            else if (isVertical2){
                x = line2.startPos.x;
                y = k_line1 * x + b_line1;
            }
            else if (k_line2 == 0 && k_line1 != 0){
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
            float k = 0;
            float b = 0;
            bool isVertical = GetLineEquation(line, out k, out b);
            for (int i = 0; i < points.Length; i++)
            {
                Vector2 point = points[i];
                if (isVertical){
                    if (Mathf.Abs(point.x - line.startPos.x) <= 0.001f){
                        pointPosDic["on"].Add(point);
                    }
                    else if (point.x < line.startPos.x){
                        pointPosDic["down"].Add(point);
                    }
                    else if (point.x > line.startPos.x){
                        pointPosDic["up"].Add(point);
                    }
                }
                else{
                    float realY = k * point.x + b;
                    // 保留两位小数判断位置
                    if (Mathf.Abs(realY - point.y) <= 0.001f){
                        pointPosDic["on"].Add(point);
                    }
                    else if (realY < point.y){
                        pointPosDic["up"].Add(point);
                    }
                    else if (realY > point.y) {
                        pointPosDic["down"].Add(point);
                    }
                }
            }
            return pointPosDic;
        }
    }
}