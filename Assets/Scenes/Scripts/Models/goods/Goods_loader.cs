using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System.IO;

public class Goods_loader : MonoBehaviour 
{


    [SerializeField]
    public static List<Goods.Good> allGoodsDefinition;

    public static List<Goods.Good> Load_goods()
    {
        string _FilePath = FilePath.Goods;
        Debug.Log("File Path: " + _FilePath);
        string jsonText = File.ReadAllText(_FilePath);
        List<Goods.Good>  good_list = JsonConvert.DeserializeObject<List<Goods.Good>>(jsonText);
        allGoodsDefinition = good_list;






        return good_list;

    }

   

}
