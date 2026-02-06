using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.IO;

public class GoodLoader  
{

    public static List<Good> allGoodsDefinition;

    public static List<Good> Load_goods()
    {
        string _FilePath = FilePath.Goods;
        string jsonText = File.ReadAllText(_FilePath);
        List<Good>  good_list = JsonConvert.DeserializeObject<List<Good>>(jsonText);
        allGoodsDefinition = good_list;
        return good_list;
    }

   

}
