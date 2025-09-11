using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System.IO;

public class Goods_loader   
{
    // Start is called before the first frame update

    private static string _FilePath = FilePath.Goods;

    public static List<Goods.Good> Load_goods()
    {
        Debug.Log("File Path: " + _FilePath);
        string jsonText = File.ReadAllText(_FilePath);
        List<Goods.Good>  good_list = JsonConvert.DeserializeObject<List<Goods.Good>>(jsonText);
      return good_list;

    }

}
