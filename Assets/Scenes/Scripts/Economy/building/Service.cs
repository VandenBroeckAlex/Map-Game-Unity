using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Service 
{
    Production productionWorkplace;
    public Dictionary<string, int> output; // research, admin capacity, literacy boost, doctor( + birth rate, less death)

    public Production Production => productionWorkplace;
    public Workplace workplace => productionWorkplace.workplace;

    public Service(Production _productionWorkplace, Dictionary<string, int> _output)
    {
        productionWorkplace = _productionWorkplace;
        output = _output;
    }
}
