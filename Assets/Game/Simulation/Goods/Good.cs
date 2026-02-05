using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Good;
using static Goods_loader;

public class Good 
{
  
    public int id;
    public string name;
    public GoodType type; // Raw, Manufactured, Luxury
    public int basePrice;
    public int weight; // For transport, if needed
    public string iconPath;
    //public float price;
    //icon
 
}
public enum GoodType
{
    Raw, Manufactured, Luxury, Military
}


public static class GoodDatabase
{
    public static List<Good> good_definition_list = new List<Good>();

    public static void Initialize()
    {
        good_definition_list = Load_goods();
    }
}