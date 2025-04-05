using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Tick_script;

public class MouseClickHandeler : MonoBehaviour
{
    // Start is called before the first frame update

    public delegate void OnLeftClick();
    public static OnLeftClick onLeftClick;


    public delegate void OnRightClick();
    public static OnRightClick onRightClick;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // left click
        if(Input.GetMouseButtonDown(0))
        {
            Debug.Log("left click");
            onLeftClick?.Invoke();
        }
        //right click
        if(Input.GetMouseButtonDown(1))
        {
            Debug.Log("Right click");
            onRightClick?.Invoke();
        }
    }
}
