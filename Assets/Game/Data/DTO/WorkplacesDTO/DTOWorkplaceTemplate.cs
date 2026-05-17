

using System.Collections.Generic;

public class DTOWorkplaceTemplate
{
    //conditions
    public string tag;
    public string name;
    public int constructionCost;
    public string upgradeTemplateId;
    public string downgradeTemplateId;
    public Dictionary<string, int> goodConstructionCost;
    public Dictionary<string, int> goodmaintenanceCost;
    public Dictionary<string, int> input;
    public List<DTOProduction> output;
    public Dictionary<string, int> workersType;
}

public class DTOProduction
{
    public string type;
    public string id;
    public float baseAmount;
}