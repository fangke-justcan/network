using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    }

    public GameObject node;
    public GameObject edge;
    public GameObject[] nodes;
    public GameObject[] edges;
    Vector3 a;
    public int edgeCnt = 0;
    public int sickCnt = 0;
    public int nodeCnt = 50;
    public float Pconnect = 0.05f;
    public float Psick = 0.2f; 
    // Start is called before the first frame update
    void Start()
    {
        
        nodes = new GameObject[nodeCnt];
        edges = new GameObject[nodeCnt*(nodeCnt-1)/2];
        for (int i = 0;i < nodeCnt; i++)   //instantiate the nodes 
        {
            a.x = Random.Range(-5f, 5f); a.y = Random.Range(-5f, 5f); a.z = 0;
            nodes[i] = Instantiate(node,a,Quaternion.identity);
            nodes[i].GetComponent<node>().index = i;
            
        

        }

        
        for(int i =0; i< nodeCnt; i++)    // add edges - random network - each nodes has a Pconnect probability to connect to every other node
        {
            for (int j = i+1;j< nodeCnt; j++)
            {
                if (Random.Range(0f,1f)< Pconnect)
                {
                    edges[edgeCnt] = Instantiate(edge);
                    edges[edgeCnt].GetComponent<edgeVertex>().v1 =  nodes[i];
                    edges[edgeCnt].GetComponent<edgeVertex>().v2 = nodes[j];
                    nodes[i].GetComponent<node>().neighbours[nodes[i].GetComponent<node>().neighbourCnt] = j;
                    nodes[i].GetComponent<node>().neighbourCnt++;
                    nodes[j].GetComponent<node>().neighbours[nodes[j].GetComponent<node>().neighbourCnt] = i;
                    nodes[j].GetComponent<node>().neighbourCnt++;
                    edgeCnt++;

                }
            }
        }
        relocate();

        
        
    }

    public void nextDay()     // next day , events and infections 
    {
        if (sickCnt == 0)   // the first day 
        {
            nodes[Random.Range(0, nodeCnt-1)].GetComponent<node>().currentStatus = global::node.nodeStatus.Sick;
            sickCnt++;
        }
        else
        {
            int[] nextSick = new int[nodeCnt];
            int nextSickcnt = 0;
            for (int i =0; i< nodeCnt; i++)      // each sick node has a Psick probability to infect its neighbour. 
            {
                if (nodes[i].GetComponent<node>().currentStatus == global::node.nodeStatus.Sick)
                {
                    for (int j = 0; j< nodes[i].GetComponent<node>().neighbourCnt; j++)
                    {
                        if (Random.Range(0f, 1f) < Psick) 
                        {
                            if (nodes[nodes[i].GetComponent<node>().neighbours[j]].GetComponent<node>().currentStatus != global::node.nodeStatus.Sick)
                            {
                                nextSick[nextSickcnt] = nodes[i].GetComponent<node>().neighbours[j];
                                nextSickcnt++;
                                sickCnt++;
                            } 
                        }
                    }
                }
            }
            for (int i =0; i<nextSickcnt; i++) nodes[nextSick[i]].GetComponent<node>().currentStatus = global::node.nodeStatus.Sick;    // update the sick nodes for next day 
        }
    }

    public void relocate()   // random new location, network does not change. 
    {
        float Eedgecnt = Pconnect * nodeCnt; // average neighbour count 
        Vector3 a;
        for (int i = 0; i < nodeCnt; i++)
        {
            
            
            int nc = nodes[i].GetComponent<node>().neighbourCnt;

            if (nc >  1.5 *Eedgecnt)      // node with more neighbous at the center 
            {
                float thelta = Random.Range(0f, 2 * Mathf.PI);
                float r = Random.Range(1f, 1f);
                a.x = r * Mathf.Cos(thelta);
                a.y = r * Mathf.Sin(thelta);
                a.z = 0;
            }
            else if (nc > 0.8f*Eedgecnt)      // node with medium neighbous  
            {

                float thelta = Random.Range(0f, 2 * Mathf.PI);
                float r = Random.Range(3f, 3f);
                a.x = r * Mathf.Cos(thelta);
                a.y = r * Mathf.Sin(thelta);
                a.z = 0;

            }
            else    // node with little neighbous  
            {

                float thelta = Random.Range(0f, 2 * Mathf.PI);
                float r = Random.Range(5f, 5f);
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

            if (nc > 0.8 * Eedgecnt && nc <= 1.5 * Eedgecnt && nc>1)
            {
               Vector3 center = Vector3.zero;
               for (int j =0; j< nc; j++)
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
                nodes[i].transform.position = a+ nodes[nodes[i].GetComponent<node>().neighbours[0]].transform.position;
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

    }

    public void clearSick()
    {
        for (int i = 0; i<nodeCnt; i++)
        {
            nodes[i].GetComponent<node>().currentStatus = global::node.nodeStatus.Normal;
        }
        sickCnt = 0;
    }
}
