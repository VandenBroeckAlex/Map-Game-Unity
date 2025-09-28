using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Country : MonoBehaviour
{
    // Start is called before the first frame update

    public int id { get; }
    public string name;
    public Color color;
    public float treasury;
    public float income; // the sum of all incom last month

    public float income_tax;
    public Country(int ID, string NAME, Color COLOR, float TREASURY )
    {
        id = ID;
        name = NAME;
        color = COLOR;
        treasury = TREASURY;
    }
    ///public float expenses;
    //public GovernmentType governmentType;
    //public Dictionary<GoodsType, float> nationalStockpile;

    //public Dictionary<Country, DiplomaticRelation> diplomaticRelations;

    

    

}
