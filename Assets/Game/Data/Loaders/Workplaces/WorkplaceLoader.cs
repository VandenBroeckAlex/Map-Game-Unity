
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using static DTOWorkplaceDef;
using static WorkplacesDefinitions;

public class WorkplaceLoader
{
    DataRegistery _registery;
    public WorkplaceLoader( DataRegistery registery)
    {
        _registery = registery;
    }
    
    public List<DefinitionWorkplace> DeserializeWorkplaces(string json)
    {
        JArray listWorkplaces = JArray.Parse(json);
        List<DefinitionWorkplace> result = new List<DefinitionWorkplace>();

        foreach (JToken token in listWorkplaces)
        {

            if (token is not JObject item) continue;


            string type = item["type"]?.Value<string>();

            if (string.IsNullOrEmpty(type))
            {
                continue;
            }

            type = type.ToLower();

            switch (type)
            {
                case "mining":
                    result.Add(CreateMinigWorkplaceDef(item));
                    break;
                case "crops": 
                    result.Add(CreateCropWorkplaceDef(item));
                    break;
                case "factory":
                    result.Add(CreateFactoryWorkplace(item));
                    break;
                default:
                    throw new Exception($"Couldn't find type: {type}");
            }
        }
        return result;
    }


    private DefinitionMiningWorkplace CreateMinigWorkplaceDef(JObject workplace)
    {
        MineralRgoDtoDef dto = workplace.ToObject<MineralRgoDtoDef>();
        
        // check string to id
        int outputId = _registery.GetGoodIdByTagId(dto.outputGood);

        Dictionary<int, int> workers = _registery.GetWorkersDictionary(dto.workersType);

        DefinitionMiningWorkplace mWorkplace = new DefinitionMiningWorkplace(
            dto.name,
            dto.type,
            dto.constructionCost,
            dto.efficiency,
            outputId,
            workers
            );
        return mWorkplace;
    }

    private DefinitionCropWorkplace CreateCropWorkplaceDef(JObject workplace)
    {
        CropRgoDtoDef dto = workplace.ToObject<CropRgoDtoDef>();

        int outputId = _registery.GetGoodIdByTagId(dto.outputGood);

        Dictionary<int, int> workers = _registery.GetWorkersDictionary(dto.workersType);

        int[] climat = new int[dto.climate.Length];

        for (int i = 0; i< climat.Length; i++)
        {
            climat[i] = _registery.GetClimateTagId(dto.climate[i]);
        }

        DefinitionCropWorkplace mWorkplace = new DefinitionCropWorkplace(
            dto.name,
            dto.type,
            dto.constructionCost,
            dto.efficiency,
            outputId,
            workers,
            climat
            );
        return mWorkplace;
    }

    private DefinitionFactoryWorkplace CreateFactoryWorkplace(JObject workplace) 
    {
        FactoryBuildingDTODef dto = workplace.ToObject<FactoryBuildingDTODef>();

        int outputId = _registery.GetGoodIdByTagId(dto.outputGood);

        Dictionary<int, int> workers = _registery.GetWorkersDictionary(dto.workersType);

        Dictionary<int,int> inputGoods = _registery.GetGoodDictionary(dto.inputGood);

        DefinitionFactoryWorkplace mWorkplace = new DefinitionFactoryWorkplace(
            dto.name,
            dto.type,
            dto.constructionCost,
            dto.efficiency,
            outputId,
            workers,
            inputGoods
            );
        return mWorkplace;
    }

  
}
