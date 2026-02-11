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
                ""strata"": ""Lowest Strata""
              },
              {
                ""type"": ""Mecanics"",
                ""strata"": ""middle Strata""
              },{
                ""type"": ""Bourgeois"",
                ""strata"": ""Higher Strata""
              },
            ]";

        PopJobDeserializeResult result = loader.Deserialize_PopJob(json);
        Dictionary<int, RunTimePopJob> popJob = result.popJob;

        Assert.IsNotNull(popJob);
        Assert.AreEqual(3, popJob.Count);
        Assert.AreEqual("Serfs", popJob[0].type);
        Assert.AreEqual("Lowest Strata", result.strata[1]);
    }
}
