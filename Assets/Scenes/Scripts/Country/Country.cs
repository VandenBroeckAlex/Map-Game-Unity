using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static CountriesManager;

[System.Serializable]
public class Country 
{
    // Start is called before the first frame update


    public int id;
    public int playerId;
    public string name;
    public Color color;
    public float treasury;
    public float income; // the sum of all income last month

    public float Income_tax = 0.1f;
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

    public void ReceiveCash(float cash )
    {
        treasury += cash;
        income += cash;
    }
    public void ResetIncome()
    {
        income = 0;
    }

    public class TaxOnGood
    {
        string type; // import - export - TVA
                     // GoodID / all ?
        float ammount; // in %
    }
    public class TaxOnEntity
    {
        string type; // pop / working places ?
        float ammount; //in %
    }

}
