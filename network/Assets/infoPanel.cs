using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//using System;

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
    public Text researchCityButton;
    public Text medicalSupplyCityButton;
    public GameObject initPanel;



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
       // sickpercentage.text = "日产能: " + init.Instance.totalProduction.ToString("f1") + "万";
       // sickpercentage.text += "\n总产值： " + init.Instance.totalIncome.ToString("f1") + "万";



        //+ "万\n预估规模疫情城市数: " +init.Instance.sickCnt;

        //  sickpercentage.text += "\n特殊经费等级: " + init.Instance.specialFundLevelRate[init.Instance.specialFundLevel]*100 +"%总生产力" ;
        //  sickpercentage.text += "\n特殊经费增量: " + (init.Instance.specialFundLevelRate[init.Instance.specialFundLevel] * init.Instance.totalProduction).ToString("f1") + "万";
        //   sickpercentage.text += "\n特殊经费累计: " + init.Instance.specialFund.ToString("f1") + "万";

       // sickpercentage.text += "\n\n医疗物资日产能: " + init.Instance.totalMedicalSupplyProduction.ToString("f1") + "套";
       // sickpercentage.text += "\n医疗物资: " + init.Instance.medicalSupply.ToString("f1") + "套";
       // sickpercentage.text += "\n\n疫苗研究: " + (init.Instance.totalVaccineResearchProduction*100/init.Instance.ResearchFinish).ToString("f1") + "%";
      //  sickpercentage.text += "\n疫苗累计进展: " + (init.Instance.vaccineProgress * 100 / init.Instance.ResearchFinish).ToString("f1") + "%";

        quarantinecnt.text = "" + init.Instance.quarantineCnt;
        testcnt.text = "" + init.Instance.testCnt; // + "\n circling node:" + circlingCnt;
        researchCityButton.text = "建设(" + init.Instance.VRCityConstruction + "万)";
        medicalSupplyCityButton.text = "建设(" + init.Instance.MSCityConstruction + "万)";


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
        if (init.Instance.totalIncome >= init.Instance.MSCityConstruction)
        {
            init.Instance.nodes[index].GetComponent<node>().changeNodeType(node.nodeProductionStatus.Supply);
            SupplyCity.interactable = false;
            init.Instance.totalIncome -= init.Instance.MSCityConstruction;
            init.Instance.MSCityConstruction += init.Instance.MSCityConstructionIncrease;
        }
        
    }
    public void onResearchButton()
    {
        if (ResearchCity.interactable == false) return;
        if (!int.TryParse(ResearchCity.text, out int index))  return;
        if (index < 0 || index >= init.Instance.nodeCnt) return;
        if (init.Instance.totalIncome >= init.Instance.VRCityConstruction)
        {
            init.Instance.nodes[index].GetComponent<node>().changeNodeType(node.nodeProductionStatus.Research);
            ResearchCity.interactable = false;
            init.Instance.totalIncome -= init.Instance.VRCityConstruction;
            
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
    
    public void onScientificModelling()
    {
        if (init.Instance.totalIncome >= init.Instance.scientificModellingCost)
        {
            init.Instance.totalIncome -= init.Instance.scientificModellingCost;
            float credibility = Random.Range(0.7f,0.9f);
            int minNum = Mathf.RoundToInt(credibility * init.Instance.sickCnt);
            int maxNum = Mathf.RoundToInt((2 - credibility) * init.Instance.sickCnt);
            
            int finalEst = Random.Range(minNum, maxNum);
            minNum += (finalEst - init.Instance.sickCnt);
            maxNum += (finalEst - init.Instance.sickCnt);
            sickpercentage.text = "科学模型估计，全国有" +minNum+"-" +maxNum+"个城市处于规模疫情状态，本次估计可信度为"+(credibility*100).ToString("f0") +"%";
        }
    }
    
    public void backToMainMenu()
    { 
        init.Instance.currentGameStarus = init.gameStatus.init;
        initPanel.SetActive(true);
        
    }

}
