using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mouseCursor : MonoBehaviour
{
    bool trackMouse;
    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = false;
        trackMouse = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (trackMouse)
        {
            Vector2 cursorPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            transform.position = cursorPos;
        }
            
    }

    public void changeTrackingMouss()
    {
        if (trackMouse)
        {
            trackMouse = false;
        }
        else
            trackMouse = true;
    }
}
