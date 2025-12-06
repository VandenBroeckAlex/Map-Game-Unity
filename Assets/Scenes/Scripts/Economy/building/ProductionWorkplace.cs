using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Workplace;

public abstract class ProductionWorkplace 
{
    public Workplace workplace;
    public int cashBuffer;
    public int cashBufferMax;
    public int efficiency;
    public Dictionary<int, int> owner;

    ProductionWorkplace(int _provinceId, int _workplaceId,
        List<GoodRequirement> _goodConstructionCost,
        List<GoodRequirement> _maintenanceCost,
        List<WorkerRequirment> _workers,
        float wageMultiplier,
        int _cashBuffer,
        int _cashBufferMax,
        int _efficiency,
        Dictionary<int,int> _owner
        ) 
    {
        workplace = new Workplace(
         _provinceId, _workplaceId,
         _goodConstructionCost,
        _maintenanceCost,
        _workers,
         wageMultiplier);

        cashBuffer = _cashBuffer;
        cashBufferMax = _cashBufferMax;
        efficiency = _efficiency;
        owner = _owner;
    }


}
