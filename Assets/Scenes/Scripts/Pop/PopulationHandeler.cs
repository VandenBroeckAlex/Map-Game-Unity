using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;


public class PopulationHandeler: MonoBehaviour
{

    public int test_population_size = 100;

    [SerializeField] public List<Pop> populationList = new();
    
    public float base_growth_rate = 0.004f;
    public float base_consumption = 1f;
    public float base_production = 1.1f;
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
       
        populationList.Clear();
        for (int i = 0; i < test_population_size; i++)
        {
            populationList.Add(new Pop(1, 1000, 1, Population_Type.Farmer, Culture.French, Religion.Catholic, 1, new int[] { 1, 2, 1 }, new int[] { 10, 10, 10 }));
        }        
        //get save or initial to create pop
    }

    /*
     public Pop[] SelectPopulationByCountry(int countryId)
     {

     }
    */
 
    public List<Pop> SelectPopulationByProvince(int provinceID)
    {
        List <Pop> selectedPops = new ();
        for (int i = 0; i < populationList.Count; i++)
        {
            if (populationList[i].ProvinceId == provinceID) 
            {
                selectedPops.Add(populationList[i]);
            }
        }
        return selectedPops;
    }


    private void PopGrowth()
    {
        for (int i = 0; i < populationList.Count; i++)
        {
            populationList[i].Size += (int)Math.Round(populationList[i].Size * base_growth_rate); 
            //Base Growth Rate × Pop Size × Modifiers every years
            //Debug.Log(pops[i].size);
        }
        Debug.Log("the pop have grow !");
    }


   private void PopBuy()
    {
        for (int i = 0; i < populationList.Count; i++)
        {
            // pass all the needs and all the money of a pop to market
            // market awner with needs fill and money left
            
        }
        Debug.Log("the pop's have buy goods !");
    }

}



[System.Serializable]
public class Pop
{
    public int Id;
    public int Size;
    public int ProvinceId;
    public Population_Type ClassType;
    public Culture Culture;
    public Religion Religion;
    public float CashAmount;
    //private float education;
    //private float militency;
    public int[] Stockpile;
    public int[] MaxNeed;
    public Dictionary<string, float> PoliticalLeaning  = new Dictionary<string, float>
    {
        { "Liberal", 0.3f },
        { "Monarchist", 0.6f },
        { "Socialist", 0.1f }
    };
    public string Ideology  = "Monarchist";  // Highest leaning by default


    //constructor
    public Pop(int ID, int SIZE, int PROVINCEID, Population_Type TYPE, Culture CULTURE, Religion RELIGION, float CASHAMOUNT, int[] STOCKPILE, int[] MAXNEED  )
    {
        Id = ID;
        Size = SIZE;
        ProvinceId = PROVINCEID;
        ClassType = TYPE;
        Culture = CULTURE;
        Religion = RELIGION;
        CashAmount = CASHAMOUNT;
        Stockpile = STOCKPILE;
        MaxNeed = MAXNEED;
}
    
 
}
public enum Population_Type
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





