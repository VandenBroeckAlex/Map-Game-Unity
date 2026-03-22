
using Newtonsoft.Json.Linq;
using PlasticGui.WorkspaceWindow.Home;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using static DTOTile;
using static DTOWorkplaceDef;
using static ProvincesLoader;

public class WorkplaceLoader
{
    LoaderDataRegistery _registery;
    public WorkplaceLoader( LoaderDataRegistery registery)
    {
        _registery = registery;
    }
    public class DefinitionWorkplace
    {
        string name;
        string type;
        int constructionIC;
        Dictionary<int, int> maintenanceCost; // good id num
        Dictionary<int, int> workers; //popjob id - num
        
        protected DefinitionWorkplace(string name,string type,int ic, Dictionary<int, int> workers)
        {
            this.name = name;
            this.type = type;
            this.constructionIC = ic;
            this.workers = workers;
        }
    }

    public class DefinitionCropWorkplace : DefinitionWorkplace
    {
        public int efficiency;
        public int output; //good id
        public int[] climat; // climate id

        public DefinitionCropWorkplace(
            string name,
            string type,
            int icCost,
            int efficiency,
            int ouputId,
            Dictionary<int, int> workers,
            int[] climat
            ) : base(name,type, icCost, workers)
        {

            this.efficiency = efficiency;
            this.output = ouputId;
            this.climat = climat;
        }
    }
    public class DefinitionMiningWorkplace : DefinitionWorkplace
    {

        public int efficiency;
        public int output; //good id

        public DefinitionMiningWorkplace(
            string name,
            string type,
            int icCost, 
            int efficiency,
            int ouputId,
            Dictionary<int,int> workers
            ) : base(name, type, icCost, workers)
        {
            this.efficiency = efficiency;
            this.output = ouputId;

        }

    }

    public class DefinitionFactoryWorkplace : DefinitionWorkplace
    {
        public DefinitionWorkplace workplace;
        public int efficiency; 
        public int output; //good id
        public Dictionary<int,int> input; //good id - num

        public DefinitionFactoryWorkplace(
            string name,
            string type,
            int icCost,
            int efficiency,
            int ouputId,
            Dictionary<int, int> workers,
            Dictionary<int, int> input
            ) : base(name, type, icCost, workers)
        {
            this.efficiency = efficiency;
            this.output = ouputId;
            this.input = input;
        }
    }
    
    public class DefinitionServiceWorkplace : DefinitionWorkplace
    {
        public DefinitionServiceWorkplace(
            string name,
            string type,
            int icCost,
            int efficiency,
            int ouputId,
            Dictionary<int, int> workers,
            Dictionary<int, int> input
            ) : base(name, type, icCost, workers)
        {
            this.output = output;
            this.input = input;
        }
        public int efficiency;
        public int output; //good id
        public Dictionary<int, int> input; //good id - num
        //formula xA + xB = zC
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

        int[] climat = new int[dto.valid_climate.Length];

        for (int i = 0; i< climat.Length; i++)
        {
            climat[i] = _registery.GetClimateTagId(dto.valid_climate[i]);
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
