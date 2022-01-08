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
    // Start is called before the first frame update
    void Start()
    {
        
        nodes = new GameObject[nodeCnt];
        edges = new GameObject[nodeCnt*(nodeCnt-1)/2];
        for (int i = 0;i < nodeCnt; i++)
        {
            a.x = Random.Range(-5f, 5f); a.y = Random.Range(-5f, 5f); a.z = 0;
            nodes[i] = Instantiate(node,a,Quaternion.identity);
            nodes[i].GetComponent<node>().index = i;
            
        

        }

        
        for(int i =0; i< nodeCnt; i++)
        {
            for (int j = i+1;j< nodeCnt; j++)
            {
                if (Random.Range(0f,1f)<0.1)
                {
                    edges[edgeCnt] = Instantiate(edge);
                    edges[edgeCnt].GetComponent<edgeVertex>().v1 =  nodes[i];
                    edges[edgeCnt].GetComponent<edgeVertex>().v2 = nodes[j];
                    int k = nodes[i].GetComponent<node>().neighbourCnt;
                    int[] a = nodes[i].GetComponent<node>().neighbours;
                    nodes[i].GetComponent<node>().neighbours[nodes[i].GetComponent<node>().neighbourCnt] = j;
                    nodes[i].GetComponent<node>().neighbourCnt++;
                    nodes[j].GetComponent<node>().neighbours[nodes[j].GetComponent<node>().neighbourCnt] = i;
                    nodes[j].GetComponent<node>().neighbourCnt++;

                    edgeCnt++;

                }
            }
        }

        
        
    }

    public void nextDay()
    {
        if (sickCnt == 0)
        {
            nodes[Random.Range(0, nodeCnt-1)].GetComponent<node>().currentStatus = global::node.nodeStatus.Sick;
            sickCnt++;
        }
        else
        {
            for (int i =0; i< nodeCnt; i++)
            {
                if (nodes[i].GetComponent<node>().currentStatus == global::node.nodeStatus.Sick)
                {
                    for (int j = 0; j< nodes[i].GetComponent<node>().neighbourCnt; j++)
                    {
                        if (Random.Range(0f,1f)< 0.2) nodes[nodes[i].GetComponent<node>().neighbours[j]].GetComponent<node>().currentStatus = global::node.nodeStatus.Sick; 
                    }
                }
            }
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
