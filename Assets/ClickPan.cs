using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ClickPan : MonoBehaviour {
    public GameObject Pan;
    public GameObject Dot;
    public GameObject EndPopup;
    public Text infotext;
    private LineRenderer lineRenderer;
    Vector3[,] arr = new Vector3[19, 19];
    int[,,] stone = new int[2, 19, 19];
    int turn;
    


    void OnMouseEnter()
    {
      
    }
    void OnMouseExit()
    {

    }

    void OnMouseUp()
    {
        //Debug.Log("OnMouseUp");
    }
    void OnMouseDown()
    {
        Debug.Log("OnMouseDown");
        Vector3 mouseInput = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        float min = 10000f;
        int indexx = -1;
        int indexy = -1;
        for (int i = 0; i < 19; i++)
        {
            for (int j = 0; j < 19; j++)
            {
                if (Mathf.Sqrt((mouseInput.x - arr[i, j].x) * (mouseInput.x - arr[i, j].x) + (mouseInput.y - arr[i, j].y) * (mouseInput.y - arr[i, j].y)) < min)
                {
                    indexx = i;
                    indexy = j;
                    min = Mathf.Sqrt((mouseInput.x - arr[i, j].x) * (mouseInput.x - arr[i, j].x) + (mouseInput.y - arr[i, j].y) * (mouseInput.y - arr[i, j].y));
                }
            }
        }
        if (stone[turn, indexx, indexy] == 0 && stone[1 - turn, indexx, indexy] == 0)
        {
            stone[turn, indexx, indexy] = 1;
            Vector3 vect = new Vector3(arr[indexx, indexy].x, arr[indexx, indexy].y, -1);
            GameObject newdot = Instantiate(Dot, vect, Quaternion.Euler(90.0f, 0, 0));
            if (turn == 0) newdot.GetComponent<MeshRenderer>().material.color = Color.black;
            else newdot.GetComponent<MeshRenderer>().material.color = Color.white;
            newdot.transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);
            turn = 1 - turn;
            if(turn == 0) infotext.text = "흑돌 차례입니다.";
            else infotext.text = "백돌 차례입니다.";
        }
        if (evaluate(stone) == 1)
        {
            infotext.text = "흑돌이 이겼습니다.";

          
            EndPopup.SetActive(true);
            

            //break;
        }
        else if (evaluate(stone) == 2)
        {
            infotext.text = "백돌이 이겼습니다.";
          
            EndPopup.SetActive(true);

            //break;
        }
    }
    void Awake()
    {
        Debug.Log("OnAwake");
     
    }
    // Use this for initialization
    void Start () {
        EndPopup.SetActive(false);
        turn = 0; // 검정돌
        Debug.Log("Onstart");
        Color32 color = new Color(220 / 255.0f, 179 / 255.0f, 92 / 255.0f, 255f / 255.0f);
        Pan.GetComponent<MeshRenderer>().material.color = color;
        color = new Color(0 / 255.0f, 0 / 255.0f, 0 / 255.0f, 255f / 255.0f);
        Dot.GetComponent<MeshRenderer>().material.color = color;

        RectTransform r = Pan.gameObject.AddComponent<RectTransform>();
 


        float panwidth = 8.0f;
        float panheight = 8.0f;
        float edge = 0.5f;

        panwidth = transform.localScale.x;
        panheight = transform.localScale.y;
        Debug.Log(panwidth);
        Debug.Log(panheight);

        for (int i = 0; i < 19; i++)
        {
            for (int j = 0; j < 19; j++)
            {
                Vector3 vect = new Vector3((panwidth / (-2.0f)) + edge + (panwidth - 2 * edge) / 18 * j,
                    (panheight / (-2.0f)) + edge + (panheight - 2 * edge) / 18 * i, -1);
                arr[i, j] = vect;
                if ((i == 3 && j == 3) || (i == 3 && j == 9) || (i == 3 && j == 15) || (i == 9 && j == 3) || (i == 9 && j == 9) || (i == 9 && j == 15) || (i == 15 && j == 3) || (i == 15 && j == 9) || (i == 15 && j == 15))
                {
                    vect.z = -1;
                }
                else vect.z = 0;               
                Instantiate(Dot, vect, Quaternion.Euler(90.0f, 0, 0));
            }
        }      
      
        for (int i = 0; i < 19; i++)
        {
            lineRenderer = new GameObject("Line").AddComponent<LineRenderer>();
            lineRenderer.material.color = Color.black;
            lineRenderer.SetWidth(0.01f, 0.01f);
            color = new Color(0 / 255.0f, 0 / 255.0f, 0 / 255.0f, 255f / 255.0f);
            lineRenderer.SetColors(color, color);
            lineRenderer.SetPosition(0, arr[i, 0]);
            lineRenderer.SetPosition(1, arr[i, 18]);
        }
        for (int i = 0; i < 19; i++)
        {
            lineRenderer = new GameObject("Line").AddComponent<LineRenderer>();
            lineRenderer.material.color = Color.black;
            lineRenderer.SetWidth(0.01f, 0.01f);
            lineRenderer.SetColors(Color.black, Color.black);
            lineRenderer.SetPosition(0, arr[0, i]);
            lineRenderer.SetPosition(1, arr[18, i]);
        }
        //infotext.transform.localScale = new Vector3(0f, 0f, -2f);
        infotext.text = "흑돌 차례입니다.";
    }

    // Update is called once per frame
    void Update () {
    
    }

    int evaluate(int [,,] matrix)
    {
        int clear = 0;
        for (int i = 0; i < 19; i++)
        {
            for (int j = 0; j < 14; j++)
            {
                clear = 0;
                for (int k = 0; k < 5; k++)
                {
                    if (matrix[0,i,j + k] != 1)
                    {
                        clear = 1;
                        break;
                    }
                }
                if (clear == 0) return 1;

            }
        }
        clear = 0;
        for (int i = 0; i < 14; i++)
        {
            for (int j = 0; j < 19; j++)
            {
                clear = 0;
                for (int k = 0; k < 5; k++)
                {
                    if (matrix[0,i + k,j] != 1)
                    {
                        clear = 1;
                        break;
                    }
                }
                if (clear == 0) return 1;
            }
        }
        clear = 0;
        for (int i = 0; i < 14; i++)
        {
            for (int j = 0; j < 14; j++)
            {
                clear = 0;
                for (int k = 0; k < 5; k++)
                {
                    if (matrix[0,i + k,j + k] != 1)
                    {
                        clear = 1;
                        break;
                    }
                }
                if (clear == 0) return 1;
            }
        }

        clear = 0;
        for (int i = 0; i < 19; i++)
        {
            for (int j = 0; j < 14; j++)
            {
                clear = 0;
                for (int k = 0; k < 5; k++)
                {
                    if (matrix[1,i,j + k] != 1)
                    {
                        clear = 1;
                        break;
                    }
                }
                if (clear == 0) return 2;
            }
        }
        clear = 0;
        for (int i = 0; i < 14; i++)
        {
            for (int j = 0; j < 19; j++)
            {
                clear = 0;
                for (int k = 0; k < 5; k++)
                {
                    if (matrix[1,i + k,j] != 1)
                    {
                        clear = 1;
                        break;
                    }
                }
                if (clear == 0) return 2;
            }
        }
        clear = 0;
        for (int i = 0; i < 14; i++)
        {
            for (int j = 0; j < 14; j++)
            {
                clear = 0;
                for (int k = 0; k < 5; k++)
                {
                    if (matrix[1,i + k,j + k] != 1)
                    {
                        clear = 1;
                        break;
                    }
                }
                if (clear == 0) return 2;
            }
        }

        return 0; //게임이 안 끝났으면 0 반환
    }


}
