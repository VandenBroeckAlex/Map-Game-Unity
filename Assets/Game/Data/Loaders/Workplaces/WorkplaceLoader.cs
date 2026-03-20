
using Newtonsoft.Json.Linq;
using PlasticGui.WorkspaceWindow.Home;
using System.Collections.Generic;
using System.IO;
using static DTOTile;
using static DTOWorkplaceDef;
using static ProvincesLoader;

public class WorkplaceLoader
{
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
    private DefinitionMiningWorkplace CreateMinigWorkplaceDef(JObject workplace, Dictionary<string, int> validRgoTag, RunTimePopJob[] validPopJobs)
    {
        MineralRgoDtoDef dto = workplace.ToObject<MineralRgoDtoDef>();
        // check string to id
        int outputId = 0;
        if (validRgoTag.ContainsKey(dto.outputGood))
        {
             outputId = validRgoTag[dto.outputGood];
        }
        else
        {
            throw new InvalidDataException(
               $"the good: {dto.outputGood} is not valid. " +
               $"Creating workplace definition : {dto.name} , type : {dto.type}");
        }

        Dictionary<int, int> workers = GetWorkersDictionary(validPopJobs, dto.jobAssignment);

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

    
    private Dictionary<int, int> GetWorkersDictionary(RunTimePopJob[] validPopJobs, Dictionary<string, int> workers)
    {
        Dictionary<int,int> result = new Dictionary<int,int>();
        foreach (KeyValuePair<string, int> kvp in workers)
        {
            int id = GetIdByTag(validPopJobs, kvp.Key);
            if(id < 0)
            {
                throw new InvalidDataException(
              $"the pop tag: {kvp.Key} is not valid. " +
              $"While creating workplace definition");
            }
            else
            {
                result[id] = kvp.Value;
            }
        }
        return result;
    }
    private int GetIdByString(string str, string[] array)
    {
        for (int i = 0; i < array.Length; i++)
        {
            if (array[i] == str)
            {
                return i;
            }
        }
        //TODO
        throw new InvalidDataException($"");
    }
    private int GetIdByTag<T>(T[] data, string givenTag) where T : IHaveTag
    {
        for (int i = 0; i < data.Length; i++)
        {
            if (data[i].tag == givenTag)
            {
                return i;
            }
        }
        return -1;
    }
}
