using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AStart{
	public class AStarController : MonoBehaviour {
		public GameObject pointObj;
        public Vector2 startVector;
        public Vector2 endVector;

        public int wallPos;
        public int wallWidth;

		Transform ground;
		Vector3 startPos = new Vector3(-20, -11, 0);
		int column = 41;
		int row = 23;
		
		Dictionary<string, Point> points;
        Dictionary<string, Point> wall;
		void Start () {
			ground = GameObject.Find("Ground").transform;
			points = new Dictionary<string, Point>(); 
            wall = new Dictionary<string, Point>();
			CreatePoints();
			CreateStartEnd();
            CreateWall();
            StartFindPath();
		}
		
		// Update is called once per frame
		void Update () {
			
		}

		void CreatePoints(){
			for (int i = 0; i < column; i++)
			{
				for (int j = 0; j < row; j++)
				{
					GameObject point = GameObject.Instantiate(pointObj);
					point.transform.SetParent(ground);
					point.transform.localScale = Vector3.one * 0.3f;
					point.transform.position = startPos + new Vector3(i, j, 0);
					point.gameObject.name = i + "_" + j;
					SpriteRenderer sr = point.GetComponent<SpriteRenderer>();
					Point p = new Point(i, j, sr);
                    if (i == wallPos && Mathf.Abs(j - 11) < wallWidth){
                        wall[i + "_" + j] = p;
                    }else{
					    points[i + "_" + j] = p;
                    }
				}
			}
		}

		Point startPoint;
		Point endPoint;
		void CreateStartEnd(){
			startPoint = points[string.Format("{0}_{1}", startVector.x, startVector.y)];
			endPoint = points[string.Format("{0}_{1}", endVector.x, endVector.y)];
			startPoint.sr.color = Color.green;
			endPoint.sr.color = Color.red;
		}

        void CreateWall(){
            foreach (var item in wall)
            {
                item.Value.sr.color = Color.gray;   
            }
        }

        public void StartFindPath(){
            Debug.Log("StartFindPath");
            StartCoroutine(Caculate());
        }

        List<Point> openList;
        List<Point> closeList;
        List<Point> pathPoints;
        Point currentPoint;

        IEnumerator Caculate(){
            openList = new List<Point>();
            closeList = new List<Point>();
            pathPoints = new List<Point>();
            openList.Add(startPoint);
			startPoint.G = 0;
            int index = 0;
            WaitForSeconds wait = new WaitForSeconds(0.25f);
			while (openList.Count > 0 && index < 1000){
				index ++;
				openList.Sort((Point a, Point b) => {
					if (a.H == b.H){
						return a.F - b.F;
					}
					return a.H - b.H;
				});
				currentPoint = openList[0];
				Debug.Log(currentPoint.name);
				currentPoint.sr.color = Color.yellow;
				openList.RemoveAt(0);
				yield return wait;
				currentPoint.sr.color = Color.blue;
				Debug.LogFormat("G{0} H{1} F{2}", currentPoint.G, currentPoint.H, currentPoint.F);
				yield return wait;
				if (currentPoint.name == endPoint.name){
					break;
				}
				closeList.Add(currentPoint);
				getNext(0, 1);
				getNext(0, -1);
				getNext(1, 0);
				getNext(1, -1);
			}

			// Point p = endPoint;
			// while (p != null){
			// 	p.sr.color = Color.yellow;
			// 	p = p.from;
			// 	yield return wait;
			// }
        }

        void getNext(int dx, int dy){
            string up = string.Format("{0}_{1}", currentPoint.pos.x + dx, currentPoint.pos.y + dy);
            if (points.ContainsKey(up)){
				if (!closeList.Contains(points[up])){
					points[up].CaculateValue(currentPoint, endPoint);
					if (!openList.Contains(points[up])){
						openList.Add(points[up]);
					}
				}
            }
        }

        IEnumerator ChangeColor(){
            for (int i = 0; i < openList.Count; i++)
            {
                openList[i].sr.color = Color.blue;
            }
            yield return new WaitForSeconds(0.5f);
            for (int i = 0; i < openList.Count; i++)
            {
                openList[i].sr.color = Color.white;
            }
        }
	}
}
