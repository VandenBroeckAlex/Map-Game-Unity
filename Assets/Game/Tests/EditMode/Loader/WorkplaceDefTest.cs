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

        PopJob[] popJobs = new PopJob[5];
        PopJob popJob = new PopJob("miners", 0, "miners");
        PopJob popJob1 = new PopJob("machinists", 0, "machinists");
        PopJob popJob2 = new PopJob("clerks", 0, "clerks");
        PopJob popJob3 = new PopJob("engineer", 0, "engineer");
        PopJob popJob4 = new PopJob("peasants", 0, "peasants");

        popJobs[0] = popJob;
        popJobs[1] = popJob1;
        popJobs[2] = popJob2;
        popJobs[3] = popJob3;
        popJobs[4] = popJob4;

        registery.goodList = goodArray;
        registery.popJobs = popJobs;

        registery.climateTypesTags = new string[] { "temperate", "continental" };

        WorkplaceLoader workplaceLoader = new WorkplaceLoader(registery);

        string json = @"
[
    {
        ""tag"": ""grain_farm_1"",
        ""name"": ""Grain Fields"",
        ""constructionCost"": 100,
        ""upgradeTemplateId"": ""grain_farm_2"",
        ""downgradeTemplateId"": null,
        ""goodConstructionCost"": {
            ""grain"": 10
        },
        ""goodmaintenanceCost"": {
            ""grain"": 2
        },
        ""input"": {},
        ""output"": [
            {
                ""type"": ""Market"",
                ""id"": ""grain"",
                ""baseAmount"": 50.0
            }
        ],
        ""workersType"": {
            ""peasants"": 1000
        }
    },
    {
        ""tag"": ""coal_mine_1"",
        ""name"": ""Basic Coal Mine"",
        ""constructionCost"": 400,
        ""upgradeTemplateId"": ""coal_mine_2"",
        ""downgradeTemplateId"": null,
        ""goodConstructionCost"": {
            ""grain"": 25
        },
        ""goodmaintenanceCost"": {
            ""grain"": 5
        },
        ""input"": {},
        ""output"": [
            {
                ""type"": ""Market"",
                ""id"": ""coal"",
                ""baseAmount"": 100.0
            }
        ],
        ""workersType"": {
            ""miners"": 900,
            ""machinists"": 50,
            ""clerks"": 20,
            ""engineer"": 20
        }
    },
    {
        ""tag"": ""coal_mine_2"",
        ""name"": ""Advanced Coal Mine"",
        ""constructionCost"": 800,
        ""upgradeTemplateId"": null,
        ""downgradeTemplateId"": ""coal_mine_1"",
        ""goodConstructionCost"": {
            ""grain"": 50
        },
        ""goodmaintenanceCost"": {
            ""grain"": 12
        },
        ""input"": {
            ""grain"": 5 
        },
        ""output"": [
            {
                ""type"": ""Market"",
                ""id"": ""coal"",
                ""baseAmount"": 250.0
            }
        ],
        ""workersType"": {
            ""miners"": 800,
            ""machinists"": 150,
            ""clerks"": 30,
            ""engineer"": 50
        }
    }
]";

        Dictionary<int, WorkplaceTemplate> result = workplaceLoader.DeserializeWorkplaceTemplate(json);

        Assert.IsNotNull(result);
        Assert.AreEqual(3, result.Count);

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
        ""tag"": ""grain_farm_1"",
        ""name"": ""Grain Fields"",
        ""constructionCost"": 100,
        ""upgradeTemplateId"": ""grain_farm_2"",
        ""downgradeTemplateId"": null,
        ""goodConstructionCost"": {
            ""grain"": 10
        },
        ""goodmaintenanceCost"": {
            ""grain"": 2
        },
        ""input"": {},
        ""output"": [
            {
                ""type"": ""Market"",
                ""id"": ""grain"",
                ""baseAmount"": 50.0
            }
        ],
        ""workersType"": {
            ""peasants"": 1000
        }
    },
    {
        ""tag"": ""coal_mine_1"",
        ""name"": ""Basic Coal Mine"",
        ""constructionCost"": 400,
        ""upgradeTemplateId"": ""coal_mine_2"",
        ""downgradeTemplateId"": null,
        ""goodConstructionCost"": {
            ""grain"": 25
        },
        ""goodmaintenanceCost"": {
            ""grain"": 5
        },
        ""input"": {},
        ""output"": [
            {
                ""type"": ""Market"",
                ""id"": ""coal"",
                ""baseAmount"": 100.0
            }
        ],
        ""workersType"": {
            ""miners"": 900,
            ""machinists"": 50,
            ""clerks"": 20,
            ""engineer"": 20
        }
    },
    {
        ""tag"": ""coal_mine_2"",
        ""name"": ""Advanced Coal Mine"",
        ""constructionCost"": 800,
        ""upgradeTemplateId"": null,
        ""downgradeTemplateId"": ""coal_mine_1"",
        ""goodConstructionCost"": {
            ""grain"": 50
        },
        ""goodmaintenanceCost"": {
            ""grain"": 12
        },
        ""input"": {
            ""grain"": 5 
        },
        ""output"": [
            {
                ""type"": ""Market"",
                ""id"": ""coal"",
                ""baseAmount"": 250.0
            }
        ],
        ""workersType"": {
            ""miners"": 800,
            ""machinists"": 150,
            ""clerks"": 30,
            ""engineer"": 50
        }
    }
]";

        Dictionary<int, WorkplaceTemplate> result = workplaceLoader.DeserializeWorkplaceTemplate(json);

        Assert.IsNotNull(result);
        Assert.AreEqual(2, result.Count);

    }
}
