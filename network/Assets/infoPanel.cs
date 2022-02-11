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
    public Text sickOrder;
    public Toggle SickOrderToggle;

    // Update is called once per frame
    void Update()
    {
        if (init.Instance.currentGameStarus != init.gameStatus.play) return;
        int circlingCnt = 0;
        for (int i  = 0; i< init.Instance.nodeCnt;i++)
        {
            if (init.Instance.nodes[i].GetComponent<RandomMove>().currentMoveStatus == RandomMove.moveStatus.circle) circlingCnt++;
        }
        daycnt.text = "Day: " + init.Instance.showdaycnt;
        sickpercentage.text = "Sick:" + init.Instance.sickCnt+ "people out of total "+ init.Instance.nodeCnt+ " people, sick percentage:" + init.Instance.sickPercentage*100 +"%";
        quarantinecnt.text = "Quarantine Left: " + init.Instance.quarantineCnt;
        testcnt.text = "Test Left: " + init.Instance.testCnt + "\n circling node:" + circlingCnt;


    }
    public void outputSickOrder()
    {
        sickOrder.text = "";
        if (SickOrderToggle.isOn)
        {
           
            for (int i = 0; i < init.Instance.sickCnt; i++)
                sickOrder.text += ("" + init.Instance.sickOrder[i, 0] + " get sick at " + init.Instance.sickOrder[i, 1] + "\n");
        }
     
    }
}
