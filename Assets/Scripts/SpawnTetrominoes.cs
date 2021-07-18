using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SpawnTetrominoes : MonoBehaviour
{

    public GameObject[] Tetrominoes;

  
    private GameObject newTetro;


    // Start is called before the first frame update
    void Start()
    {

        NewTetromino();


      
    }

    // Update is called once per frame
   public void NewTetromino()
    {

        //Instantiate(Tetrominoes[Random.Range(0, Tetrominoes.Length)],transform.position, Quaternion.identity);
        
        newTetro = (GameObject)Instantiate(Tetrominoes[Random.Range(0, Tetrominoes.Length)], transform.position, Quaternion.identity);
   
        RandomSprite();
        newTetro.GetComponent<TetrisBlock>().ChangeColor();






    }

    public void RandomSprite()
    {



        //tetro裡有幾個block
        int blockNum = newTetro.transform.childCount;
        
        
        //把block存��Eist
        List<GameObject> tetroBlock = new List<GameObject>();
        for (int i = 0; i < blockNum; i++)
        {
            

            GameObject obj = newTetro.transform.GetChild(i).gameObject;
            tetroBlock.Add(obj);
           

           // obj.GetComponent<SpriteRenderer>().sprite = blockSprite[Random.Range(0, blockSprite.Length)];

        }

        //給這些block隨機換圖
        for (int j = 0; j < tetroBlock.Count; j++)
        {
            tetroBlock[j].GetComponentInChildren<TMP_Text>().text = (Random.Range(1, 3) * 2).ToString();

            //Debug.Log(tetroBlock[j].GetComponentInChildren<TMP_Text>().text);
        }


       



    }
}
