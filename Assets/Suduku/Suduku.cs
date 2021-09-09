using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Suduku : MonoBehaviour
{
    public GameObject item;
    public Transform itemParent;
    public float itemW = 100f;
    public float itemH = 100f;
    public int w = 9;
    public int h = 9;
    GameObject[,] items = new GameObject[9, 9];
    int[,] numbers = new int[9, 9];
    void Start()
    {
        GenerateNumbers();
        GenerateItems();
        FillNum2Item();
    }
    void GenerateNumbers()
    {
        int startNum = 0;
        for (int i = 0; i < w; i++)
        {
            for (int j = 0; j < h; j++)
            {
                numbers[i, j] = (i + j) % 9 + 1;
            }
        }
    }

    void GenerateItems() {
        for (int i = 0; i < w; i++)
        {
            for (int j = 0; j < h; j++)
            {
                float x = (i - 4) * 100;
                float y = (j - 4) * 100;
                Transform itemTr = GameObject.Instantiate(item).transform;
                itemTr.name = string.Format("{0},{1}", i, j);
                items[i, j] = itemTr.gameObject;
                itemTr.SetParent(itemParent);
                itemTr.localPosition = new Vector3(x, y, 0);

                Image img = itemTr.GetComponent<Image>();
                if (i < 3 && j < 3 ||
                    i > 5 && j < 3 ||
                    i < 3 && j > 5 ||
                    i > 5 && j > 5 ||
                    i >= 3 && i <= 5 && j >= 3 && j <= 5)
                {
                    img.color = new Color(1, 1, 1, 1);
                }
                else {
                    img.color = new Color(0.8f, 0.8f, 0.8f, 1f);
                }
            }
        }
    }

    void FillNum2Item() {
        for (int i = 0; i < w; i++)
        {
            for (int j = 0; j < h; j++)
            {
                items[i, j].transform.Find("Text").GetComponent<Text>().text = numbers[i, j].ToString();
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
