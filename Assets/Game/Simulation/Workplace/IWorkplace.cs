

using System.Collections.Generic;
using static MarketTransactionsObj;



public interface IWorkplace
{
    int GetWorkplaceId();
    int GetProvinceId();
    int GetWorkAvailableByJobType(PopJob popJob);
    void OnWorkerHired(int popId,int numberOfHired, PopJob job);
    void OnWorkerLeave(int popId,int numberOfLeaving);
    List<IdNum> LayOffWorker();
    List<IdNum> PayEmployees();
    List<IdNum>? PayOwners();
    void SetWages();
    void Upgrade();
    void Degrade();
    void DestroyBuiding();
    void TakeLoan();
    void AddCash(int cash);
    MarketBuyRequest BuyMaintenanceGood();
    void ReciveCash(int ammount);
    void SetCashTo(int ammount);
    void AddGood(int id, int amount);
}

public interface IProductionBuilding
{
   MarketSellRequest SellRequest();
}

public interface ITransformationBuilding
{
    MarketBuyRequest BuyInputGoods();
}

public interface IMilitaryBuilding
{
    
}

public interface IMerchantBuilding
{
    void BuyGood();
    void SellGood();
    void SetProgitMarginTarget();
    void BuyExchangeCapacity();
}
