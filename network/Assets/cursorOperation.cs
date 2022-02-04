using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cursorOperation : MonoBehaviour
{
    // Start is called before the first frame update
  

    // Update is called once per frame
    void Update()
    {
      
    }
    private void OnMouseDown()
    {
        if (init.Instance.currentShowMode == init.showMode.past) return;
        if (init.Instance.currentCursor == init.cursorState.quarantine )   
        {
            if (GetComponent<node>().currentStatus != node.nodeStatus.Quaratine && init.Instance.quarantineCnt > 0)
            {
                GetComponent<node>().lastStatus = GetComponent<node>().currentStatus;
                GetComponent<node>().currentStatus = node.nodeStatus.Quaratine;
                init.Instance.quarantineCnt--;
            }
            else if (GetComponent<node>().currentStatus == node.nodeStatus.Quaratine)
            {
                GetComponent<node>().currentStatus = GetComponent<node>().lastStatus;
                init.Instance.quarantineCnt++;
            }

        }

        if (init.Instance.currentCursor == init.cursorState.test && init.Instance.testCnt >0 && !GetComponent<node>().sickDetcted)
        {
            bool safe = true;

            if (GetComponent<node>().currentStatus == node.nodeStatus.Sick) { GetComponent<node>().sickDetcted = true; safe = false; }
            else if (GetComponent<node>().currentStatus == node.nodeStatus.Normal) GetComponent<node>().normalDected = true;
            else if (GetComponent<node>().currentStatus == node.nodeStatus.Quaratine && GetComponent<node>().lastStatus == node.nodeStatus.Sick) { GetComponent<node>().sickDetcted = true; safe = false; }
            else if (GetComponent<node>().currentStatus == node.nodeStatus.Quaratine && GetComponent<node>().lastStatus == node.nodeStatus.Normal) GetComponent<node>().normalDected = true;
            init.Instance.testCnt--;
            Debug.Log("You tested: " + GetComponent<node>().index +", and he's " + (safe?"safe":"sick"));

        }


    }
}
