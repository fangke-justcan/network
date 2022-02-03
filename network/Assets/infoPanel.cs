using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class infoPanel : MonoBehaviour
{
    public Text daycnt;
    public Text sickpercentage;
    public Text quarantinecnt;
    public Text testcnt;

    // Update is called once per frame
    void Update()
    {
        daycnt.text = "Day: " + init.Instance.daycnt;
        sickpercentage.text = "Sick:" + init.Instance.sickCnt+ "people out of total "+ init.Instance.nodeCnt+ " people, sick percentage:" + init.Instance.sickPercentage*100 +"%";
        quarantinecnt.text = "Quarantine Left: " + init.Instance.quarantineCnt;
        testcnt.text = "Test Left: " + init.Instance.testCnt;


    }
}
