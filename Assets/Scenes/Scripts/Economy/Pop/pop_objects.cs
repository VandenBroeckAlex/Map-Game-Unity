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
        public int countryID { get; set; }
        public PopJob job { get; }
        public Culture culture { get; }
        public Religion religion { get; }
        private int _cashAmount;
        private int _savings;
        public int cashAmount 
        { get { return _cashAmount; }
          set { _cashAmount = value; }
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
        public Pop(int ID, int SIZE, int PROVINCEID, PopJob JOB, Culture CULTURE, Religion RELIGION, int CASHAMOUNT, List<PopGood> STOCKPILE)
        {
            id = ID;
            size = SIZE;
            provinceId = PROVINCEID;
            job = JOB;
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


    public enum JobType
    {
        Farmer,
        Miner,
        Priest,
        Soldier,        
    }

    //population strata 
    public class PopJob
    {
        public string Type { get; }
        public string DefaultStrata { get; }

        public PopJob(string type, string defaultStrata)
        {
            Type = type;
            DefaultStrata = defaultStrata;
        }
    }

    //changing strata shoud be done at culture level
    //public class CultureStrataOverrides
    //{
    //    public Dictionary<JobType, string> Overrides = new();

    //    public string GetStrataForJob(PopJob job)
    //    {
    //        if (Overrides.TryGetValue(job.Type, out string s))
    //            return s;

    //        return job.DefaultStrata;
    //    }
    //}
    //culture.StrataOverrides.Overrides[JobType.Merchant] = "Lower";
    

    //culture and religion will be object holding stats not just name
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
