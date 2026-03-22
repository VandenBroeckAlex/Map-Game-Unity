

using System.Collections.Generic;

public class DTOWorkplace
{
    public class DTONnumPerTag
    {
        public string tag;
        public int num;
    }
    public class WorkplaceDTO
    {
        public string countryTag;
        public string name;
        public string type;
        public int constructionCost;
        public Dictionary<string,int> goodConstructionCost;
        public Dictionary<string, int> goodmaintenanceCost;
        public Dictionary<string, int> workersType;
    }
    public class ProductionWorkplaceDTO : WorkplaceDTO
    {
        public int efficiency; //how much of output produce if all input 100%
        public int defaultCashBufferMax;
    }

    public class CropRGODTO : ProductionWorkplaceDTO
    {
        public string outputGood;
        public string climateType;
    }
    public class MineralRGODTO : ProductionWorkplaceDTO
    {
        public string outputGood;
    }

    public class FactoryBuildingDTO : ProductionWorkplaceDTO
    {
        public string outputGood; 
        public string inputGood; //string + number
    }
    public class ServiceBuildingDTO : ProductionWorkplaceDTO
    {
        public string outputedService;
    }
    public class InfrastructureBuildingDTO : ProductionWorkplaceDTO 
    {
        public string inputGood;
    }
    //military
}





public class DTOWorkplaceDef
{
 
    public class WorkplaceDTODef
    {
        public string name;
        public string type;
        public int constructionCost;
        public Dictionary<string, int> goodConstructionCost;
        public Dictionary<string, int> goodmaintenanceCost;
        public Dictionary<string, int> workersType;
    }
    public class ProductionWorkplaceDTODef : WorkplaceDTODef
    {
        public int efficiency; //how much of output produce if all input 100%
        public int defaultCashBufferMax;
    }

    public class CropRgoDtoDef : ProductionWorkplaceDTODef
    {
        public string outputGood;
        public string[] valid_climate;
    }
    public class MineralRgoDtoDef : ProductionWorkplaceDTODef
    {
        public string outputGood;
    }

    public class FactoryBuildingDTODef : ProductionWorkplaceDTODef
    {
        public string outputGood;
        public Dictionary<string,int> inputGood; //string + number
    }
    public class ServiceBuildingDTODef : ProductionWorkplaceDTODef
    {
        public string outputedService;
    }
    public class InfrastructureBuildingDTODef : ProductionWorkplaceDTODef
    {
        public Dictionary<string,int> inputGood;
    }
    //military
}
