using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Market_object;
using static helpers_math;

public class Pop_objects 
{

    [System.Serializable]
    public class Pop
    {
        public int id { get; }
        public int size;
        public int provinceId { get; }
        public Population_Type classType { get; }
        public Culture culture { get; }
        public Religion religion { get; }
        private float _cashAmount;
        private float _savings;
        public float cashAmount 
        { get { return _cashAmount; }
          set { _cashAmount = RoundToTwoDecimals(value); }
        }
        //private float education;
        //private float militency;
        public List<PopGood> GoodList = new List<PopGood>();
        
        public Dictionary<string, float> PoliticalLeaning = new Dictionary<string, float>
        {
        { "Liberal", 0.1f },
        { "Monarchist", 0.6f },
        {"Neutral", 0.3f }
        };



        //constructor
        public Pop(int ID, int SIZE, int PROVINCEID, Population_Type TYPE, Culture CULTURE, Religion RELIGION, float CASHAMOUNT, List<PopGood> STOCKPILE)
        {
            id = ID;
            size = SIZE;
            provinceId = PROVINCEID;
            classType = TYPE;
            culture = CULTURE;
            religion = RELIGION;
            cashAmount = CASHAMOUNT;
            GoodList =  STOCKPILE;
        }
        public bool HaveBasicNeed()
        {
            for (int i = 0; i < GoodList.Count; i++)
            {
                if (GoodList[i].Stockpile != GoodList[i].MaxNeed)
                {
                    return false;
                }
            }
            return true;
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
}
