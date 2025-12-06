using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Workplace;

public class ResourceGatheringOperation 
{
    ProductionWorkplace productionWorkplace;
    public Goods outputGoods;
    public int rgoRequirment;


    public ResourceGatheringOperation(ProductionWorkplace _productionWorkplace,
        Goods _outputGoods)
    {
        productionWorkplace = _productionWorkplace;
        outputGoods = _outputGoods;
    }
}
