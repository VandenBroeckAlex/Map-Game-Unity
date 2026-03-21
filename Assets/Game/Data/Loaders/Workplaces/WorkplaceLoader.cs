
using Newtonsoft.Json.Linq;
using PlasticGui.WorkspaceWindow.Home;
using System.Collections.Generic;
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
        
        public DefinitionWorkplace(string name,string type,int ic, Dictionary<int, int> workers)
        {
            this.name = name;
            this.type = type;
            this.constructionIC = ic;
            this.workers = workers;
        }
    }

    public class DefinitionCropWorkplace
    {
        public DefinitionWorkplace workplace;
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
            )
        {
            DefinitionWorkplace workplace = new DefinitionWorkplace(name, type, icCost, workers);
            this.workplace = workplace;
            this.efficiency = efficiency;
            this.output = ouputId;
            this.climat = climat;
        }
    }
    public class DefinitionMiningWorkplace
    {
        public DefinitionWorkplace workplace;
        public int efficiency;
        public int output; //good id

        public DefinitionMiningWorkplace(
            string name,
            string type,
            int icCost, 
            int efficiency,
            int ouputId,
            Dictionary<int,int> workers
            )
        {
            DefinitionWorkplace workplace = new DefinitionWorkplace(name, type, icCost, workers);
            this.workplace = workplace;
            this.efficiency = efficiency;
            this.output = ouputId;

        }

    }

    public class DefinitionFactoryWorkplace
    {
        public DefinitionWorkplace workplace;
        public int efficiency; 
        public int output; //good id
        public Dictionary<int,int> input; //good id - num
    }
    public class DefinitionServiceWorkplace
    {
        public DefinitionWorkplace workplace;
        public int efficiency;
        public int output; //good id
        public Dictionary<int, int> input; //good id - num
        //formula xA + xB = zC
    }
    public void DeserializeWorkplaces(string json, RunTimePopJob[] validPopJobs, Dictionary<string, int> validRgoTag)
    {
        JArray listWorkplaces = JArray.Parse(json);

        foreach (JObject item in listWorkplaces) 
        { 
            if(item != null) continue;


            string type = item["type"]?.Value<string>();
            //throw if null
            type.ToLower();

            switch (type) 
            {
                case "mining":
                    
                    break;
                case "crop":
                    break;
                case "factory":
                    break;
                case "infra":
                    break;
                case "serv":
                    break;
                case "mil":
                    break;
                default:
                    //throw error
                    break;
            }
        }
    }
    

    private DefinitionMiningWorkplace CreateMinigWorkplaceDef(JObject workplace)
    {
        MineralRgoDtoDef dto = workplace.ToObject<MineralRgoDtoDef>();
        
        // check string to id
        int outputId = _registery.GetGoodIdByTagId(dto.outputGood);

        Dictionary<int, int> workers = _registery.GetWorkersDictionary(dto.jobAssignment);

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

        Dictionary<int, int> workers = _registery.GetWorkersDictionary(dto.jobAssignment);

        int[] climat = new int[dto.climateType.Length];

        for (int i = 0; i< climat.Length; i++)
        {
            climat[i] = _registery.GetClimateTagId(dto.climateType[i]);
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



  
}
