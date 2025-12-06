using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Infrastructure 
{
    ProductionWorkplace productionWorkplace;
    public Dictionary<string, int> outputGoods;

    public Infrastructure(ProductionWorkplace _productionWorkplace, Dictionary<string, int> _outputGoods)
    {
        productionWorkplace = _productionWorkplace;
        outputGoods = _outputGoods;
    }
}
