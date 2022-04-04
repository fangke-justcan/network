using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class node : MonoBehaviour
{

    public float production = 1.0f;
    public float productionRate = 1.0f;
    public int index;
    public int neighbourCnt=0;
    public int[] neighbours;
    public TextMesh indexName;
    public int sickDays = 0;
  
    public enum nodeStatus { Normal, Sick , Quaratine, Vaccinated};
    public enum nodeProductionStatus { Normal, Supply, Research };
    public bool sickDetcted = false;
    public bool normalDected = false; 
    public nodeStatus currentStatus = nodeStatus.Normal;
    public nodeStatus lastStatus = nodeStatus.Normal;
    public nodeProductionStatus currentProductionStatus = nodeProductionStatus.Normal;
    // Start is called before the first frame update


 

    void Start()
    {
        indexName.text = "" + index;

    }

    // Update is called once per frame
    void Update()
    {
        if (init.Instance.currentGameStarus != init.gameStatus.play) return;
        if (currentProductionStatus == nodeProductionStatus.Normal) indexName.text = "" + index;
        else if (currentProductionStatus == nodeProductionStatus.Supply) indexName.text = "   " + index + "医";
        else if (currentProductionStatus == nodeProductionStatus.Research) indexName.text = "   " + index + "研";

        if (currentStatus == nodeStatus.Sick && sickDetcted) { GetComponent<SpriteRenderer>().color = Color.red; productionRate = 0.15f; }
        if (currentStatus == nodeStatus.Sick && !sickDetcted) { GetComponent<SpriteRenderer>().color = Color.blue; productionRate = 0.5f; }
        if (currentStatus == nodeStatus.Normal && !normalDected) { GetComponent<SpriteRenderer>().color = Color.blue; productionRate = 1f; }
        if (currentStatus == nodeStatus.Normal && normalDected) { GetComponent<SpriteRenderer>().color = Color.green; productionRate = 1f; }
        if (currentStatus == nodeStatus.Quaratine) { GetComponent<SpriteRenderer>().color = Color.black; productionRate = 0f; }
        if (currentStatus == nodeStatus.Vaccinated) { GetComponent<SpriteRenderer>().color = Color.yellow; productionRate = 1f; }
        if (neighbourCnt > 4) transform.localScale = new Vector3(3, 3, 1);
        else if (neighbourCnt > 2) transform.localScale = new Vector3(2f, 2f, 1);


        production = Mathf.Pow(1f * neighbourCnt, 0.8f);



    }

    public void changeNodeType(nodeProductionStatus newNodeType)
    {
        currentProductionStatus = newNodeType;
        
    }
}
