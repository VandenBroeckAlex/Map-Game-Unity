using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

public class GoodTypeLoader
{
    public Dictionary<int, string> Deserialize_goodsType(string json)
    {
        Dictionary<int, string> types = new Dictionary<int, string>();
        string[] result = JsonConvert.DeserializeObject<string[]>(json);

        for (int i = 0; i < result.Length; i++) 
        {
            types.Add(i, result[i]);
        }
        return types;
    }
}
