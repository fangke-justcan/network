using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class resourcePanel : MonoBehaviour
{
    public Text totalIncomeText;
    public Text medicalSupplyText;
    public Text researchProgressText;
   

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        totalIncomeText.text = "总收入：" + init.Instance.totalIncome.ToString("f1") + "万(明日+" + init.Instance.totalProduction.ToString("f1") + "万)";
        medicalSupplyText.text = "医疗物资：" + init.Instance.medicalSupply.ToString("f1") + "套(明日 + " + init.Instance.totalMedicalSupplyProduction.ToString("f1") + "套)";
        researchProgressText.text = "疫苗研究："+ (init.Instance.vaccineProgress * 100 / init.Instance.ResearchFinish).ToString("f1") + "%（明日+"+ (init.Instance.totalVaccineResearchProduction * 100 / init.Instance.ResearchFinish).ToString("f1") + "%）";

    }
}
