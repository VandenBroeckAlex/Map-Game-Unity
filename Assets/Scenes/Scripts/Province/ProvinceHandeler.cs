using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProvinceHandeler : MonoBehaviour
{


    public GameObject[] allProvinces;
    //ux
    public Material MatTileHiglight;



    

    void Awake()
    {
        foreach (Transform t in transform)
        {
            t.gameObject.tag = "Province";
            t.gameObject.layer = 6;
        }
        allProvinces = GameObject.FindGameObjectsWithTag("Province");

    }
}
