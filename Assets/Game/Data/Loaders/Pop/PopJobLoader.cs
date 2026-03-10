using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;


public struct RunTimePopJob
{
    public string type;
    public int strata;
}

public struct PopJobDeserializeResult
{
    public RunTimePopJob[] popJob;
}

public class PopJobLoader
{
    public struct PopJobData
    {
        public string type;
        public string strata;
    }
    //
    //Strata should be decleared in an other json 
    public RunTimePopJob[] Deserialize_PopJob(string json, string[] strata)
    {

        List<RunTimePopJob> popJobList = new List<RunTimePopJob>();

        PopJobData[] data = JsonConvert.DeserializeObject<PopJobData[]>(json);

        foreach(PopJobData dataItem in data)
        {
            RunTimePopJob rtpj = new RunTimePopJob();
            rtpj.type = dataItem.type;
            bool strataExist = false;
            for (int i = 0; i < strata.Length; i++) 
            {
                if (strata[i] == dataItem.strata)
                {
                    rtpj.strata = i;
                    popJobList.Add(rtpj);
                    strataExist = true;
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
