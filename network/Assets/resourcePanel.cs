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
        totalIncomeText.text = "�����룺" + init.Instance.totalIncome.ToString("f1") + "��(����+" + init.Instance.totalProduction.ToString("f1") + "��)";
        medicalSupplyText.text = "ҽ�����ʣ�" + init.Instance.medicalSupply.ToString("f1") + "��(���� + " + init.Instance.totalMedicalSupplyProduction.ToString("f1") + "��)";
        researchProgressText.text = "�����о���"+ (init.Instance.vaccineProgress * 100 / init.Instance.ResearchFinish).ToString("f1") + "%������+"+ (init.Instance.totalVaccineResearchProduction * 100 / init.Instance.ResearchFinish).ToString("f1") + "%��";

    }
}
