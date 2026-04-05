using Newtonsoft.Json;
using NUnit.Framework;
using System.Collections.Generic;
using System.IO;

public class StrataNeedLoader
{
  
    public Dictionary<string, GoodNeedMax[]> DeserializeStrataNeeds(string json, string[] strataList, Good[] goods)
    {
        Dictionary<string, Dictionary<string, int>> strataNeedData = JsonConvert.DeserializeObject<Dictionary<string, Dictionary<string, int>>>(json);

        Dictionary<string, GoodNeedMax[]> strataNeeds = new Dictionary<string, GoodNeedMax[]>();

        for (int i = 0; i < strataList.Length; i++)
        {
            if (strataNeedData.TryGetValue(strataList[i], out Dictionary<string, int> strataNeed)){

            }
            else
            {
                throw new InvalidDataException(
                 $" Needs for Strata ${strataList[i]} not found in StrataNeedDef.json");
            }

           GoodNeedMax[] result = new GoodNeedMax[strataNeed.Count];

            int indexer = 0;
            foreach (var kvp in strataNeed)
            {
                GoodNeedMax data = new GoodNeedMax();
                data.goodId = FindGoodID(kvp.Key, goods);
                data.Max = kvp.Value;
                result[indexer] = data;
                indexer++;
            }


            strataNeeds[strataList[i]] = result;

        }
        // strata : [int,int],[int,int],[int,int]

        return strataNeeds;
    }

    private int FindGoodID(string goodNameToFind,Good[] goods)
    {
        foreach (var good in goods) 
        { 
            string goodName = good.name.Trim().ToLower();
            string _goodNameToFind = goodNameToFind.Trim().ToLower();

            if(goodName == _goodNameToFind)
            {
                return good.id;
            }
           
        }
        throw new InvalidDataException(
            $"Could not find {goodNameToFind} from StrataNeedDef.json in GoodDef.json." +
            $"The Good list include {goods.Length} goods" +
            $""
            );
    }
}
