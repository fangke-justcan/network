using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class infoPanel : MonoBehaviour
{
    public Text daycnt;
    public Text sickpercentage;
    public Text quarantinecnt;
    public Text testcnt;
    public Text sickOrder;
    public Toggle SickOrderToggle;
    public InputField SupplyCity;
    public InputField ResearchCity;


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
        //sickpercentage.text = "Sick:" + init.Instance.sickCnt+ "people out of total "+ init.Instance.nodeCnt+ " people, sick percentage:" + init.Instance.sickPercentage*100 +"%";
        sickpercentage.text = "总生产力: " + init.Instance.totalProduction.ToString("f1") + "万\n预估规模疫情城市数: " +init.Instance.sickCnt;
       
        sickpercentage.text += "\n特殊经费等级: " + init.Instance.specialFundLevelRate[init.Instance.specialFundLevel]*100 +"%总生产力" ;
        sickpercentage.text += "\n特殊经费增量: " + (init.Instance.specialFundLevelRate[init.Instance.specialFundLevel] * init.Instance.totalProduction).ToString("f1") + "万";
        sickpercentage.text += "\n特殊经费累计: " + init.Instance.specialFund.ToString("f1") + "万";

        sickpercentage.text += "\n\n医疗物资生产力: " + init.Instance.totalMedicalSupplyProduction.ToString("f0") + "套";
        sickpercentage.text += "\n医疗物资: " + init.Instance.medicalSupply.ToString("f0") + "套";
        sickpercentage.text += "\n\n疫苗研究生产力: " + init.Instance.totalVaccineResearchProduction.ToString("f0") + "批次实验";
        sickpercentage.text += "\n疫苗累计进展: " + init.Instance.vaccineProgress.ToString("f0") + "批次实验";

        quarantinecnt.text = "" + init.Instance.quarantineCnt;
        testcnt.text = "" + init.Instance.testCnt + "\n circling node:" + circlingCnt;


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



    public void onSupplyButton()
    {
        if (SupplyCity.interactable == false) return;
        if (!int.TryParse(SupplyCity.text, out int index)) return;
        if (index < 0 || index >= init.Instance.nodeCnt) return;
        if (init.Instance.specialFund >= init.Instance.MSCityConstruction)
        {
            init.Instance.nodes[index].GetComponent<node>().changeNodeType(node.nodeProductionStatus.Supply);
            SupplyCity.interactable = false;
            init.Instance.specialFund -= init.Instance.MSCityConstruction;
        }
        
    }
    public void onResearchButton()
    {
        if (ResearchCity.interactable == false) return;
        if (!int.TryParse(ResearchCity.text, out int index))  return;
        if (index < 0 || index >= init.Instance.nodeCnt) return;
        if (init.Instance.specialFund >= init.Instance.VRCityConstruction)
        {
            init.Instance.nodes[index].GetComponent<node>().changeNodeType(node.nodeProductionStatus.Research);
            ResearchCity.interactable = false;
            init.Instance.specialFund -= init.Instance.VRCityConstruction;
        }
        
    }
    public void resetResearch()
    {
        ResearchCity.interactable = true ;
    }
    public void resetSupply()
    {
        SupplyCity.interactable = true;
    }
    
    

}
