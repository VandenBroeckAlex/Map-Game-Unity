using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static helpers_math;
using static Market_object;
using static Workplace;

public class Pop_objects 
{

    [System.Serializable]
    public class Pop
    {
        public int id { get; }
        public int size;
        public List<IdNum> workplace;
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
        public List<GoodRequirement> GoodList = new List<GoodRequirement>();
        
        public Dictionary<string, float> PoliticalLeaning = new Dictionary<string, float>
        {
        { "Liberal", 0.1f },
        { "Monarchist", 0.6f },
        {"Neutral", 0.3f }
        };



        //constructor
        public Pop(int ID, int SIZE, int PROVINCEID, PopJob JOB, Culture CULTURE, Religion RELIGION, int CASHAMOUNT, List<GoodRequirement> STOCKPILE)
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
    
        public int GetUnemployedNumber()
        {
            int unemployed = size;
            foreach(IdNum val in workplace)
            {
                unemployed -= val.num;
            }

            if(unemployed < 0)
            {
                new InvalidOperationException("Their is more employed pop than pop size");
            }

            return unemployed;
        }

        public void FiredFromWorkplace(int workplaceId,int ammount)
        {
            IdNum workerInWP =  workplace.Where(w => w.id == workplaceId).FirstOrDefault();
            if(workerInWP != null)
                new InvalidOperationException("Pop is fired from a workplace Pop don't know about");

            int workerNumber = workerInWP.num;

            if (workerNumber == 0) 
            {
                workplace.Remove(workerInWP);
            }
            if(workerNumber < 0)
            {
                new InvalidOperationException("Workplace and pop data is De-Sync");
            }

            workerInWP.num -= ammount;
        }

        public void HireInWorkplace(int workplaceId, int ammount) 
        {
            IdNum workerInWP = workplace.Where(w => w.id == workplaceId).FirstOrDefault();

            if(workerInWP is null)
            {
                IdNum _workerInWP = new IdNum(workplaceId, ammount);
                workplace.Add(_workerInWP);
            }
            else
            {
                workerInWP.num += ammount;
            }
        }
    }


    //population strata 
    //define in a json
    public class PopJob
    {
        public int iD;
        public string type { get; }
        public string strata { get; }

        public PopJob(string type, string defaultStrata)
        {
            this.type = type;
            strata = defaultStrata;
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
    //popjob : miners, farmers

}
