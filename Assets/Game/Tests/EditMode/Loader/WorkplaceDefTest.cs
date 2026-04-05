using NUnit.Framework;
using System.Collections.Generic;
using static WorkplaceLoader;

public class WorkplaceDefTest
{
    [Test]
    public void WorkplaceDefinitionLoader_workplaceDef_True()
    {
        DataRegistery registery = new DataRegistery();

        Good[] goodArray = new Good[1];
        Good good = new Good();
        good.tag = "grain";
        goodArray[0] = good;

        PopJob[] popJobs = new PopJob[2];
        PopJob popJob = new PopJob("miners", 0, "miners");
       
        popJobs[0] = popJob;

        registery.goodList = goodArray;
        registery.popJobs = popJobs;

        registery.climateTypesTags = new string[] { "temperate", "continental" };

        WorkplaceLoader workplaceLoader = new WorkplaceLoader(registery);

       string json = @"
[
    {
        ""name"": ""coal mine"",
        ""type"": ""mining"",
        ""constructionIC"": 400,
        ""maintenanceCost"": 10,
        ""workersType"": {
            ""miners"": 900,
            ""machinists"": 50,
            ""clerks"": 20,
            ""engineer"": 20
        },
        ""efficiency"": 100,
        ""output"": ""coal""
    },
    {
        ""name"": ""grain fields"",
        ""type"": ""crops"",
        ""constructionIC"": 100,
        ""maintenanceCost"": 5,
        ""workersType"": {
            ""peasants"": 1000
        },
        ""efficiency"": 100,
        ""output"": ""grain"",
        ""climate"": [""temperate"", ""continental""]
    }
]";
    
        List<WorkplacesDefinitions.DefinitionWorkplace> result = workplaceLoader.DeserializeWorkplaces(json);

        Assert.IsNotNull(result);
        Assert.AreEqual(2, result.Count);

    }

    public void WorkplaceDefinitionLoader_workplaceDef_false()
    {
        DataRegistery registery = new DataRegistery();

        Good[] goodArray = new Good[1];
        Good good = new Good();
        good.tag = "grain";
        goodArray[0] = good;

        PopJob[] popJobs = new PopJob[2];
        PopJob popJob = new PopJob("miners", 0, "miners");
        popJob.tag = "farmer";
        popJobs[0] = popJob;

        registery.goodList = goodArray;
        registery.popJobs = popJobs;

        registery.climateTypesTags = new string[] { "temperate", "continental" };

        WorkplaceLoader workplaceLoader = new WorkplaceLoader(registery);

        string json = @"
[
    {
        ""name"": ""coal mine"",
        ""type"": ""mining"",
        ""constructionIC"": 400,
        ""maintenanceCost"": 10,
        ""workersType"": {
            ""miners"": 900,
            ""machinists"": 50,
            ""clerks"": 20,
            ""engineer"": 20
        },
        ""efficiency"": 100,
        ""output"": ""coal""
    },
    {
        ""name"": ""grain fields"",
        ""type"": ""crops"",
        ""constructionIC"": 100,
        ""maintenanceCost"": 5,
        ""workersType"": {
            ""peasants"": 1000
        },
        ""efficiency"": 100,
        ""output"": ""grain"",
        ""valid_climate"": [""temperate"", ""continental""]
    }
]";

        List<WorkplacesDefinitions.DefinitionWorkplace> result = workplaceLoader.DeserializeWorkplaces(json);

        Assert.IsNotNull(result);
        Assert.AreEqual(2, result.Count);

    }
}
