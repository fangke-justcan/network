using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomMove : MonoBehaviour
{
   
    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 a;
        a.x = Random.Range(-0.01f, 0.01f);
        a.y = Random.Range(-0.01f, 0.01f);
        a.z = 0;
        transform.Translate(a);
    }
}
