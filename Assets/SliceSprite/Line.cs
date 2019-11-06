using UnityEngine;

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

        // 获取两条直线的交点
        // lin1,line2 : 
        public static bool GetIntersectionPoint(Line line1, Line line2, out Vector2 intersectionPoint){
            intersectionPoint = Vector2.one; 

            float k_line1 = (line1.startPos.y - line1.endPos.y) / (line1.startPos.x - line1.endPos.x);
            float b_line1 = line1.startPos.y - k_line1 * line1.startPos.x;

            float k_line2 = (line2.startPos.y - line2.endPos.y) / (line2.startPos.x - line2.endPos.x);
            float b_line2 = line2.startPos.y - k_line2 * line2.startPos.x;

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
            // 交点坐标是否在lin2线段坐标范围内
            bool isInThisArea = x >= line2MinX && x <= line2MaxX && y >= line2MinY && y <= line2MaxY;

            if (isInThisArea && isInLineArea){
                intersectionPoint = new Vector2(x, y);
                return true;
            }

            return false;
        }
    }
}