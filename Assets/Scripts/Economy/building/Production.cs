using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Workplace;

public class Production 
{

    public int cashBuffer;
    public int cashBufferMax;
    public int efficiency;
    

    public Production(int _provinceId, int _workplaceId,
        List<GoodRequirement> _goodConstructionCost,
        List<GoodRequirement> _maintenanceCost,
        List<WorkerTypeCurrentMax> _workers,
        float wageMultiplier,
        int _cashBuffer,
        int _cashBufferMax,
        int _efficiency
        ) 
    {
        cashBuffer = _cashBuffer;
        cashBufferMax = _cashBufferMax;
        efficiency = _efficiency;
    }


}
