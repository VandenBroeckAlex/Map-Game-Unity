using log4net.Core;
using Newtonsoft.Json;
using System.Collections.Generic;

public class CultureLoader
{
    public struct RunTimeCulture : IHaveTag
    {
        public string tag { get; set;}
        public string name;
     
    }

    public RunTimeCulture[] DeserializeCultures(string json, IResolutionErrorHandler errorHandler)
    {

        RunTimeCulture[] cultures = JsonConvert.DeserializeObject<RunTimeCulture[]>(json);
        
        if (cultures.Length == 0) {
            errorHandler.RaiseError("There is no culture in cultureDef.json");
        }
        
        return cultures;
    }
}
