using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

public class GoodTypeLoader
{
  
    

    public string[] Deserialize_goodsType(string json)
    {
        string[] goodTypes = JsonConvert.DeserializeObject<string[]>(json);

    
        return goodTypes;
    }
}
