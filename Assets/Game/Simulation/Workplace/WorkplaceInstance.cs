using System.Collections.Generic;
using System.Linq;

public class WorkplaceInstance
{
    // Contextual Info
    private int id;
    public int TemplateId { get; private set; }
    public int tileId { get; private set; }
    public int provinceId { get; private set; }
    public int countryId { get; private set; }
    public int marketId { get; private set; }
    //keep trak of profit

    // Dynamic State
    public bool canProduce;
    public bool isDamaged;
    public bool isOpen;

    public int size { get; set; } = 1;
    public float efficiency { get; private set; } = 1.0f;
    public int cashPool;
    public int companyId;
    public int wages;
    //public int poorStrataWage = 100;
    //public int middleStrataWage = 200;

    //Stockpile (input and output)?
    public List<GoodRequirement> inputGoodsStockpile;
    public List<GoodRequirement> maintenanceGoodsStockpile;
    // Tracks current employment: JobType -> Current Count
    // id current
    public List<IdNum> currentWorkers { get; set; } = new();
    public List<IdNum> owners; 

   
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
    
    public List<JobOffer> JobOffers(WorkplaceTemplate template)
    {
        List <JobOffer> jobs = new List <JobOffer>();
        foreach (var req in template.workerRequirements)
        {
            int maxAllowed = req.Value * size;
            IdNum worker = GetIdNumInList(req.Key, currentWorkers);
            int openPosition = maxAllowed - worker.num;
            if (openPosition > 0)
            {
                JobOffer offer = new JobOffer();
                offer.workplaceId = id;
                offer.provinceId = provinceId;
                offer.wage = wages;
                offer.popType = worker.id;
                offer.openPositions = openPosition;
                jobs.Add(offer);
            }
        }
        return jobs;
    }

    public List<PopFired> FirePop(int percentage)
    {
        List<PopFired> result = new List<PopFired>();
        foreach (IdNum idNum in currentWorkers)
        {
            int amount = (int)((idNum.num / 100) * percentage);

            idNum.num -= amount;//remove amount from workplace

            PopFired popFired = new PopFired();
            popFired.amount = amount;
            popFired.popId = idNum.id;
            popFired.workplaceId = id;
            result.Add(popFired);
        }
        return result;
    }

    public void HirePop(int popId, int PopType, int ammount)
    {
        IdNum idNum = currentWorkers.Where(i => i.id == popId).FirstOrDefault();

        if (idNum != null) 
        {
            idNum.num += ammount;
        }
        else
        {
           idNum = new IdNum(popId,ammount);
        }
    }

    public List<IdNum> PayWorkers()
    {
        List <IdNum> result = new List<IdNum >();
        foreach (IdNum idWorker in currentWorkers) 
        {
            int cashLeft = GetCash();
            if(cashLeft <= 0)
            {
                break;
            }

            int amount = ((idWorker.num / 1000) * wages);
            if (cashLeft > amount)
            {
                IdNum popPay = new IdNum(idWorker.id, amount);
                result.Add(popPay);
                UpdateCash(-amount);
            }
            else
            {
                IdNum popPay = new IdNum(idWorker.id, cashLeft);
                result.Add(popPay);
                UpdateCash(-cashLeft);
            }
        }
        return result;
    }

    public float CalculateEmploymentRatio(WorkplaceTemplate template)
    {
        float totalNeeded = 0;
        float totalHired = 0;

        foreach (var req in template.workerRequirements)
        {
            int maxAllowed = req.Value * size;
            IdNum worker = GetIdNumInList(req.Key, currentWorkers);
            
            totalNeeded += maxAllowed;
            totalHired += worker.num;
        }

        return totalNeeded > 0 ? totalHired / totalNeeded : 0;
    }

    public void UpdateCash(int cash)
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
            GoodRequirement maintenanceGoodrequirement = GetGoodRequirementInList(id, maintenanceGoodsStockpile);

            if (maintenanceGoodrequirement.stockpile < maxNeed)
            {
                int total = maintenanceGoodrequirement.stockpile + amount;

                if (total <= maxNeed )
                {
                    maintenanceGoodrequirement.stockpile = total;
                    amount = 0;
                }
                else
                {
                    maintenanceGoodrequirement.stockpile = maxNeed;
                    amount = total - maxNeed; // Return the overflow amount
                }
            }
        }

        GoodRequirement inputGoodrequirement = GetGoodRequirementInList(id, inputGoodsStockpile);
        if (inputGoodrequirement is not null && amount > 0)
        {
            inputGoodrequirement.stockpile += amount;
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
        foreach (GoodRequirement gr in maintenanceGoodsStockpile)
        {
            ResourceRequirement? good  = template.maintenanceGoods
                .Cast<ResourceRequirement?>()
                .Where(mg => mg.Value.goodId == gr.good_id).First();

            if (good is null) { 
                continue;
            }
            int maxNeed = (int)(good.Value.baseAmount * size);

            if (maxNeed > gr.stockpile)
            {
                return false;
            }
        }
        return true;
    }
   
    public Dictionary<int,int> CreateStockpileRequirement(List<ResourceRequirement> requirements)
    {
        Dictionary<int,int> stockpile = new Dictionary<int,int>();

        foreach(ResourceRequirement requirement in requirements)
        {
            int amount = requirement.baseAmount;
            int id = requirement.goodId;

            stockpile[id] = amount;
        }
        return stockpile;
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

    private IdNum? GetIdNumInList(int id,List<IdNum> list)
    {
        foreach(IdNum idnum in list)
        {
            if(idnum.id == id)
            {
                return idnum;
            }
        }
        return null;
    }

    private GoodRequirement? GetGoodRequirementInList(int id,List<GoodRequirement> list)
    {
        foreach(GoodRequirement gr  in list)
        {
            if(gr.good_id == id)
            {
                return gr;
            }
        }
        return null;
    }

    public int GetCash()
    {
        //Get cash in bank
        return cashPool;
    }
} 

