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
        if (init.Instance.currentCursor == init.cursorState.isolation)
        {
            if (GetComponent<node>().currentStatus == node.nodeStatus.Normal || GetComponent<node>().currentStatus == node.nodeStatus.Sick)
            {
                GetComponent<node>().lastStatus = GetComponent<node>().currentStatus;
                GetComponent<node>().currentStatus = node.nodeStatus.Isolation;
            }
            else if (GetComponent<node>().currentStatus == node.nodeStatus.Isolation)
            {
                GetComponent<node>().currentStatus = GetComponent<node>().lastStatus;
            }

        }
          
    }
}
