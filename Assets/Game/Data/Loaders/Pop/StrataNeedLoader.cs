using Newtonsoft.Json;
using NUnit.Framework;
using System.Collections.Generic;

public class StrataNeedLoader
{
    public class GoodNeedMax
    {
        public int goodId; 
        public int Max;
    }
    public Dictionary<string, List<GoodNeedMax>> Deserialize_goodsType(string json, string[] strataList, Good[] goods)
    {
        Dictionary<string, Dictionary<string, int>> strataNeedData = JsonConvert.DeserializeObject<Dictionary<string, Dictionary<string, int>>>(json);

        GoodNeedMax[] result = new GoodNeedMax[strataList.Length];

        for (int i=0; i < strataList.Length; i++)
        {
            Dictionary<string, int> a = strataNeedData[strataList[i]];

            GoodNeedMax data = new GoodNeedMax();
            

        }
        // strata : [int,int],[int,int],[int,int]

        return strataNeedData;
    }

}
