using Newtonsoft.Json;
using System.Collections.Generic;

public class CultureLoader
{
    public struct RunTimeCulture
    {
        public string tag;
        public string name;
    }

    public RunTimeCulture[] DeserializeCultures(string json){

        RunTimeCulture[] cultures = JsonConvert.DeserializeObject<RunTimeCulture[]>(json);
    
        return cultures;
    }
}
