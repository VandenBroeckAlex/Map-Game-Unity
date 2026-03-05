using Newtonsoft.Json;
using System.Collections.Generic;

public class ReligionLoader
{
    public struct RunTimeReligion
    {
        public string tag;
        public string name;
    }

    public Dictionary<int, RunTimeReligion> DeserializeReligions(string json)
    {

        RunTimeReligion[] cultures = JsonConvert.DeserializeObject<RunTimeReligion[]>(json);

        Dictionary<int, RunTimeReligion> result = new Dictionary<int, RunTimeReligion>();

        int index = 0;
        foreach (RunTimeReligion c in cultures)
        {
            result[index] = c;
            index++;
        }
        return result;
    }
}
