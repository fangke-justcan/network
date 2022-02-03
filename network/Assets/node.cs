using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class node : MonoBehaviour
{
    public int index;
    public int neighbourCnt=0;
    public int[] neighbours;
    public TextMesh indexName;
    public enum nodeStatus { Normal, Sick , Quaratine};
    public bool sickDetcted = false;
    public bool normalDected = false; 
    public nodeStatus currentStatus = nodeStatus.Normal;
    public nodeStatus lastStatus = nodeStatus.Normal;
    // Start is called before the first frame update
    void Awake()
    {
        neighbours = new int[init.Instance.nodeCnt];
        neighbourCnt = 0;
}

    // Update is called once per frame
    void Update()
    {
        indexName.text = "" + index;
        if (currentStatus == nodeStatus.Sick && sickDetcted) GetComponent<SpriteRenderer>().color = Color.red;
        if (currentStatus == nodeStatus.Sick && !sickDetcted) GetComponent<SpriteRenderer>().color = Color.blue;
        if (currentStatus == nodeStatus.Normal && !normalDected) GetComponent<SpriteRenderer>().color = Color.blue;
        if (currentStatus == nodeStatus.Normal && normalDected) GetComponent<SpriteRenderer>().color = Color.green;
        if (currentStatus == nodeStatus.Quaratine) GetComponent<SpriteRenderer>().color = Color.black;

    }
}
