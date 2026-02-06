using System;
using System.Collections.Generic;

public class MarketGood
{
    public int id;
    public Good good;
    private int _price;

    public int price
    {
        get => _price;
        set => _price = Math.Max(1, value);
    }

    public int supply;
    public int demand;
    public int stockpile;
    public bool isDiscovered;
    public float demandTrend;
    public float priceVolatility;

    private List<int> price_history = new List<int>();
    private List<int> demand_history = new List<int>();
    private List<int> supply_history = new List<int>();
    private List<int> stockpile_history = new List<int>();

    public void RecordGoodHistory()
    {
        price_history.Add(price);
        supply_history.Add(supply);
        demand_history.Add(demand);
        stockpile_history.Add(stockpile);
    }

    public List<int> GetPriceHistory()
    {
        return price_history;
    }

    public List<int> GetDemandHistory()
    {
        return demand_history;
    }

    public List<int> GetSupplyHistory()
    {
        return supply_history;
    }

    public List<int> GetStockpileHistory()
    {
        return stockpile_history;
    }

    //average of the difference between each price history
    //The bigger it is the more volatile
    // -1 if a total year wanted
    public float Price_volatility(int num_of_year)
    {
        if (num_of_year < 0 || num_of_year * 12 < price_history.Count) { num_of_year = price_history.Count; }


        int sum_of_difference = 0;
        for (int i = 0; i < price_history.Count; i++)
        {
            if (i != 0)
            {
                sum_of_difference += Math.Abs(price_history[i - 1] - price_history[i]);
            }
        }
        return sum_of_difference / price_history.Count;
    }

    public float Demand_trend(int num_of_year)
    {
        throw new NotImplementedException();
    }


}