using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

public struct RunTimePopJob
{
    public string type;
    public int strata;
}

public struct PopJobDeserializeResult
{
    public Dictionary<int, string> strata;
    public Dictionary<int, RunTimePopJob> popJob;
}

public class PopJobLoader
{
    public struct PopJobData
    {
        public string strata;
        public string type;
    }
    //
    //Strata should be decleared in an other json 
    public PopJobDeserializeResult Deserialize_PopJob(string json)
    {
        Dictionary<int, string> strataDict = new Dictionary<int, string>();
        Dictionary<int, RunTimePopJob> runTimePopJob = new Dictionary<int, RunTimePopJob>();
        PopJobData[] data = JsonConvert.DeserializeObject<PopJobData[]>(json);

        //check if strata already in strata
        //check if type already in PopJob
        int i = 0;
        int dictId = 1;
        foreach (PopJobData popJob in data) 
        {
            int strataId = strataDict.FirstOrDefault(x => x.Value == popJob.strata).Key;

            //if strataId null push it to strataDict
            if(strataId == 0)
            {
                strataDict.Add(dictId, popJob.strata);
                strataId = dictId;
                dictId++;
            }
            RunTimePopJob _popJob = new RunTimePopJob();
            _popJob.type = popJob.type;
            _popJob.strata = strataId;

            runTimePopJob.Add(i,_popJob);

            i++;
        }
        PopJobDeserializeResult result = new PopJobDeserializeResult();
        result.popJob = runTimePopJob;
        result.strata = strataDict;
        return result;
    }
   


}
