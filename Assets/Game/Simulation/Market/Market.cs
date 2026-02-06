
using System.Collections.Generic;
using System.Linq;

public class Market
{
    public int id;
    public int countryId;
    public float CashAmount = 0;
    public List<MarketGood> goods_list = new();
    private float owner_Income_tax = 0.05f;
    public void SetOwnerIncomeTax(float _owner_Income_tax)
    {
        owner_Income_tax = _owner_Income_tax;
    }
    public float GetOwner_Income_tax()
    {
        return owner_Income_tax;
    }

    //public MarketSellResponse SellGood(MarketSellRequest sellRequest) 
    //{
    //    //create the response object
    //    MarketSellResponse response = new MarketSellResponse();
    //    //search for good
    //    int goodId = sellRequest.goodSell.goodId;         

    //    Market_object.MarketGood Marketgood = goods_list
    //    .Where(good => good.good.id == goodId).FirstOrDefault();
    //    //country tax

    //    int BrutCash = Marketgood.price * sellRequest.goodSell.amountsell;
    //    float country_income_tax = owner_Income_tax;
    //    int country_income = (int)(BrutCash * country_income_tax); //country_income_tax * province controle * admin capacity
    //    // NetCash
    //    response.cashRecived = BrutCash - country_income;
    //    //batch_response.Add(response);

    //    //Marketgood.supply += marketSellRequestList[i].goodSell.amountsell;
    //    //Marketgood.stockpile += marketSellRequestList[i].goodSell.amountsell;
    //}

    public void AddGoodStockpile(int goodId, int ammount)
    {
        MarketGood Marketgood = goods_list
        .Where(good => good.good.id == goodId).FirstOrDefault();

        Marketgood.supply += ammount;
        Marketgood.stockpile += ammount;
    }

    public void SubstractGoodStockpile(int goodId, int ammount)
    {
        MarketGood Marketgood = goods_list
        .Where(good => good.good.id == goodId).FirstOrDefault();

        Marketgood.demand += ammount;
        Marketgood.stockpile -= ammount;
    }
}