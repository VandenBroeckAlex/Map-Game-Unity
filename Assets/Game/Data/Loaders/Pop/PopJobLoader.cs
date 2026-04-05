using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;



public struct PopJobDeserializeResult
{
    public PopJob[] popJob;
}

public class PopJobLoader
{
   public struct PopJobData
    {
        public string type;
        public string strata;
        public string tag;
    }
    //
    //Strata should be decleared in an other json 
    public PopJob[] Deserialize_PopJob(string json, string[] strata)
    {

        List<PopJob> popJobList = new List<PopJob>();

        PopJobData[] data = JsonConvert.DeserializeObject<PopJobData[]>(json);

        foreach(PopJobData dataItem in data)
        {
            
            bool strataExist = false;
            for (int i = 0; i < strata.Length; i++) 
            {
                //check if given strata exist
                if (strata[i] == dataItem.strata)
                {
                    strataExist = true;
                    PopJob rtpj = new PopJob(dataItem.type, i, dataItem.tag);
                    popJobList.Add(rtpj);
                    break;
                } 
            }
            if (!strataExist)
            {
                throw new InvalidDataException(
                $"Unknown strata  '{dataItem.strata}' while creating job '{dataItem.type}'.");
            }
        }
        return popJobList.ToArray();
    }
   


}
