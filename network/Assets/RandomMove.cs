using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomMove : MonoBehaviour
{
   
    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 a;
        a.x = Random.Range(-0.001f, 0.001f);
        a.y = Random.Range(-0.001f, 0.001f);
        a.z = 0;
        transform.Translate(a);

    }

    private void OnMouseOver()
    {
        Vector3 a;
        a.x = 0.001f* Mathf.Sin(Time.time*50);
        a.y = 0.001f* Mathf.Cos(Time.time*50);
        a.z = 0;
        transform.Translate(a);
    }
}
