using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System.IO;

public class Goods_loader : MonoBehaviour 
{


    [SerializeField]
    public static List<Good> allGoodsDefinition;

    public static List<Good> Load_goods()
    {
        Debug.Log("Is initializing goods");
        string _FilePath = FilePath.Goods;
        string jsonText = File.ReadAllText(_FilePath);
        List<Good>  good_list = JsonConvert.DeserializeObject<List<Good>>(jsonText);
        allGoodsDefinition = good_list;
        return good_list;

    }

   

}
