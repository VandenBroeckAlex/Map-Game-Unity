using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;




public class GoodLoader  
{
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


    public static Good[] allGoodsDefinition;
    public Dictionary<int, string> goodType;


    public GoodLoadedData Deserialize_goods(string _FilePath)
    {
        string jsonText = File.ReadAllText(_FilePath);
        return Load_goods(jsonText, goodType);
    }

    public class GoodLoadedData
    {
        public Good[] goodList;
        public Dictionary<string,int> rgoTag;
    }

    public GoodLoadedData Load_goods(string jsonText, Dictionary<int, string> goodType)
    {
        
        HashSet<string> validType = new HashSet<string>();

        foreach (KeyValuePair<int, string> kvp in goodType)
        {
            validType.Add(kvp.Value);
        }


        List<GoodData>  good_list = JsonConvert.DeserializeObject<List<GoodData>>(jsonText);
        Good[] goodArray = new Good[good_list.Count];
        Dictionary<string, int> rgoTag = new Dictionary<string, int>();

        int id = 0;

        JsonValidator validator = new JsonValidator();

        bool isValid = validator.ValidateGoods(jsonText, validType);

        foreach (GoodData _good in good_list) 
        { 
            Good good = new Good();
            good.id = id;
            good.name = _good.name;
            good.basePrice = _good.basePrice;
            good.type = goodType.FirstOrDefault(x => x.Value == _good.type).Key;
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
