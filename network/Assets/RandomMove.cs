using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomMove : MonoBehaviour
{
    public enum moveStatus { random, circle};
    public moveStatus currentMoveStatus = moveStatus.random , lastMoveStatus = moveStatus.random;
  
    
    public int startCircleCD = 0;
    


    void circleMove()
    {
        Vector3 a;
        a.x = 0.01f * Mathf.Sin(Time.time * 20);
        a.y = 0.01f * Mathf.Cos(Time.time * 20);
        a.z = 0;
        transform.Translate(a);
    }

    void randomMove()
    {
        Vector3 a;
        a.x = Random.Range(-0.001f, 0.001f);
        a.y = Random.Range(-0.001f, 0.001f);
        a.z = 0;
        transform.Translate(a);

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (init.Instance.currentGameStarus != init.gameStatus.play) return;
        if (currentMoveStatus == moveStatus.random)
        {
            if (lastMoveStatus == moveStatus.circle)
            {
                // existing circle
                // tell neighbour to stop circle
                for (int i = 0; i < GetComponent<node>().neighbourCnt; i++)
                {
                    
                    init.Instance.nodes[GetComponent<node>().neighbours[i]].GetComponent<RandomMove>().currentMoveStatus = moveStatus.random;
                    init.Instance.nodes[GetComponent<node>().neighbours[i]].GetComponent<RandomMove>().startCircleCD = 0;
                }
            }
            randomMove();


        }
        else if (currentMoveStatus == moveStatus.circle)
        {
            if (lastMoveStatus == moveStatus.random)
            {
                // entering circle
                // tell neighbours to enter circle in 100 frames 

                for (int i = 0; i < GetComponent<node>().neighbourCnt; i++)
                {
                    if (init.Instance.nodes[GetComponent<node>().neighbours[i]].GetComponent<RandomMove>().currentMoveStatus == moveStatus.random)
                        init.Instance.nodes[GetComponent<node>().neighbours[i]].GetComponent<RandomMove>().startCircleCD = 100;
                }

            }
            circleMove();

        }

        lastMoveStatus = currentMoveStatus;

        if (startCircleCD >0 )
        {
            startCircleCD--;
            if (startCircleCD == 0) currentMoveStatus = moveStatus.circle;
        }
        
    }




    
    private void OnMouseOver()
    {
        currentMoveStatus = moveStatus.circle;
        
    }


 
    private void OnMouseExit()
    {
        currentMoveStatus = moveStatus.random;
    }
}
