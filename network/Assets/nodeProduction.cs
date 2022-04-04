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
        NPtext.GetComponent<Text>().text = "Node:" + GetComponent<node>().index + "\nFull Production: " + GetComponent<node>().production*100f/init.Instance.totalProductionNetwork;
        if (GetComponent<node>().normalDected == true || GetComponent<node>().sickDetcted == true)

        {
            float realP = GetComponent<node>().production * 100f / init.Instance.totalProductionNetwork * GetComponent<node>().productionRate;
            NPtext.GetComponent<Text>().text += "\nReal Production: " + realP;

        } 
        else {
            NPtext.GetComponent<Text>().text += "\nReal Production: ???";
        }

    }

    private void OnMouseExit()
    {
        NPtext.GetComponent<Text>().text = "";
    }

}
