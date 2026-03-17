

public class DTOWorkplace
{
    public class WorkplaceDTO
    {
        public string countryTag;
        public string name;
        public string type;
        public int constructionCost;
        //goodConstructionCost
        //goodmaintenanceCost
        // string in job assignment
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
