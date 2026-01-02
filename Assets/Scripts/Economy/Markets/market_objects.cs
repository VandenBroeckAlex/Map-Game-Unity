using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static Good;
using static Market_object;

public class Market_object 
{

    [System.Serializable]

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
            Market_object.MarketGood Marketgood = goods_list
           .Where(good => good.good.id == goodId).FirstOrDefault();

            Marketgood.supply += ammount;
            Marketgood.stockpile += ammount;
        }

        public void SubstractGoodStockpile(int goodId, int ammount)
        {
            Market_object.MarketGood Marketgood = goods_list
           .Where(good => good.good.id == goodId).FirstOrDefault();

            Marketgood.demand += ammount;
            Marketgood.stockpile -= ammount;
        }
    }

    [System.Serializable]
    public class MarketGood
    {
        public int id;
        public Good good;
        private int _price;

        public int price
        {
            get => _price;
            set => _price = Mathf.Max(1, value);
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
            if (num_of_year < 0 || num_of_year * 12 < price_history.Count ) { num_of_year = price_history.Count; }


            int sum_of_difference = 0;
            for (int i = 0; i < price_history.Count; i++)
            {
                if( i != 0)
                {
                    sum_of_difference += Mathf.Abs(price_history[i - 1] - price_history[i]);
                }
            }
            return sum_of_difference/ price_history.Count;
        }

        public float Demand_trend(int num_of_year)
        {
            throw new NotImplementedException();
        }


    } 


    // business object
    public class MarketBuyRequest
    {
        public int Id;
        public int marketId;
        public List<GoodBuyRequest> GoodRequest; 
        public int cashAmount;
    }

    public class MarketSellRequest
    {
        public int Id;
        public int marketId;
        public GoodSellRequest goodSell;

        public MarketSellRequest(int id, int marketId, GoodSellRequest goodSell)
        {
            Id = id;
            this.marketId = marketId;
            this.goodSell = goodSell;
        }
    }


    public class GoodBuyRequest
    {
        public int goodId;
        public int amountWanted;
        public GoodBuyRequest(int goodId, int amountWanted)
        {
            this.goodId = goodId;
            this.amountWanted = amountWanted;
        }
    }

    public class GoodSellRequest
    {
        public int goodId;
        public int amountsell;
        public GoodSellRequest(int goodId, int amountsell)
        {
            this.goodId = goodId;
            this.amountsell = amountsell;
        }
    }

    public class MarketSellResponse 
    {
        public int Id;
        public int cashRecived;
    }


    public class MarketBuyResponse
    {
        public int id; 
        public List<GoodResponse> goodsBought;
        public int cashLeft;
    }

    public class GoodResponse
    {
        public int goodId;
        public int amountBought;
    }

}
