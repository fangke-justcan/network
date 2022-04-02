using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class init : MonoBehaviour
{
    private static init _instance;

    public static init Instance
    {
        get
        {
            if (_instance == null)
            {
                GameObject instance = new GameObject("init");
                instance.AddComponent<init>();
                
            }

            return _instance;
        }
    }

    void Awake()
    {
        _instance = this;
        currentGameStarus = gameStatus.init;
    }

    public int totaldays = 30;
    public int nodeCnt = 50;
    public float Pconnect = 0.05f;
    public float PconnectEdge = 0.01f;
    public float Psick = 0.2f;
    public initMethod thisInit;
    public Texture2D cursorTextureQuarantine;
    public Texture2D cursorTextureTest;
    public CursorMode cursorMode = CursorMode.Auto;
    public Vector2 hotSpot = Vector2.zero;
    public GameObject nextdayButton;
    public simulation sim;

    
    public GameObject node,edge;
    [HideInInspector]
    public GameObject[] nodes,edges;
    public enum initMethod { randomNode, randomEdge };
    public enum showMode { today , past};
    public enum gameStatus { init, play};
    [HideInInspector]
    public showMode currentShowMode = showMode.today; 
    public enum cursorState { normal, quarantine, test };
    [HideInInspector]
    public cursorState currentCursor = cursorState.normal;
    [HideInInspector]
    public int quarantineCnt = 0, testCnt = 0, daycnt = 0, showdaycnt = 0, edgeCnt = 0, sickCnt = 0;
    [HideInInspector]
    public float sickPercentage;
    [HideInInspector]
    public int[,] sickOrder;
    [HideInInspector]
    public gameStatus currentGameStarus; 
    protected bool[,] sickDetectedHistory;
    protected bool[,] normalDetectedHistory;
    protected node.nodeStatus[,] currentStatusHistory;
    protected int[] sickCntHistory;
    protected Vector3 a;
    [HideInInspector]
    public string dataFolder;
  
    // Start is called before the first frame update
    void Start()
    {
        nodes = new GameObject[nodeCnt];
        edges = new GameObject[nodeCnt * (nodeCnt - 1) / 2];
        sickOrder = new int[nodeCnt, 2];   // [i,0] = sickPersonIndex  [i,1] = dayOfSick 
        sickDetectedHistory = new bool[nodeCnt, totaldays];
        normalDetectedHistory = new bool[nodeCnt, totaldays];
        currentStatusHistory = new node.nodeStatus[nodeCnt, totaldays];
        sickCntHistory = new int[totaldays]; 
        if (thisInit == initMethod.randomNode) init_randomNode();
        else init_randomEdge();
        sim.startSimulation();
        // create data dir
        dataFolder = "dataRecord/" + System.DateTime.Now.ToString("MMddHHmm");
        System.IO.Directory.CreateDirectory(dataFolder);
        quarantineCnt = 0; testCnt = 0;

    }

 
    void destroyNodesEdges()
    {
        for (int i = 0; i < nodeCnt; i++) Destroy(nodes[i]);
        for (int i = 0; i < edgeCnt; i++) Destroy(edges[i]);

    }

    void init_randomEdge()
    {
        for (int i = 0; i < nodeCnt; i++)   //instantiate the nodes 
        {
            a.x = Random.Range(-5f, 5f); a.y = Random.Range(-5f, 5f); a.z = 0;
            nodes[i] = Instantiate(node, a, Quaternion.identity);
            nodes[i].GetComponent<node>().index = i;



        }

        /*
        edges[0] = Instantiate(edge);
        edges[0].GetComponent<edgeVertex>().v1 = nodes[0];
        edges[0].GetComponent<edgeVertex>().v2 = nodes[1];
        nodes[i].GetComponent<node>().neighbours[nodes[i].GetComponent<node>().neighbourCnt] = j;
        nodes[i].GetComponent<node>().neighbourCnt++;
        nodes[j].GetComponent<node>().neighbours[nodes[j].GetComponent<node>().neighbourCnt] = i;
        nodes[j].GetComponent<node>().neighbourCnt++;
        edgeCnt++;
        */
        for (int i = 0; i < nodeCnt; i++)    // add edges - random network - each nodes has a Pconnect probability to connect to every other node
        {
            for (int j = i + 1; j < nodeCnt; j++)
            {
                if (Random.Range(0f, 1f) < Pconnect * ((nodes[i].GetComponent<node>().neighbourCnt + 1)))
                {
                    edges[edgeCnt] = Instantiate(edge);
                    edges[edgeCnt].GetComponent<edgeVertex>().v1 = nodes[i];
                    edges[edgeCnt].GetComponent<edgeVertex>().v2 = nodes[j];
                    nodes[i].GetComponent<node>().neighbours[nodes[i].GetComponent<node>().neighbourCnt] = j;
                    nodes[i].GetComponent<node>().neighbourCnt++;
                    nodes[j].GetComponent<node>().neighbours[nodes[j].GetComponent<node>().neighbourCnt] = i;
                    nodes[j].GetComponent<node>().neighbourCnt++;
                    edgeCnt++;

                }
            }
        }


    }


    void init_randomNode()
    {

        for (int i = 0; i < nodeCnt; i++)   //instantiate the nodes 
        {
            a.x = Random.Range(-5f, 5f); a.y = Random.Range(-5f, 5f); a.z = 0;
            nodes[i] = Instantiate(node, a, Quaternion.identity);
            nodes[i].GetComponent<node>().index = i;
            nodes[i].GetComponent<node>().neighbours = new int[nodeCnt];
            nodes[i].GetComponent<node>().neighbourCnt = 0;



        }


        for (int i = 0; i < nodeCnt; i++)    // add edges - random network - each nodes has a Pconnect probability to connect to every other node
        {
            for (int j = i + 1; j < nodeCnt; j++)
            {
                if (Random.Range(0f, 1f) < Pconnect)
                {
                    edges[edgeCnt] = Instantiate(edge);
                    edges[edgeCnt].GetComponent<edgeVertex>().v1 = nodes[i];
                    edges[edgeCnt].GetComponent<edgeVertex>().v2 = nodes[j];
                    nodes[i].GetComponent<node>().neighbours[nodes[i].GetComponent<node>().neighbourCnt] = j;
                    nodes[i].GetComponent<node>().neighbourCnt++;
                    nodes[j].GetComponent<node>().neighbours[nodes[j].GetComponent<node>().neighbourCnt] = i;
                    nodes[j].GetComponent<node>().neighbourCnt++;
                    edgeCnt++;

                }
            }
        }
    }

 

    public void reNetwork()
    {
        for (int i = 0; i < edgeCnt; i++) Destroy(edges[i]);
        for (int i = 0; i < nodeCnt; i++) nodes[i].GetComponent<node>().neighbourCnt = 0;
        edgeCnt = 0;
        for (int i = 0; i < nodeCnt; i++)    // rearrange edges - random network - each nodes has a Pconnect probability to connect to every other node
        {
            
            for (int j = i + 1; j < nodeCnt; j++)
            {
                if (Random.Range(0f, 1f) < Pconnect)
                {
                    edges[edgeCnt] = Instantiate(edge);
                    edges[edgeCnt].GetComponent<edgeVertex>().v1 = nodes[i];
                    edges[edgeCnt].GetComponent<edgeVertex>().v2 = nodes[j];
                    nodes[i].GetComponent<node>().neighbours[nodes[i].GetComponent<node>().neighbourCnt] = j;
                    nodes[i].GetComponent<node>().neighbourCnt++;
                    nodes[j].GetComponent<node>().neighbours[nodes[j].GetComponent<node>().neighbourCnt] = i;
                    nodes[j].GetComponent<node>().neighbourCnt++;
                    edgeCnt++;

                }
            }
        }
    }

    public void nextDay()     // next day , events and infections 
    {
        save(daycnt);
        daycnt++; showdaycnt = daycnt;
        if (sickCnt == 0)   // the first day 
        {

            int failedFirstSick = 0;
            while (failedFirstSick >=0)
            {
                int sickIndex = Random.Range(0, nodeCnt - 1);
                nodes[sickIndex].GetComponent<node>().currentStatus = global::node.nodeStatus.Sick;
                nodes[sickIndex].GetComponent<node>().sickDays++;
                sickOrder[sickCnt, 0] = sickIndex; sickOrder[sickCnt, 1] = daycnt;
                sickCnt++;

                sim.startSimulation();
                if (sim.averageSimResults[10]/nodeCnt > 0.6 && sim.averageSimResults[10]/nodeCnt < 0.75) failedFirstSick = -100;
                else { clearSick(); Debug.Log( "try"+ failedFirstSick +"end: " + sim.averageSimResults[10] / nodeCnt); failedFirstSick++; }
                if (failedFirstSick >= 10) 
                { 
                    Debug.Log("Failed first Sick, pls change the config"); 
                    
                    break; 
                }

            }


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
                        sickOrder[sickCnt, 0] = nextSick[i]; sickOrder[sickCnt, 1] = daycnt;
                        sickCnt++;
                    }
                }
            } // update the sick nodes for next day 


            for (int i = 0; i < nodeCnt; i++)   //reset Quanratine and normaldected 
            {
                nodes[i].GetComponent<node>().normalDected = false;
                if (nodes[i].GetComponent<node>().currentStatus == global::node.nodeStatus.Quaratine)
                {
                    // Quaratine status has a small chance to remain red, but in most cases they will turn green! 

                    if (Random.Range(0f,1f) < 0.2)
                    {
                        
                        global::node.nodeStatus a = nodes[i].GetComponent<node>().lastStatus;
                        nodes[i].GetComponent<node>().currentStatus = nodes[i].GetComponent<node>().lastStatus;
                    

                    }

                    else
                    {
                        if (nodes[i].GetComponent<node>().lastStatus == global::node.nodeStatus.Sick) sickCnt--;
                        nodes[i].GetComponent<node>().currentStatus = global::node.nodeStatus.Normal;
                        nodes[i].GetComponent<node>().lastStatus = global::node.nodeStatus.Normal;
                        nodes[i].GetComponent<node>().normalDected = true;
                        nodes[i].GetComponent<node>().sickDetcted = false;
                        nodes[i].GetComponent<node>().sickDays = 0;


                    }
                    

                }

            }
            for (int i = 0; i < nodeCnt; i++)
            {
                if (nodes[i].GetComponent<node>().currentStatus == global::node.nodeStatus.Sick)
                {
                    nodes[i].GetComponent<node>().sickDays++;
                    if (nodes[i].GetComponent<node>().sickDays >1 && sickCnt > 2 )
                    {
                        if (Random.Range(0f, 1f) >   0.5-0.1* nodes[i].GetComponent<node>().sickDays) nodes[i].GetComponent<node>().sickDetcted = true;  // after two days , the sick has a increasing chance to reveal the sick
                    }
                }   
            }
            sickPercentage = 1f * sickCnt / nodeCnt;
            
           




        }
        

        int greenBorder = 0, redBorder = 0;
        sim.simInit();
        sim.backToDayNow();
        sim.calcBorder(out redBorder, out greenBorder);
        
        if (sickPercentage >= 0.05) { quarantineCnt = 0; testCnt = 3; }
        if (sickPercentage >= 0.10) { quarantineCnt = 1; testCnt = 4; }
        if (sickPercentage >= 0.20) { quarantineCnt = 2; testCnt = 6; }
        if (sickPercentage >= 0.30) { quarantineCnt = 3; testCnt = 6; }
        if (sickPercentage >= 0.50) { quarantineCnt = 4; testCnt = 6; }
       /*
        if (quarantineCnt< redBorder / 2) quarantineCnt = redBorder / 2;
        if (sickPercentage < 0.25 )
        {
            if (testCnt < (greenBorder+redBorder) / 2) testCnt = (greenBorder+redBorder) / 2;


        }
        else { if (testCnt < (greenBorder) / 2) testCnt = (greenBorder) / 2; }
       */


        Debug.Log("Now, it is Day:" + daycnt + " redB: " + redBorder + " grennB: " + greenBorder);

        
    }

    public void relocate()   // random new location, network does not change. 
    {
        float Eedgecnt = Pconnect * nodeCnt; // average neighbour count 
        Vector3 a;
        for (int i = 0; i < nodeCnt; i++)
        {


            int nc = nodes[i].GetComponent<node>().neighbourCnt;

            if (nc > 1.5 * Eedgecnt)      // node with more neighbous at the center 
            {
                float thelta = Random.Range(0f, 2 * Mathf.PI);
                float r = Random.Range(1f, 1f);
                a.x = r * Mathf.Cos(thelta);
                a.y = r * Mathf.Sin(thelta);
                a.z = 0;
            }
            else if (nc > 0.8f * Eedgecnt)      // node with medium neighbous  
            {

                float thelta = Random.Range(0f, 2 * Mathf.PI);
                float r = Random.Range(1.5f, 1.5f);
                a.x = r * Mathf.Cos(thelta);
                a.y = r * Mathf.Sin(thelta);
                a.z = 0;

            }
            else    // node with little neighbous  
            {

                float thelta = Random.Range(0f, 2 * Mathf.PI);
                float r = Random.Range(2f, 2f);
                a.x = r * Mathf.Cos(thelta);
                a.y = r * Mathf.Sin(thelta);
                a.z = 0;

            }
            nodes[i].transform.position = a;

        }


    }



    public void reshape()    // node position at the center of their neighbours 
    {
        float Eedgecnt = Pconnect * nodeCnt;

        for (int i = 0; i < nodeCnt; i++)
        {

            int nc = nodes[i].GetComponent<node>().neighbourCnt;

            if (nc > 0.8 * Eedgecnt && nc <= 1.5 * Eedgecnt && nc > 1)
            {
                Vector3 center = Vector3.zero;
                for (int j = 0; j < nc; j++)
                {
                    center += nodes[nodes[i].GetComponent<node>().neighbours[j]].transform.position;
                }
                center /= nc;
                nodes[i].transform.position = center;
            }

        }
        for (int i = 0; i < nodeCnt; i++)
        {

            int nc = nodes[i].GetComponent<node>().neighbourCnt;

            if (nc > 1.5 * Eedgecnt && nc > 1)
            {
                Vector3 center = Vector3.zero;
                for (int j = 0; j < nc; j++)
                {
                    center += nodes[nodes[i].GetComponent<node>().neighbours[j]].transform.position;
                }
                center /= nc;
                nodes[i].transform.position = center;
            }

        }

        /*
        for (int i = 0; i < nodeCnt; i++)
        {

            int nc = nodes[i].GetComponent<node>().neighbourCnt;

            if (nc <= 0.8 * Eedgecnt && nc > 1)
            {
                Vector3 center = Vector3.zero;
                for (int j = 0; j < nc; j++)
                {
                    center += nodes[nodes[i].GetComponent<node>().neighbours[j]].transform.position;
                }
                center /= nc;
                nodes[i].transform.position = center;
            }

        }
        */

        for (int i = 0; i < nodeCnt; i++)
        {

            int nc = nodes[i].GetComponent<node>().neighbourCnt;

            if (nc == 1)
            {
                Vector3 a;
                float thelta = Random.Range(0f, 2 * Mathf.PI);
                float r = 0.5f;
                a.x = r * Mathf.Cos(thelta);
                a.y = r * Mathf.Sin(thelta);
                a.z = 0;
                nodes[i].transform.position = a + nodes[nodes[i].GetComponent<node>().neighbours[0]].transform.position;
            }

        }
    }


    public void zoomout()
    {
        for (int i = 0; i < nodeCnt; i++)
        {
            Vector3 a;
            a.x = nodes[i].transform.position.x * 0.95f;
            a.y = nodes[i].transform.position.y * 0.95f;
            a.z = 0;
            nodes[i].transform.position = a;
        }
    }

    public void zoomin()
    {
        for (int i = 0; i < nodeCnt; i++)
        {
            Vector3 a;
            a.x = nodes[i].transform.position.x * 1.05f;
            a.y = nodes[i].transform.position.y * 1.05f;
            a.z = 0;
            nodes[i].transform.position = a;
        }
    }
    // Update is called once per frame
    void Update()
    {
        sickPercentage = 1f * sickCnt / nodeCnt;
        if (Input.GetKeyDown(KeyCode.Escape)) Application.Quit();
        if (Input.GetKey(KeyCode.C))
        {
            Cursor.SetCursor(cursorTextureQuarantine, hotSpot, cursorMode);
            currentCursor = cursorState.quarantine;

        }
        else if (Input.GetKey(KeyCode.T))
        {
            Cursor.SetCursor(cursorTextureTest, hotSpot, cursorMode);
            currentCursor = cursorState.test;
        }
        else
        {
            Cursor.SetCursor(null, Vector2.zero, cursorMode);
            currentCursor = cursorState.normal;
        }





    }

    public void clearSick()
    {
        daycnt = 0;
        showdaycnt = 0;
        quarantineCnt = 0;
        testCnt = 0;
        for (int i = 0; i < nodeCnt; i++)
        {
            nodes[i].GetComponent<node>().currentStatus = global::node.nodeStatus.Normal;
            nodes[i].GetComponent<node>().sickDetcted = false;
            nodes[i].GetComponent<node>().normalDected = false;
            nodes[i].GetComponent<node>().sickDays = 0;

        }
        sickCnt = 0;
    }
    

    public void lookBack()
    {
        if (currentShowMode == showMode.today )
        {  
            save(daycnt);
            nextdayButton.SetActive(false);
            Camera.main.backgroundColor = new Color(0.3f,0.3f,0.3f);
            currentShowMode = showMode.past;
        }

        showdaycnt--;
        if (showdaycnt < 0) showdaycnt = 0;
        load(showdaycnt);
    }

    public void lookforward()
    {
        
        showdaycnt++;
        if (showdaycnt >= daycnt) 
        { 
            showdaycnt = daycnt; 
            nextdayButton.SetActive(true);
            Camera.main.backgroundColor = Color.grey;
            currentShowMode = showMode.today;
        }
        load(showdaycnt);
    }


    void save( int day)
    {
        for (int i =0; i< nodeCnt; i++)
        {
            sickDetectedHistory[i,day] = nodes[i].GetComponent<node>().sickDetcted;

            normalDetectedHistory[i,day] = nodes[i].GetComponent<node>().normalDected;
            currentStatusHistory[i,day] = nodes[i].GetComponent<node>().currentStatus;
            
        }
        sickCntHistory[day] = sickCnt;
    }

    void load(int day)
    {
        for (int i = 0; i < nodeCnt; i++)
        {
            nodes[i].GetComponent<node>().sickDetcted = sickDetectedHistory[i, day];

            nodes[i].GetComponent<node>().normalDected = normalDetectedHistory[i, day] ;
            nodes[i].GetComponent<node>().currentStatus = currentStatusHistory[i, day];

        }
        sickCnt = sickCntHistory[day];
    }



    public void calcStats(out int maxNeighbourCnt, out int midNeighbourCnt, out int quaterHighNeighbourCnt, out float averageNeighbourCnt )
    {
        

        List<int> sortedNeighbourCnt = new List<int>(nodeCnt);

        averageNeighbourCnt = 0;
        for (int i =0; i<nodeCnt; i++)
        {
            sortedNeighbourCnt.Add(nodes[i].GetComponent<node>().neighbourCnt);
            averageNeighbourCnt += sortedNeighbourCnt[i];

        }
        sortedNeighbourCnt.Sort();
        maxNeighbourCnt = sortedNeighbourCnt[nodeCnt - 1];
        midNeighbourCnt = sortedNeighbourCnt[nodeCnt / 2];
        quaterHighNeighbourCnt = sortedNeighbourCnt[nodeCnt * 3 / 4];
        averageNeighbourCnt /= nodeCnt;


    }

}

