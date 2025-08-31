using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goods : MonoBehaviour
{
    [System.Serializable]
    public class Good : ScriptableObject
    {
        public string goodName;
        public GoodType type; // Raw, Manufactured, Luxury
        public float basePrice;
        public float weight; // For transport, if needed
        //icon
    }
    
}
public enum GoodType
{
    Raw, Manufactured, Luxury
}

                    
