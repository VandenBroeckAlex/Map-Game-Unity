using Newtonsoft.Json;
using System.Collections.Generic;

public class CultureLoader
{
    public struct RunTimeCulture
    {
        public string tag;
        public string name;
    }

    public Dictionary<int,RunTimeCulture> DeserializeCultures(string json){

        RunTimeCulture[] cultures = JsonConvert.DeserializeObject<RunTimeCulture[]>(json);

        Dictionary<int, RunTimeCulture> result = new Dictionary<int, RunTimeCulture>();

        int index = 0;
        foreach (RunTimeCulture c in cultures) 
        { 
            result[index] = c;
            index++;
        }
        return result;
    }
}
