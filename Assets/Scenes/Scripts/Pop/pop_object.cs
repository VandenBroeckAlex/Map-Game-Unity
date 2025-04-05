using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


public class PopList : MonoBehaviour
{
    
    [SerializeField] public List<Pop> pop = new();
    [SerializeField] private string test;

    private void Start()
    {
        pop.Clear();
        pop.Add(new Pop(1,50, 1 ,Type.Farmer, Culture.French, Religion.Catholic, 1,new int[] {1,2,1},  new int[] {10,10,10}  ));
    }




}

[Serializable]
public struct Pop
{
    public int id { get; set; }
    public int size;
    public int provinceId;
    public Type type;
    public Culture culture;
    public Religion religion;
    private float CashAmount;
    //private float education;
    //private float militency;
    private int[] stockpile;
    private int[] maxNeed;


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



