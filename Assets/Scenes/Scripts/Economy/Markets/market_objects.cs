using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Goods;

public class Market_object 
{

    [System.Serializable]

    public class Market
    {
        public int id;
        public int countryId;
        public float CashAmount = 0;
        public List<MarketGood> goods_list = new();
        //owner tax
        private float owner_Income_tax = 0.05f;

        public void SetOwnerIncomeTax(float _owner_Income_tax)
        {
            owner_Income_tax = _owner_Income_tax; 
        }
        public float GetOwner_Income_tax()
        {
            return owner_Income_tax;
        }
    }

    [System.Serializable]
    public class MarketGood
    {
        public int id;
        public Good good;
        public int price 
        {
            get { return price; }
            set 
            {  
                if(value < 1) {  price = 1; }
                else {  price = value; }
            }
        }
        
        public int supply;
        public int demand;
        public int stockpile;
        public bool isDiscovered;
        public List <int> price_history = new List<int>();
        public List<int> demand_history = new List<int>();
        public List<int> supply_history = new List<int>();
        public List<int> stockpile_history = new List<int>();

        public void RecordGoodHistory()
        {
            price_history.Add(price);
            supply_history.Add(supply);
            demand_history.Add(demand);
            stockpile_history.Add(stockpile);
        }
    }

    



    // business object

    public class MarketBuyRequest
    {
        public int popId;
        public int marketId;
        public List<GoodBuyRequest> GoodRequest; // Turn it into an array
        public int cashAmount;
    }

    public class MarketSellRequest
    {
        public int popId;
        public int marketId;
        //pop type
        public GoodSellRequest goodSell;
    }


    public class GoodBuyRequest
    {
        public int goodId;
        public int amountWanted;
    }

    public class GoodSellRequest
    {
        public int goodId;
        public int amountsell;
    }

    public class MarketSellResponse 
    {
        public int popId;
        public int cashRecived;
    }


    public class MarketResponse
    {
        public int popId; 
        public List<GoodResponse> goodsBought;
        public int cashLeft;
    }

    public class GoodResponse
    {
        public int goodId;
        public int amountBought;
    }

}
