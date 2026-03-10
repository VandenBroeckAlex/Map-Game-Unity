using Newtonsoft.Json;
using System.Collections.Generic;

public class ReligionLoader
{
    public struct RunTimeReligion
    {
        public string tag;
        public string name;
    }

    public  RunTimeReligion[] DeserializeReligions(string json)
    {

        RunTimeReligion[] religionsData = JsonConvert.DeserializeObject<RunTimeReligion[]>(json);

        return religionsData;
    }
}
