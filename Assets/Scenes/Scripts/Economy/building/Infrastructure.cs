using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Infrastructure 
{
    Production productionWorkplace;
    public Dictionary<string, int> outputGoods;

    public Infrastructure(Production _productionWorkplace, Dictionary<string, int> _outputGoods)
    {
        productionWorkplace = _productionWorkplace;
        outputGoods = _outputGoods;
    }
}
