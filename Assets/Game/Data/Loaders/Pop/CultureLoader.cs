using log4net.Core;
using Newtonsoft.Json;
using System.Collections.Generic;

public class CultureLoader
{
    public Culture[] DeserializeCultures(string json, IResolutionErrorHandler errorHandler)
    {

        Culture[] cultures = JsonConvert.DeserializeObject<Culture[]>(json);
        
        if (cultures.Length == 0) {
            errorHandler.RaiseError("There is no culture in cultureDef.json");
        }
        
        return cultures;
    }
}
