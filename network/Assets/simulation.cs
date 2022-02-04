using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class simulation : init
{
    // Start is called before the first frame update

    int totalSimCnt = 1000;

    void Start()
    {
        nodeCnt = -1;  // hasn't been initialized
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void backToDayNow()
    {
        for (int i = 0; i < nodeCnt; i++) nodes[i].GetComponent<node>().currentStatus = init.Instance.nodes[i].GetComponent<node>().currentStatus;
        sickCnt = init.Instance.sickCnt;
        daycnt = init.Instance.daycnt;
        quarantineCnt = init.Instance.quarantineCnt;
    }

    void simInit()
    {
        nodeCnt = init.Instance.nodeCnt;
        nodes = new GameObject[nodeCnt];
        daycnt = init.Instance.daycnt;
        totaldays = init.Instance.totaldays;
        for (int i = 0; i < nodeCnt; i++)
        {
            nodes[i] = Instantiate(init.Instance.nodes[i], new Vector3(100, 100, 0), Quaternion.identity);    // node will not be visualized, but data is useful
            nodes[i].GetComponent<node>().neighbourCnt = init.Instance.nodes[i].GetComponent<node>().neighbourCnt;
            for (int j = 0; j < nodes[i].GetComponent<node>().neighbourCnt; j++)
                nodes[i].GetComponent<node>().neighbours[j] = init.Instance.nodes[i].GetComponent<node>().neighbours[j];            
        }
        Psick = init.Instance.Psick;
        totaldays = init.Instance.totaldays;

        
    }

    public void startSimulation()
    {
        if (nodeCnt == -1)  simInit(); // Initialize simulation if it hasn't been initialized
        backToDayNow();

        int[,] simResults = new int[totaldays, totalSimCnt];
        float[] averageSimResults = new float[totaldays];
        for (int iSim = 0; iSim <totalSimCnt; iSim ++)
        {
            for (;daycnt< totaldays-1; )
            {
                

                nextdaySim();
                simResults[daycnt, iSim] = sickCnt;
                averageSimResults[daycnt] += sickCnt;

            }
            backToDayNow();

        }


        for (int daySim = daycnt + 1; daySim < totaldays; daySim++) 
        { 
            averageSimResults[daySim] /= totalSimCnt;
            Debug.Log("Simumlation: day" + daySim + ", Sickcnt=" + averageSimResults[daySim] + ",Percentage= " + averageSimResults[daySim]*100/nodeCnt + "%");
        }




    }

    void nextdaySim()
    {
        daycnt++;
        if (sickCnt == 0)   // the first day 
        {
            int sickIndex = Random.Range(0, nodeCnt - 1);
            nodes[sickIndex].GetComponent<node>().currentStatus = global::node.nodeStatus.Sick;
            sickCnt++;
           
        }
        else
        {
            int[] nextSick = new int[nodeCnt];
            int nextSickcnt = 0;
            for (int i = 0; i < nodeCnt; i++)      // each sick node has a Psick probability to infect its neighbour. 
            {
                if (nodes[i].GetComponent<node>().currentStatus == global::node.nodeStatus.Sick)
                {
                    for (int j = 0; j < nodes[i].GetComponent<node>().neighbourCnt; j++)
                    {
                        if (Random.Range(0f, 1f) < Psick)
                        {
                            if (nodes[nodes[i].GetComponent<node>().neighbours[j]].GetComponent<node>().currentStatus != global::node.nodeStatus.Sick)
                            {
                                nextSick[nextSickcnt] = nodes[i].GetComponent<node>().neighbours[j];
                                nextSickcnt++;

                            }
                        }
                    }
                }
            }


            for (int i = 0; i < nextSickcnt; i++)
            {
                if (nodes[nextSick[i]].GetComponent<node>().currentStatus != global::node.nodeStatus.Sick)
                {
                    if (nodes[nextSick[i]].GetComponent<node>().currentStatus != global::node.nodeStatus.Quaratine)
                    {
                        nodes[nextSick[i]].GetComponent<node>().currentStatus = global::node.nodeStatus.Sick;
                        sickCnt++;
                    }
                }
            }

        }
            



        }
    

}
