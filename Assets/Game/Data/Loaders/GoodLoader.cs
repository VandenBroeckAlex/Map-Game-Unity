using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.IO;
using System;


public class GoodLoader  
{

    private struct GoodData
    {
        public string name;
        public int basePrice;
        public float baseProductionModdifier;
        public string type;
        public string color;
        public string iconPath;
    }

    public static List<Good> allGoodsDefinition;

    public static List<Good> Deserialize_goods(string _FilePath)
    {
        string jsonText = File.ReadAllText(_FilePath);
        return Load_goods(jsonText);
    }


    public static List<Good> Load_goods(string jsonText)
    {
        
        List<Good>  good_list = JsonConvert.DeserializeObject<List<Good>>(jsonText);
        allGoodsDefinition = good_list;
        return good_list;
    }

   

}
