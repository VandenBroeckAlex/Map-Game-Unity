using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class ProvinceClass : MonoBehaviour
{
    
    public Renderer  rend; 
    public Province worldProvince = new Province();    
    Material base_mat;    
    public Material highlight;
    Material[] baseMaterials = new Material [2];
    readonly Material[] highlightMaterials = new Material[2];
    [SerializeField] Vector3 cubePosition;

    [SerializeField] bool isProvince = true;



    private void Awake()
    {        
        base_mat = GetComponent<Renderer>().material;
        rend = GetComponent<Renderer>();
        cubePosition = rend.bounds.center;
    }


    void Start()
    {
        highlight= GameObject.Find("ProvinceHandeler").GetComponent<ProvinceHandeler>().MatTileHiglight;
        baseMaterials[0] = base_mat;
        baseMaterials[1] = base_mat;        
        highlightMaterials[0] = base_mat;
        highlightMaterials[1] = highlight;
    }

    void OnMouseUpAsButton()
    {
            Debug.Log(name);
    }

    void OnMouseEnter()
    {
            //add higlight Mat to material list
            GetComponent<Renderer>().materials = highlightMaterials;           
    }
    void OnMouseExit()
    {
            //remove higlight Mat to material list
            GetComponent<Renderer>().materials = baseMaterials;
    }}

    [System.Serializable]
    public class Province
    {
        public string name;
        public string description;
        public int id;
        public int Type;
        public int owner;
    }

