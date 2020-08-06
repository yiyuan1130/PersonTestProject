using UnityEngine;

namespace AStart{
    public class Point{
        public string name;
        public Vector2 pos;
        public SpriteRenderer sr;
        public int H;
        public int G;
        public int F;
        public Point from = null;
        public Point(int x, int y, SpriteRenderer sr){
            this.pos = new Vector2(x, y);
            this.sr = sr;
            this.name = string.Format("{0}_{1}", x, y);
            // this.from = p;
            this.G = 100000;
        }
        public void CaculateValue(Point p1, Point p2){
            if (this.G > p1.G + 1){
                this.G = p1.G + 1;
                this.from = p1;
                // this.G = (int)Mathf.Abs(this.pos.x - p1.pos.x) + (int)Mathf.Abs(this.pos.y - p1.pos.y);
                this.H = (int)Mathf.Abs(this.pos.x - p2.pos.x) + (int)Mathf.Abs(this.pos.y - p2.pos.y);
                // this.H = 0;
                this.F = this.G + this.H;
            }
        }
    }
}