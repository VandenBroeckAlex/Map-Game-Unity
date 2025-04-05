using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Tick_script : MonoBehaviour
{
    
    
    public int curentTick;
    private float gameSpeed = 0.1f;
    private bool timeIsRunning = true;
    public delegate void OnTick();
    public static OnTick onTick;

   
    
     void Start()

    {

        StartCoroutine(TickTime());       
    }



     public  IEnumerator TickTime()
    {
        while (timeIsRunning == true)
        {
            onTick?.Invoke(); // ? check if is null and if not invoke
            curentTick++;            
            print("Tick: " + curentTick);
            //Event on tick
            yield return new WaitForSeconds(gameSpeed);
        }
    }       


    //Event on tick
}