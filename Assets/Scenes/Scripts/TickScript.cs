using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class TickScript : MonoBehaviour
{

    public static TickScript instance;
    public int curentTick = 3;
    private float gameSpeed = 0.1f;
    private bool timeIsRunning = true;
    public bool isPaused = true;
    public delegate void OnTick();
    public static OnTick onTick;

    private void CreateSingleton()
    {
        // Singleton pattern: only one instance allowed
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Debug.Log("An instance of tick script already exist");
            Destroy(gameObject);
        }
    }


    public void Initialize()
    {
        CreateSingleton();
       
    }

    public void StartTickScript()
    {
        Debug.Log("Tick script start called");
        StartCoroutine(TickTime());
    }


    public IEnumerator TickTime()
    {
        while (timeIsRunning == true)
        {
            while (isPaused)
            {
                yield return null;
            }
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