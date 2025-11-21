using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static CountriesManager;
[System.Serializable]
public class Country 
{
    // Start is called before the first frame update

    public int id;
    public string name;
    public Color color;
    public float treasury;
    public float income; // the sum of all incom last month

    public float Income_tax;
    public Country(int ID, string NAME, Color COLOR, float TREASURY )
    {
        id = ID;
        name = NAME;
        color = COLOR;
        treasury = TREASURY;
    }
    ///public float expenses;
    //public GovernmentType governmentType;
    //public Dictionary<GoodsType, float> nationalStockpile; (should probably not be here)

    //public Dictionary<Country, DiplomaticRelation> diplomat icRelations; (should probably not be here)


    public class TaxOnGood
    {
        //Type import / export 
        // GoodID / all ?
        //ammount in %
    }
    public class TaxOnEntity
    {
        //Type pop / working places ?
        //ammount in %
    }

}
