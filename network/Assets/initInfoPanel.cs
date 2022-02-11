using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class initInfoPanel : MonoBehaviour
{

    public GameObject initPanel;
    public simulation sim;
    public Slider Pconnect;
    public Slider Psick;
    public Text PconnectText;
    public Text PsickText;
    public Text ParaText;
    // Start is called before the first frame update
    void Start()
    {
        Pconnect.value = init.Instance.Pconnect;
        Psick.value = init.Instance.Psick;
    }

    public void playButton()
    {
        init.Instance.currentGameStarus = init.gameStatus.play;
        init.Instance.relocate();
        init.Instance.reshape();
    }

    public void simButtton()
    {
        sim.startSimulation();
    }

    public void onPconnectSliderChange()
    {
        init.Instance.Pconnect = Pconnect.value;
    }

    public void onPsickSliderChange()
    {
        init.Instance.Psick = Psick.value;
    }

    public void reNetworkButton()
    {
        init.Instance.reNetwork();
        sim.startSimulation();
    }

    // Update is called once per frame
    void Update()
    {
        if (init.Instance.currentGameStarus == init.gameStatus.play) initPanel.SetActive(false);
        PconnectText.text = "Pconnect=" + init.Instance.Pconnect;
        PsickText.text = "Psick=" + init.Instance.Psick;
        ParaText.text = "Total Edge Count   = " + init.Instance.edgeCnt + "\nDay 5 Percentage = " + sim.averageSimResults[5] / init.Instance.nodeCnt + "\nDay10 Percentage = " + sim.averageSimResults[10] / init.Instance.nodeCnt;


    }
}
