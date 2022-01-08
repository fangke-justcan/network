﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class edgeVertex : MonoBehaviour
{
    
    public GameObject v1;
    public GameObject v2;

    // Start is called before the first frame update
    

    // Update is called once per frame
    void Update()
    {

        GetComponent<LineRenderer>().SetPosition(0, v1.transform.position);
        GetComponent<LineRenderer>().SetPosition(1, v2.transform.position);

        //if (Input.GetKeyDown(KeyCode.A))  pullALittle();
    }

    void pullALittle()
    {
        Vector3 newv1, newv2;
        newv1= Vector3.Lerp(v1.transform.position, v2.transform.position,0.001f);
        newv2= Vector3.Lerp(v1.transform.position, v2.transform.position, 0.999f);
        v1.transform.position = newv1;
        v2.transform.position = newv2;
    }
}