using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;




public class GoodLoader  
{
    public static Good[] allGoodsDefinition;
    string[] goodType;
    DataRegistery _registery;

    public GoodLoader(DataRegistery registery)
    {
        _registery = registery;
    }

    private struct GoodData
    {
        public string name;
        public string tag;
        public int basePrice;
        public int baseProductionModdifier;
        public string type;
        public string color;
        public string iconPath;
        public bool isRGO;
    }


    public GoodLoadedData Deserialize_goods(string _FilePath)
    {
        string jsonText = File.ReadAllText(_FilePath);
        return Load_goods(jsonText);
    }

    public class GoodLoadedData
    {
        public Good[] goodList;
        public Dictionary<string,int> rgoTag;
    }

    public GoodLoadedData Load_goods(string jsonText)
    {
        List<GoodData>  good_list = JsonConvert.DeserializeObject<List<GoodData>>(jsonText);
        Good[] goodArray = new Good[good_list.Count];
        Dictionary<string, int> rgoTag = new Dictionary<string, int>();

        int id = 0;

        GoodJsonValidator validator = new GoodJsonValidator();


        foreach (GoodData _good in good_list) 
        { 
            Good good = new Good();
            good.id = id;
            good.name = _good.name;
            good.basePrice = _good.basePrice;
            good.tag = _good.tag;


            int typeID = _registery.GetGoodTypeTagId(_good.type);
            if(typeID < 0)
            {
                throw new InvalidDataException(
                $"Unknown good type '{_good.type}' while creating good '{good.name}'.");
            }

            good.type = typeID;
            good.baseProductionModdifier = _good.baseProductionModdifier;
            good.color = _good.color;   
            good.iconPath = _good.iconPath;

            goodArray[id] = good;

            if(_good.isRGO == true)
            {
                rgoTag[_good.tag] = good.id;
            }

            id++;
        }
        GoodLoadedData result = new GoodLoadedData();
        result.goodList = goodArray;
        result.rgoTag = rgoTag;
        return result;
    }
}
