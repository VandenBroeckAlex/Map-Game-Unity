
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using static DTOWorkplaceDef;
using static WorkplacesDefinitions;

public class WorkplaceLoader
{
    DataRegistery _registery;
    public WorkplaceLoader(DataRegistery registery)
    {
        _registery = registery;
    }

    public Dictionary<int, WorkplaceTemplate> DeserializeWorkplaceTemplate(string json)
    {
        Dictionary<int, WorkplaceTemplate> result = new Dictionary<int, WorkplaceTemplate>();
        Dictionary<string,int> tagToId = new Dictionary<string, int>();
        DTOWorkplaceTemplate[] data = JsonConvert.DeserializeObject<DTOWorkplaceTemplate[]>(json);

        if (data is null || data.Length == 0)
        {
            throw new InvalidDataException("population.json is empty");
        }
        int indexer = 0;
        foreach (DTOWorkplaceTemplate dataItem in data)
        {
            tagToId[dataItem.tag] = indexer;
            //resolve 
            WorkplaceTemplate wt = CreateWorkplaceTemplate(dataItem);
            wt.id = indexer;

            result[indexer] = wt;
            indexer++;
        }

        //resolve update - degrade id
        //TODO try get
        foreach (DTOWorkplaceTemplate dataItem in data)
        {
            int id = tagToId[dataItem.tag];

            WorkplaceTemplate template = result[id];

            if(dataItem.upgradeTemplateId is not null && tagToId.TryGetValue(dataItem.upgradeTemplateId, out int upgrade))
            {
                template.upgradeTemplateId = upgrade;
            }
            if(dataItem.downgradeTemplateId is not null && tagToId.TryGetValue(dataItem.downgradeTemplateId, out int downgrade))
            {
                template.downgradeTemplateId = downgrade;
            }
        }
            
        return result;
    }

    private WorkplaceTemplate CreateWorkplaceTemplate(DTOWorkplaceTemplate dto)
    {

        Dictionary<int, int> workers = _registery.GetWorkersDictionary(dto.workersType);
        Dictionary<int, int> inputGoods = _registery.GetGoodDictionary(dto.input);
        List<ProductionEffect> output = ProdEffectDtoToRuntime(dto.output);
        Dictionary<int, int> construGoods = _registery.GetGoodDictionary(dto.goodConstructionCost);
        Dictionary<int, int> maintenanceGoods = _registery.GetGoodDictionary(dto.goodmaintenanceCost);



        WorkplaceTemplate wt = new WorkplaceTemplate();
        wt.tag = dto.tag;
        wt.name = dto.name;
        wt.ICConstructionCost = dto.constructionCost;
        wt.workerRequirements = workers;
        wt.constructionInput = DictToRequirement(construGoods);
        wt.maintenanceGoods = DictToRequirement(maintenanceGoods);
        wt.inputs = DictToRequirement(inputGoods);
        wt.outputs = output;

        return wt;
    }

    private List<ResourceRequirement> DictToRequirement(Dictionary<int, int> dict)
    {
        List<ResourceRequirement> result = new List<ResourceRequirement>();

        foreach(KeyValuePair<int, int> kvp in dict)
        {
            ResourceRequirement req = new ResourceRequirement();

            req.goodId = kvp.Key;
            req.baseAmount = kvp.Value;
            result.Add(req);
        }

        return result;
    }

    private List<ProductionEffect> ProdEffectDtoToRuntime(List<DTOProduction> data)
    {
        List<ProductionEffect> result = new List<ProductionEffect>();

        foreach (DTOProduction prod in data) 
        {
            ProductionEffect pe = new ProductionEffect();
            Enum.TryParse("Active", out OutputDomain domain);

            if(domain == null)
            {
                //Raise Error
            }

            pe.type = domain;

            switch (domain)
            {
                case OutputDomain.Market:
                    // id of good
                    pe.targetId = _registery.GetGoodIdByTagId(prod.id);
                    break;
            }
            pe.baseAmount = prod.baseAmount;
            result.Add(pe);
        }
        return result;
    }

   
}
