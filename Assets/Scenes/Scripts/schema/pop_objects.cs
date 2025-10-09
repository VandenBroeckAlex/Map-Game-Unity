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
        public int Id { get; }
        public int Size;
        public int ProvinceId { get; }
        public Population_Type ClassType { get; }
        public Culture Culture { get; }
        public Religion Religion { get; }
        private float _cashAmount;
        public float CashAmount 
        { get { return _cashAmount; }
          set { _cashAmount = RoundToTwoDecimals(value); }
        }
        //private float education;
        //private float militency;
        public PopGood[] GoodList;
        
        public Dictionary<string, float> PoliticalLeaning = new Dictionary<string, float>
        {
        { "Liberal", 0.1f },
        { "Monarchist", 0.6f },
        {"Neutral", 0.3f }
        };



        //constructor
        public Pop(int ID, int SIZE, int PROVINCEID, Population_Type TYPE, Culture CULTURE, Religion RELIGION, float CASHAMOUNT, PopGood[] STOCKPILE)
        {
            Id = ID;
            Size = SIZE;
            ProvinceId = PROVINCEID;
            ClassType = TYPE;
            Culture = CULTURE;
            Religion = RELIGION;
            CashAmount = CASHAMOUNT;
            GoodList = STOCKPILE;
        }
        public bool HaveBasicNeed()
        {
            for (int i = 0; i < GoodList.Length; i++)
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
