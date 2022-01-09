using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class draganddrop : MonoBehaviour
{
   
   
    private void OnMouseDown()
    {

      

    }

    private void OnMouseDrag()
    {
        
        Vector3 point = new Vector3();
      //  Event currentEvent = Event.current;
        Vector2 mousePos = new Vector2();

        // Get the mouse position from Event.
        // Note that the y position from Event is inverted.
        mousePos.x = Input.mousePosition.x;
        mousePos.y = Input.mousePosition.y;
        point = Camera.main.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, 10)); // 10 is the camera depth
        transform.position = point;
        

    }
   

    
    // Update is called once per frame
    void Update()
    {
        
    }
}
