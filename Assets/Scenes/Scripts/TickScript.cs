using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class TickScript : MonoBehaviour
{
    
    
    public int curentTick = 3;
    private float gameSpeed = 0.1f;
    private bool timeIsRunning = true;
    public delegate void OnTick();
    public static OnTick onTick;

    


   

    public void StartTickScript()
    {
        Debug.Log("Tick script start called");
        StartCoroutine(TickTime());
    }


     public  IEnumerator TickTime()
    {
        while (timeIsRunning == true)
        {
            onTick?.Invoke(); // ? check if is null and if not invoke
            curentTick++;            
            print("Tick: " + curentTick);
            yield return new WaitForSeconds(gameSpeed);
        }
    }       

    public void PauseGame()
    {
        gameSpeed = 0;
    }
    public void OneSpeed()
    {
        gameSpeed = 0.8f;
    }
    public void TwoSpeed()
    {
        gameSpeed = 0.5f;
    }
    public void ThreeSpeed()
    {
        gameSpeed = 0.1f;
    }
}