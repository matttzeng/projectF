using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TetrisBlock : MonoBehaviour
{
    public Vector3 rotationPoint;
    private float previousTime;
    public float fallTime = 0.8f;
    public static int height = 20;
    public static int width = 10;
    private static Transform[,] grid = new Transform[width, height];
    public GameObject[] Squares;
    public Sprite[] Sprites;


    // Start is called before the first frame update
    void Start()
    {
        /*
        for(int i = 0; i < Squares.Length; i++)
        {
            int j = Random.Range(0, 2);
            Squares[i].GetComponent<SpriteRenderer>().sprite = Sprites[j];
        }
        */
    }

    // Update is called once per frame
    void Update()
    {
       

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            transform.position += new Vector3(-1, 0, 0);
            if (!ValidMove())
                transform.position -= new Vector3(-1, 0, 0);
        }        
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            transform.position += new Vector3(1, 0, 0);
            if (!ValidMove())
                transform.position -= new Vector3(1, 0, 0);
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            //rotate
            transform.RotateAround(transform.TransformPoint(rotationPoint), new Vector3(0,0,1), 90);
            RotateText(-90);
            if (!ValidMove())
            {
                transform.RotateAround(transform.TransformPoint(rotationPoint), new Vector3(0, 0, 1), -90);
                RotateText(90);
            }
                

        }





        if(Time.time -previousTime>(Input.GetKey(KeyCode.DownArrow) ? fallTime/10 : fallTime))
        {
            transform.position += new Vector3(0, -1, 0);
           /*
            if(CheckCombine())
            {
                return;
            }
           */
            if (!ValidMove())
            {
                transform.position -= new Vector3(0, -1, 0);
                AddToGrid();
                //CheckForLines();

                CheckCombine();
                

                this.enabled = false;
                FindObjectOfType<SpawnTetrominoes>().NewTetromino();
            }
                

            previousTime = Time.time;

        }

    }

    void AddToGrid()
    {
        foreach(Transform children in transform)
        {
            int roundedX = Mathf.RoundToInt(children.transform.position.x);
            int roundedY = Mathf.RoundToInt(children.transform.position.y);

            grid[roundedX, roundedY] = children;

        }
    }


    void CheckForLines()
    {
        for (int i = height - 1; i >= 0; i--)
        {
            if (HasLine(i))
            {
                DeleteLine(i);
                RowDown(i);
            }
        }
    }

    bool HasLine(int i)
    {
        for(int j = 0; j < width; j++)
        {
            if (grid[j, i] == null)
                return false;

        }
        return true;
    }
    void DeleteLine(int i)
    {
        for (int j = 0; j < width; j++)
        {
            Destroy(grid[j, i].gameObject);
            grid[j, i] = null;
        }
    }

    void RowDown(int i)
    {
        for (int y = i; y < height; y++)
        {
            for (int j = 0; j < width; j++)
            {
                if (grid[j, y] != null)
                {
                    grid[j, y - 1] = grid[j, y];
                    grid[j, y] = null;
                    grid[j, y - 1].transform.position -= new Vector3(0, 1, 0);
                }
            }
        }
    }

    void CheckCombine()
    {
        


        for (int j = width -1;j>=0; j--)
        {
            for (int i = height - 1; i >= 0; i--)
            {
                if (HasCombine(j, i))
                {
                    Debug.Log("hasCombine" + j + "  " + i);
                    DeleteBlock(j, i);
                    BlockDown(j, i);

                    continue;

                }
            }
        }

       
     
    }
    bool HasCombine(int j, int i)
    {
        Debug.Log("HasCombine");
       
                if (grid[j, i] != null && grid[j,i+1]) {
                    Debug.Log(grid[j,i].name);
                    Debug.Log(grid[j, i].GetComponentInChildren<TMP_Text>().text);
                    if (grid[j, i]. GetComponentInChildren<TMP_Text>().text == grid[j, i + 1].GetComponentInChildren<TMP_Text>().text)
                    {

                        return true;
                       
                    }
                    
                }

               

      

        return false;
    }
    void DeleteBlock(int j,int i)
    {
        Debug.Log("DeleteBlock  " + j + " " + i);
        Destroy(grid[j, i] .gameObject);
            grid[j, i] = null;
       
    }

    void BlockDown(int j,int i)
    {
        Debug.Log("blockDown");
                   
        for(int k= i; k < height-1; k++)
        {
             if (grid[j, k+1] != null)
             {
                grid[j, k] = grid[j, k + 1];             
                grid[j, k + 1] = null;
                grid[j, k ].transform.position -= new Vector3(0, 1, 0);

                CombineNum(j, i);
            }
          
        }
                   
       
    }
    void CombineNum(int j, int i)
    {
        grid[j, i].GetComponentInChildren<TMP_Text>().text = (int.Parse(grid[j, i].GetComponentInChildren<TMP_Text>().text)*2).ToString();
        ChangeColor();

    }
   void RotateText(int rotateAngle)
    {
        foreach (Transform children in transform)
        {
            children.GetComponentInChildren<TMP_Text>().transform.eulerAngles += new Vector3(0, 0, rotateAngle);
        }
           
    }

    public void ChangeColor()
    {
        foreach (Transform children in transform)
        {
         
            string numText = children.GetComponentInChildren<TMP_Text>().text;
            switch (numText)
            {
                case "2":
                    children.GetComponent<SpriteRenderer>().color = new Color(0.92f, 0.85f, 0.82f);
                    break;
                case "4":
                    children.GetComponent<SpriteRenderer>().color = new Color(0.9f, 0.85f, 0.75f);
                    break;
                case "8":
                    children.GetComponent<SpriteRenderer>().color = new Color(0.92f, 0.6f, 0.43f);
                    break;
                case "16":
                    children.GetComponent<SpriteRenderer>().color = new Color(0.92f, 0.5f, 0.3f);
                    break;
                case "32":
                    children.GetComponent<SpriteRenderer>().color = new Color(0.92f, 0.4f, 0.3f);
                    break;
                case "64":
                    children.GetComponent<SpriteRenderer>().color = new Color(0.92f, 0.4f, 0.3f);
                    break;
                case "128":
                    children.GetComponent<SpriteRenderer>().color = new Color(0.92f, 0.4f, 0.3f);
                    break;
                case "256":
                    children.GetComponent<SpriteRenderer>().color = new Color(0.9f, 0.8f, 0.4f);
                    break;
                case "512":
                    children.GetComponent<SpriteRenderer>().color = new Color(0.9f, 0.8f, 0.4f);
                    break;
                case "1024":
                    children.GetComponent<SpriteRenderer>().color = new Color(0.9f, 0.75f, 0.2f);
                    break;
                case "2048":
                    children.GetComponent<SpriteRenderer>().color = new Color(0.9f, 0.75f, 0.2f);
                    break;

                default:
                    break;

            }
        }
           
    }

    /*
    bool CheckCombine()
    {
        int sum = 0;

        foreach (Transform children in transform)
        {
            int j = Mathf.RoundToInt(children.transform.position.x);
            int i = Mathf.RoundToInt(children.transform.position.y);
            if(i>0 && j>0 && grid[j, i] != null)
                if (grid[j, i].GetComponentInParent<SpriteRenderer>().sprite == children.GetComponentInParent<SpriteRenderer>().sprite)
                {
                    Destroy(children.gameObject);
                    int l = int.Parse(grid[j, i].GetComponentInParent<SpriteRenderer>().sprite.name)*2;
                    int o = 0;
                    //ぃ笵或本赣计Sprite...ノ程猜よ猭...
                    if (l == 2)
                        o = 0;
                    else if (l == 4)
                        o = 1;
                    else if (l == 8)
                        o = 2;
                    else if (l == 16)
                        o = 3;
                    else if (l == 32)
                        o = 4;
                    else if (l == 64)
                        o = 5;
                    else if (l == 128)
                        o = 6;
                    else if (l == 256)
                        o = 7;
                    else if (l == 512)
                        o = 8;
                    else if (l == 1024)
                        o = 9;
                    else if (l == 2048)
                        o = 10;

                    grid[j, i].GetComponentInParent<SpriteRenderer>().sprite = grid[j, i].GetComponentInParent<TetrisBlock>().Sprites[o];
                    sum++;
                }
        }

        if (sum > 0)
            return true;

        else
            return false;
    }
    */
   
    bool ValidMove()
    {
        foreach (Transform children in transform)
        {
            int roundedX = Mathf.RoundToInt(children.transform.position.x);
            int roundedY = Mathf.RoundToInt(children.transform.position.y);

            if(roundedX <0 || roundedX >= width || roundedY<0 || roundedY>=height)
            {
                return false;
            }
            if (grid[roundedX, roundedY]!= null)
                return false;
        }
        return true; 
    }
}
