using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class nodeProduction : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject NPtext;

    private void Start()
    {
        NPtext = GameObject.FindWithTag("NProduction");
    }

    private void OnMouseOver()
    {
       

        NPtext.GetComponent<Text>().text = "Node:" + GetComponent<node>().index + "\n节点产能: " + (GetComponent<node>().production*100f/init.Instance.totalProductionNetwork).ToString("f1")+ "万/套/" + (GetComponent<node>().production * 100f / init.Instance.totalProductionNetwork *100/ init.Instance.ResearchFinish).ToString("f1") + "%";
        if (GetComponent<node>().normalDected == true || GetComponent<node>().sickDetcted == true)

        {
            float realP = GetComponent<node>().production * 100f / init.Instance.totalProductionNetwork * GetComponent<node>().productionRate;
            NPtext.GetComponent<Text>().text += "\n节点真实产能: " + realP.ToString("f1") + "万/套/" + (realP * 100 / init.Instance.ResearchFinish).ToString("f1") + "%";

        } 
        else {
            NPtext.GetComponent<Text>().text += "\n节点真实产能:  ???";
        }

    }

    private void OnMouseExit()
    {
        NPtext.GetComponent<Text>().text = "";
    }

}
