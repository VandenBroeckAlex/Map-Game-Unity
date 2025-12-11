

public interface IWorkspace
{
    void OnWorkerHired(int popId,int numberOfHired);
    void LayOffWorker(PopulationManager populationManager);
    void PayEmployees();
    void SetWages();
    void Upgrade();
    void Degrade();
    void DestroyBuiding();
    void TakeLoan();
    void BuyMaintenanceGood();
}

public interface IProductionBuilding
{
    void OutputGoods();
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
