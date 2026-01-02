

using System.Collections.Generic;
using static Market_object;
using static Workplace;

public interface IWorkplace
{
    int getWorkplaceId();
    void OnWorkerHired(int popId,int numberOfHired, Pop_objects.PopJob job);
    void OnWorkerLeave(int popId,int numberOfLeaving);
    List<IdNum> LayOffWorker();
    List<IdNum> PayEmployees();
    List<IdNum>? PayOwners();
    void SetWages();
    void Upgrade();
    void Degrade();
    void DestroyBuiding();
    void TakeLoan();
    void BuyMaintenanceGood();

    void ReciveCash(int ammount);
}

public interface IProductionBuilding
{
    MarketSellRequest SellRequest();
}

public interface ITransformationBuilding
{
    void BuyInputGoods();
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
