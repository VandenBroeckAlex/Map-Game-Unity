using System.Collections.Generic;
using System.Linq;

public class WorkplaceInstance
{
    // Contextual Info
    private int id;
    public int TemplateId { get; private set; }
    public int tileId { get; private set; }
    public int provinceId { get; private set; }
    public int marketId { get; private set; }


    // Dynamic State
    public bool canProduce;
    public bool isDamaged;
    public int size { get; set; } = 1;
    public float efficiency { get; private set; } = 1.0f;
    public int cashPool;
    public int companyId;
    //Stockpile (input and output)?
    public Dictionary<int, int> inputGoods;
    public Dictionary<int, int> maintenanceGoods;
    // Tracks current employment: JobType -> Current Count
    public Dictionary<int, int> currentWorkers { get; set; } = new();
    public Dictionary<int, int> owners; //idnum - num

   
    public WorkplaceInstance(int id,int templateId, int tileId, int provinceId)
    {
        this.id = id;
        this.TemplateId = templateId;
        this.tileId = tileId;
        this.provinceId = provinceId;
    }

    // Upgrade logic: swap out the template ID 
    public void Upgrade(WorkplaceTemplate newTemplate)
    {
        if (newTemplate != null)
        {
            TemplateId = newTemplate.id;
            // Check for change in worker need 
        }
    }

    //intent
    

    public float CalculateEmploymentRatio(WorkplaceTemplate template)
    {
        float totalNeeded = 0;
        float totalHired = 0;

        foreach (var req in template.workerRequirements)
        {
            int maxAllowed = req.Value * size;
            currentWorkers.TryGetValue(req.Key, out int hired);

            totalNeeded += maxAllowed;
            totalHired += hired;
        }

        return totalNeeded > 0 ? totalHired / totalNeeded : 0;
    }

    public void AddCash(int cash)
    {
        this.cashPool += cash;
    }

    public void AddGood(int id, int amount, WorkplaceTemplate template)
    {
        //check if good in maintenance
        ResourceRequirement? templateMG = template.maintenanceGoods
        .Cast<ResourceRequirement?>()
        .FirstOrDefault(wt => wt.Value.goodId == id);

        if (templateMG is not null)
        {
            int maxNeed = (int)(templateMG.Value.baseAmount * size);
            if (maintenanceGoods[id] < maxNeed)
            {
                int total = maintenanceGoods[id] + amount;

                if (total <= maxNeed )
                {
                    maintenanceGoods[id] = total;
                    amount = 0;
                }
                else
                {
                    maintenanceGoods[id] = maxNeed;
                    amount = total - maxNeed; // Return the overflow amount
                }
            }
        }


        if(inputGoods.TryGetValue(id, out int _ammont) && amount > 0)
        {
            inputGoods[id] += amount;
        }
        
    }

    public int GetWorkplaceId()
    {
        return id;
    }

    public int GetMarketId()
    {
        return marketId;
    }

    public bool HaveAllMaintenanceGoods(WorkplaceTemplate template)
    {
        foreach (KeyValuePair<int,int> kvp in maintenanceGoods)
        {
            ResourceRequirement? good  = template.maintenanceGoods
                .Cast<ResourceRequirement?>()
                .Where(mg => mg.Value.goodId == kvp.Key).First();

            if (good is null) { 
                continue;
            }
            int maxNeed = (int)(good.Value.baseAmount * size);

            if (maxNeed > kvp.Value)
            {
                return false;
            }
        }
        return true;
    }
   
    
    private int UpdateStockpileInList(IEnumerable<GoodRequirement> list, int id, int amount)
    {
        foreach (var gr in list)
        {
            if (gr.good_id == id)
            {
                int total = gr.stockpile + amount;

                if (total <= gr.maxNeed * size)
                {
                    gr.stockpile = total;
                    return 0; // Everything fits, nothing left to carry over
                }
                else
                {
                    gr.stockpile = gr.maxNeed * size;
                    return total - gr.maxNeed; // Return the overflow amount
                }
            }
        }
        return amount; // ID wasn't found in this list, return original amount
    }
} 

