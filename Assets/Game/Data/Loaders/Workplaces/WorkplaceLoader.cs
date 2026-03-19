
using Newtonsoft.Json.Linq;
using PlasticGui.WorkspaceWindow.Home;
using System.Collections.Generic;
using static DTOTile;

public class WorkplaceLoader
{
    public class DefinitionWorkplace
    {
        string name;
        string type;
        int constructionIC;
        //maintenanceCost
        //workers string(type) int(max)
        int maxWorkersAmmount;
        DefinitionWorkplace(string name,string type,int maxWorkersAmmount)
        {
            this.name = name;
            this.type = type;
            this.maxWorkersAmmount = maxWorkersAmmount;
        }
    }

    public class DefinitionCropWorkplace
    {
        public DefinitionWorkplace workplace;
        public int efficiency;
        public string output;
        public string[] climat;
    }
    public class DefinitionMiningWorkplace
    {
        public DefinitionWorkplace workplace;
        public int efficiency;
        public string output;
    }

    public class DefinitionFactoryWorkplace
    {
        public DefinitionWorkplace workplace;
        public int efficiency;
        public string output;
        public string[] input;
        //formula xA + xB = zC
    }
    public class DefinitionServiceWorkplace
    {
        public DefinitionWorkplace workplace;
        public int efficiency;
        public string output;
        public string[] input;
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
    private DefinitionWorkplace CreateWorplace(JObject item)
    {
        LandTileDTO lTD = item.ToObject<LandTileDTO>();
    }
}
