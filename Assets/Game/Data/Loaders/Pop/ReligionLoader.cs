using Newtonsoft.Json;
using System.Collections.Generic;

public class ReligionLoader
{
    public struct RunTimeReligion : IHaveTag
    {
        public string tag { get; set; }
        public string name;
    }

    public  RunTimeReligion[] DeserializeReligions(string json)
    {

        RunTimeReligion[] religionsData = JsonConvert.DeserializeObject<RunTimeReligion[]>(json);

        return religionsData;
    }
}
