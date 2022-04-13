using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cityPanel : MonoBehaviour
{
    public GameObject explainer;
    // Start is called before the first frame update
    private void OnMouseOver()
    {
        explainer.SetActive(true);

    }

    private void OnMouseExit()
    {
        explainer.SetActive(false);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
