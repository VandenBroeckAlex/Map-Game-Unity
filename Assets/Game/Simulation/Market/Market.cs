
using System.Collections.Generic;
using System.Linq;
using static MarketTransactionsObj;
public class Market
{
    public int id;
    public int countryId;
    public float CashAmount = 0;
    public List<MarketGood> goods_list = new();
    private float owner_Income_tax = 0.05f;

    private List<int> monthMoneyCreationHistory;
    private int monthMoneyCreation;


    public void SetOwnerIncomeTax(float _owner_Income_tax)
    {
        owner_Income_tax = _owner_Income_tax;
    }
    public float GetOwner_Income_tax()
    {
        return owner_Income_tax;
    }
    public void AddGoodStockpile(int goodId, int ammount)
    {
        MarketGood Marketgood = goods_list
        .Where(good => good.good.id == goodId).FirstOrDefault();

        Marketgood.supply += ammount;
        Marketgood.stockpile += ammount;
    }
    public void RetrieveGood(int goodId, int ammount)
    {
        MarketGood Marketgood = goods_list
        .Where(good => good.good.id == goodId).FirstOrDefault();

        Marketgood.demand += ammount;
        Marketgood.stockpile -= ammount;
    }
    public void TrackMoneyCreation(int ammount)
    {
        monthMoneyCreation += ammount;
    }
    public int GetTotalMoneyCreation()
    {
        int response = monthMoneyCreation;
        foreach (int value in monthMoneyCreationHistory)
        {
            response += value;
        }
        return response;
    }
    public void RecordMonthData()
    {
        RecordMonthMoneyCreation();
        RecordGoodTrends();
    }
    private void RecordMonthMoneyCreation()
    {
        monthMoneyCreationHistory.Add(monthMoneyCreation);
        monthMoneyCreation = 0;
    }
    private void RecordGoodTrends()
    {
        foreach (MarketGood good in goods_list) 
        {
            good.RecordGoodHistory();
        }
    }
}