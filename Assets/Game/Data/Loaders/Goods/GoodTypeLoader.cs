using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using log4net.Core;

public class GoodTypeLoader
{
  
    

    public string[] Deserialize_goodsType(string json, IResolutionErrorHandler errorHandler)
    {
        string[] goodTypes = JsonConvert.DeserializeObject<string[]>(json);

        if(goodTypes.Length == 0)
        {
            errorHandler.RaiseError("[CRITICAL] : No good type in GoodTypeDef.json");
        }


        return goodTypes;
    }
}
