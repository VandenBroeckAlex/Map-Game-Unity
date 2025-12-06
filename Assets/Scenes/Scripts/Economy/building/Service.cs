using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Service 
{
    ProductionWorkplace productionWorkplace;
    public Dictionary<string, int> output; // research, admin capacity, literacy boost, doctor( + birth rate, less death)

    public Service(ProductionWorkplace _productionWorkplace, Dictionary<string, int> _output)
    {
        productionWorkplace = _productionWorkplace;
        output = _output;
    }
}
