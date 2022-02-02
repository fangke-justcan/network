using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class infoPanel : MonoBehaviour
{
    public Text daycnt;
    public Text sickpercentage;

    // Update is called once per frame
    void Update()
    {
        daycnt.text = "Day: " + init.Instance.daycnt;
        sickpercentage.text = "SickPercentage: " + init.Instance.sickPercentage*100 + "%";
    }
}
