using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Good;


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


