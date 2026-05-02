using NUnit.Framework;
using System.Collections.Generic;

public class Country
{

    public int id;
    public int playerId;
    public string name;
    public int[] color;
    public long treasury;
    public long income; // the sum of all income last month
    public List<long> incomeHistory = new List<long>();
    public string tag;

    public float Income_tax = 0.1f;

    public CountryModdifiers stats;

    public Country(int ID, string NAME, int[] COLOR, long TREASURY, string TAG)
    {
        id = ID;
        name = NAME;
        color = COLOR;
        treasury = TREASURY;
        stats = new CountryModdifiers();
        tag = TAG;
    }

    ///public float expenses;
    //public GovernmentType governmentType;
    //public Dictionary<GoodsType, float> nationalStockpile; (should probably not be here)

    //public Dictionary<Country, DiplomaticRelation> diplomat icRelations; (should probably not be here)

    public void ReceiveCash(int cash)
    {
        treasury += cash;
        income += cash;
    }
    public void ResetIncome()
    {
        incomeHistory.Add(income);
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