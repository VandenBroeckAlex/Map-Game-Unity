using NUnit.Framework;
using System.Collections.Generic;

public class PopJobLoaderTest
{
    [Test]
    public void Deserialize_PopJob()
    {
        PopJobLoader loader = new PopJobLoader();
        string json = @"
            [{
                ""type"": ""Serfs"",
                ""strata"": ""Lowest""
              },
              {
                ""type"": ""Mecanics"",
                ""strata"": ""Middle""
              },{
                ""type"": ""Bourgeois"",
                ""strata"": ""Higher""
              },
            ]";

        string[] strata = { "Lowest","Middle","Higher"};

        RunTimePopJob[] popJob = loader.Deserialize_PopJob(json, strata);
        
        int lowestplaceInStrataArray = 0;

        Assert.IsNotNull(popJob);
        Assert.AreEqual(3, popJob.Length);
        Assert.AreEqual("Serfs", popJob[0].type);
        Assert.AreEqual(lowestplaceInStrataArray, popJob[0].strata);
    }
}
