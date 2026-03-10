using Newtonsoft.Json;
using System.Collections.Generic;

public class CultureLoader
{
    public struct RunTimeCulture : IHaveTag
    {
        public string tag { get; set;}
        public string name;
     
    }

    public RunTimeCulture[] DeserializeCultures(string json){

        RunTimeCulture[] cultures = JsonConvert.DeserializeObject<RunTimeCulture[]>(json);
    
        return cultures;
    }
}
