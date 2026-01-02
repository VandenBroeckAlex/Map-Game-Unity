using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Workplace;

public class Factory //: IWorkspace, IProductionBuilding, ITransformationBuilding
{
    public Workplace workplace;
    public Production productionWorkplace;
    public Dictionary<string, int> inputGoods;
    public Dictionary<string, int> outputGoods;

    //Type of production
    //Type of remuneration 



    public Factory(
        Production _productionWorkplace,
        Dictionary<string, int> _inputGoods,
        Dictionary<string, int> _outputGoods
        )
    {
        productionWorkplace = _productionWorkplace;
        inputGoods = _inputGoods;
        outputGoods = _outputGoods;
    }



    public virtual void Produce(Dictionary<string, int> goodsStockpile, int workerCount)
    {
        // Check Input
        if (workplace.type == WorkplaceType.Factory)
        {
            if (CanProduce(goodsStockpile, workerCount))
            {
                foreach (var input in inputGoods)
                    goodsStockpile[input.Key] -= input.Value;

                foreach (var output in outputGoods)
                    goodsStockpile[output.Key] += output.Value;
            }
        }
    }

    private bool CanProduce(Dictionary<string, int> goodsStockpile, int workerCount)
    {

        foreach (var input in inputGoods)
            if (!goodsStockpile.ContainsKey(input.Key) || goodsStockpile[input.Key] < input.Value)
                return false;
        return true;
    }

    public void OnWorkerHired(int popId, int numberOfHired)
    {
        throw new System.NotImplementedException();
    }

    public void LayOffWorker()
    {
        //if can not get good for effective production fire proportionnaly
        throw new System.NotImplementedException();
    }

    public void OutputGoods()
    {
        throw new System.NotImplementedException();
    }

    public void BuyInputGoods()
    {
        throw new System.NotImplementedException();
    }

    public void SellGood(int goodId, int ammount)
    {
        throw new System.NotImplementedException();
    }

    public void PayEmployees()
    {
        throw new System.NotImplementedException();
    }

    public void SetWages()
    {
        throw new System.NotImplementedException();
    }

    public void Upgrade()
    {
        throw new System.NotImplementedException();
    }

    public void Degrade()
    {
        throw new System.NotImplementedException();
    }

    public void DestroyBuiding()
    {
        throw new System.NotImplementedException();
    }

    public void TakeLoan()
    {
        throw new System.NotImplementedException();
    }

    public void BuyMaintenanceGood()
    {
        throw new System.NotImplementedException();
    }
}
 