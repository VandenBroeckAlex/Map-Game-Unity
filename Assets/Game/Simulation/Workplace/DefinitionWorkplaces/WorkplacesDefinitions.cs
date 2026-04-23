using System.Collections.Generic;

public class WorkplacesDefinitions
{
    public class DefinitionWorkplace
    {
        int id;
        string name;
        string type;
        int constructionIC;
        public Dictionary<int, int> constructionCost; // good id num
        public Dictionary<int, int> maintenanceCost; // good id num
        public Dictionary<int, int> workers; //popjob id - num
        //conditions
        protected DefinitionWorkplace(string name, string type, int ic, Dictionary<int, int> workers)
        {
            this.name = name;
            this.type = type;
            this.constructionIC = ic;
            this.workers = workers;
        }
    
        public int GetId() { return id; }
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
            ) : base(name, type, icCost, workers)
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
            Dictionary<int, int> workers
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
        public Dictionary<int, int> input; //good id - num

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
            this.output = ouputId;
            this.input = input;
        }
        public int efficiency;
        public int output; //good id
        public Dictionary<int, int> input; //good id - num
        //formula xA + xB = zC
    }
}
