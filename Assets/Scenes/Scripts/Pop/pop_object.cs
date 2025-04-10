using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


public class PopList : MonoBehaviour
{
    
    [SerializeField] public List<Pop> pops = new();
    [SerializeField] private string test;

    private void OnEnable()
    {

        DateHandeler.onMonth += PopGrowth;
    }

    private void OnDisable()
    {
        DateHandeler.onMonth -= PopGrowth;
    }


    private void Start()
    {
        pops.Clear();
        for (int i = 0; i < 1000; i++)
        {
            pops.Add(new Pop(1, 1000, 1, Type.Farmer, Culture.French, Religion.Catholic, 1, new int[] { 1, 2, 1 }, new int[] { 10, 10, 10 }));
            pops.Add(new Pop(1, 1000, 1, Type.Miner, Culture.German, Religion.Protestant, 1, new int[] { 1, 2, 1 }, new int[] { 10, 10, 10 }));
        }        
        //get save or initial to create pop
    }

   


    private void PopGrowth()
    {
        for (int i = 0; i < pops.Count; i++)
        {
            pops[i].size += (int)Math.Round(pops[i].size * 0.004); //Base Growth Rate × Pop Size × Modifiers every years
            //Debug.Log(pops[i].size);
        }
        Debug.Log("the pop have grow !");
    }


   

}

[Serializable]
public class Pop
{
    public int id;
    public int size;
    public int provinceId;
    public Type type;
    public Culture culture;
    public Religion religion;
    private float CashAmount { get; set; }
    //private float education;
    //private float militency;
    private int[] stockpile { get; set; }
    private int[] maxNeed { get; set; }


    //constructor
    public Pop(int ID, int SIZE, int PROVINCEID, Type TYPE, Culture CULTURE, Religion RELIGION, float CASHAMOUNT, int[] STOCKPILE, int[] MAXNEED  )
    {
        id = ID;
        size = SIZE;
        provinceId = PROVINCEID;
        type = TYPE;
        culture = CULTURE;
        religion = RELIGION;
        CashAmount = CASHAMOUNT;
        stockpile = STOCKPILE;
        maxNeed = MAXNEED;

}
    
 
}
public enum Type
{
    Miner,
    Farmer
}
public enum Culture
{
    French,
    German
}
public enum Religion
{
    Catholic,
    Protestant
}



