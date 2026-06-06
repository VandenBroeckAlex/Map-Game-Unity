using System.Collections.Generic;

public class WorkplaceTemplate
{

    /*
    Requirement:
        technology ?
        climate ?
        presence of a RGO ?
        culture ?
        governement type ?
    */
    public string tag;
    public int id { get; set; }
    public string name { get; set; }
    public int upgradeTemplateId { get; set; } // ID of what this can upgrade into (or null)
    public int downgradeTemplateId { get; set; }
    public List<ResourceRequirement> constructionInput; //upgrade cost = Uptemplate - this.
    public int ICConstructionCost;
    public List<ResourceRequirement> maintenanceGoods;

    public Dictionary<int, int> workerRequirements { get; set; } = new(); // e.g., "Craftsman": 100, "Clerk": 10
    public List<ResourceRequirement> inputs { get; set; } = new();           // Empty for extraction (RGOs)    
    public List<ProductionEffect> outputs { get; set; } = new();
    public List<ResourceRequirement> efficiencyInput { get; set; } = new();

    public int GetMaxGoodAmountById(int id)
    {
        int result = 0;

        foreach(ResourceRequirement req in maintenanceGoods)
        {
            if(req.goodId == id)
            {
                result += req.baseAmount;
            }
        }
        if(inputs != null)
        {
            foreach (ResourceRequirement req in inputs)
            {
                if (req.goodId == id)
                {
                    result += req.baseAmount;
                }
            }
        }
        return result;
    }
}
