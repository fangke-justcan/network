using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class simulation : init
{
    // Start is called before the first frame update

    int totalSimCnt = 300;
    public int[,] simResults;
    public float[] averageSimResults ;
    int firstSickIndex;
    writeFile simulationRecord;
    writeFile networkRecord;

    void Awake()
    {
        nodeCnt = -1;  // hasn't been initialized
    }
    void Start()
    {
        nodeCnt = -1;  // hasn't been initialized
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void backToDayNow()
    {
        for (int i = 0; i < nodeCnt; i++)
        {
            nodes[i].GetComponent<node>().currentStatus = init.Instance.nodes[i].GetComponent<node>().currentStatus;
            nodes[i].GetComponent<node>().lastStatus = init.Instance.nodes[i].GetComponent<node>().lastStatus;

        }
        sickCnt = init.Instance.sickCnt;
        daycnt = init.Instance.daycnt;
        quarantineCnt = init.Instance.quarantineCnt;
      

      

    }
    
    public void recordSimulation()
    {
        simInit(); // Initialize simulation 
        backToDayNow();
        
        networkRecord = new writeFile(dataFolder + "/"+ System.DateTime.Now.ToString("HHmm") + "network.txt");
        simulationRecord = new writeFile(dataFolder + "/"  +System.DateTime.Now.ToString("HHmm") + "simulation.txt");
        networkRecord.appendData("node1,node2");
        for (int i = 0; i<nodeCnt; i++)
        {
            for (int j = 0; j < nodes[i].GetComponent<node>().neighbourCnt; j++)
            {
                networkRecord.appendData(""+i+"," +  nodes[i].GetComponent<node>().neighbours[j]);
            }

        }

        simulationRecord.appendData("simulation, day, sickPercentage, border_red_cnt, border_green_cnt, total_nodecnt=" + nodeCnt + ",Psick= " +Psick + ",Edgecnt= " + edgeCnt);
        int borderRed, borderGreen;
       
       


        for (int i = 0; i < totaldays; i++) averageSimResults[i] = 0;

        for (int iSim = 0; iSim < totalSimCnt; iSim++)
        {

            for (; daycnt < totaldays - 1;)
            {

                calcBorder(out borderRed, out borderGreen);
                simResults[daycnt, iSim] = sickCnt;
              //  averageSimResults[daycnt] += sickCnt;

                simulationRecord.appendData("" + iSim + "," + daycnt+ ","+ 1f *sickCnt/nodeCnt +","+ borderRed +"," + borderGreen );
                nextdaySim();

                

            }
            backToDayNow();

        }

        /*
        for (int daySim = daycnt + 1; daySim < totaldays; daySim++)
        {
            averageSimResults[daySim] /= totalSimCnt;
            Debug.Log("Simumlation: day" + daySim + ", Sickcnt=" + averageSimResults[daySim] + ",Percentage= " + averageSimResults[daySim] * 100 / nodeCnt + "%");
        }
        */


    }

    public void calcBorder(out int borderRed, out int borderGreen)
    {
        borderRed = 0;
        borderGreen = 0;
        int[] borderRedArray = new int[nodeCnt];
        int[] borderGreenArray = new int[nodeCnt];

        for (int i = 0; i< nodeCnt; i++)
        {
            if (nodes[i].GetComponent<node>().currentStatus == global::node.nodeStatus.Sick)
            {
                int neighborRed = 0;

                for (int j = 0; j < nodes[i].GetComponent<node>().neighbourCnt; j++)
                {
                    node nodeJ = nodes[nodes[i].GetComponent<node>().neighbours[j]].GetComponent<node>();
                    
                    if (nodeJ.currentStatus != global::node.nodeStatus.Sick)
                        for (int m = 0; m < nodeJ.neighbourCnt; m++)
                        {
                            
                            if (nodes[nodeJ.neighbours[m]].GetComponent<node>().currentStatus != global::node.nodeStatus.Sick)
                            {
                                borderGreenArray[nodeJ.index] = 1;
                                break;
                            }
                        }
                    else 
                    {
                        neighborRed++;
                    }
                
                }
                if (neighborRed <= 2* nodes[i].GetComponent<node>().neighbourCnt / 3 && neighborRed < nodes[i].GetComponent<node>().neighbourCnt) borderRedArray[i] = 1;
            }
        }

        for (int i = 0; i < nodeCnt; i++)
        {
            if (borderGreenArray[i] == 1) borderGreen++;
            if (borderRedArray[i] == 1) borderRed++;
        }


    }

    public void simInit()
    {
        totaldays = init.Instance.totaldays;
        if (nodeCnt == -1)
        {
            nodeCnt = init.Instance.nodeCnt;
            nodes = new GameObject[nodeCnt];
            simResults = new int[totaldays, totalSimCnt];
            averageSimResults = new float[totaldays];
           
        }
        edgeCnt = init.Instance.edgeCnt;
        daycnt = init.Instance.daycnt;
        Psick = init.Instance.Psick;

       
        
        for (int i = 0; i < nodeCnt; i++)
        {
            if (nodes[i] == null) nodes[i] = Instantiate(init.Instance.nodes[i], new Vector3(100, 100, 0), Quaternion.identity);    // node will not be visualized, but data is useful
            nodes[i].GetComponent<node>().neighbourCnt = init.Instance.nodes[i].GetComponent<node>().neighbourCnt;
            for (int j = 0; j < nodes[i].GetComponent<node>().neighbourCnt; j++)
                nodes[i].GetComponent<node>().neighbours[j] = init.Instance.nodes[i].GetComponent<node>().neighbours[j];            
        }


        dataFolder = init.Instance.dataFolder;

    }

    public void startSimulation()
    {

        simInit(); // Initialize simulation 
        backToDayNow();
        for (int i = 0; i < totaldays; i++) averageSimResults[i] = 0;

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
            //Debug.Log("Simumlation: day" + daySim + ", Sickcnt=" + averageSimResults[daySim] + ",Percentage= " + averageSimResults[daySim]*100/nodeCnt + "%");
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
            firstSickIndex = sickIndex;


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
            for (int i = 0; i < nodeCnt; i++) 
                if (nodes[i].GetComponent<node>().currentStatus == global::node.nodeStatus.Quaratine) nodes[i].GetComponent<node>().currentStatus = nodes[i].GetComponent<node>().lastStatus;

        }
            



        }
    
     public void superSimulate()
    {
        writeFile superSimuRecord = new writeFile(init.Instance.dataFolder + "/" + System.DateTime.Now.ToString("HHmm") + "superSimu.txt");
        superSimuRecord.appendData("network, simulation, day, sickPercentage, border_red_cnt, border_green_cnt,edgeCnt, firstSickNeighbourCnt, maxNeighbourCnt,midNeighbourCnt,quaterHighNeighbourCnt,averageNeighbourCnt ");
        int networkCnt = 100;
        int simuCntPerNetwork = 50;
        int midNeighbourCnt;
        int maxNeighbourCnt;
        int quaterHighNeighbourCnt;
        float averageNeighbourCnt;
        for (int iNetwork = 0; iNetwork < networkCnt; iNetwork++)
        {
            Debug.Log("simu network:" + iNetwork);
            init.Instance.reNetwork();
            simInit(); // Initialize simulation 
            backToDayNow();
            init.Instance.calcStats(out maxNeighbourCnt, out midNeighbourCnt, out quaterHighNeighbourCnt, out averageNeighbourCnt);
            for (int iSim = 0; iSim < simuCntPerNetwork; iSim++)
            {
                for (; daycnt < totaldays - 1;) 
                {


                    nextdaySim();
                    int borderRed, borderGreen;
                    calcBorder(out borderRed, out borderGreen);
                    superSimuRecord.appendData("" + iNetwork + "," + iSim + "," + daycnt + "," + 1f * sickCnt / nodeCnt + "," + borderRed + "," + borderGreen + "," + edgeCnt + "," + nodes[firstSickIndex].GetComponent<node>().neighbourCnt + "," 
                        + maxNeighbourCnt +"," + midNeighbourCnt + "," +quaterHighNeighbourCnt +"," + averageNeighbourCnt) ;




                }
                backToDayNow();
            }

        }

    }
}
