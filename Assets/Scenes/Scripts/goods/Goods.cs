using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Goods 
{
    [System.Serializable]
    public class Good
    {
        public int id;
        public string name;
        public GoodType type; // Raw, Manufactured, Luxury
        public float basePrice;
        public float weight; // For transport, if needed
        public string iconPath;
        //public float price;
        //icon
    }
    
}
public enum GoodType
{
    Raw, Manufactured, Luxury, Military
}

                    
