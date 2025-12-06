using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Workplace;

public class ResourceGatheringOperation 
{
    ProductionWorkplace productionWorkplace;
    public Goods outputGoods;
    public int rgoRequirment;


    public ResourceGatheringOperation(int _provinceId, int _workplaceId,
        List<GoodRequirement> _goodConstructionCost,
        List<GoodRequirement> _maintenanceCost,
        List<WorkerRequirment> _workers,
        float wageMultiplier,
        int _cashBuffer,
        int _cashBufferMax,
        int _efficiency,
        Dictionary<int, int> _owner,
        Goods _outputGoods)
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
        
    }
}
