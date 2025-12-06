using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Workplace;

public class Factory 
{
    public ProductionWorkplace productionWorkplace;
    public Dictionary<string, int> inputGoods;
    public Dictionary<string, int> outputGoods;

    //Type of production
    //Type of remuneration

    public Factory(
        int _provinceId, int _workplaceId,
        List<GoodRequirement> _goodConstructionCost,
        List<GoodRequirement> _maintenanceCost,
        List<WorkerRequirment> _workers,
        float wageMultiplier,
        int _cashBuffer,
        int _cashBufferMax,
        int _efficiency,
        Dictionary<int, int> _owner,
        Dictionary<string, int> _inputGoods,
        Dictionary<string, int> _outputGoods
        )
    { 
        productionWorkplace = new ProductionWorkplace(
                     _provinceId, 
                     _workplaceId,
                      _goodConstructionCost,
                     _maintenanceCost,
                     _workers,
                     wageMultiplier,
                     _cashBuffer,
                     _cashBufferMax,
                     _efficiency,
                     _owner);
        inputGoods = _inputGoods;
        outputGoods = _outputGoods;
    }



    public virtual void Produce(Dictionary<string, int> goodsStockpile, int workerCount)
    {
        // Check Input
        if (productionWorkplace.workplace.type == WorkplaceType.Factory)
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
}
 