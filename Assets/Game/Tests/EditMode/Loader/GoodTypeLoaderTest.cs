using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class GoodTypeLoaderTest
{
    [Test]
    public void GoodTypeLoaderDeserialize_goodsType_True()
    {
        GoodTypeLoader loader = new GoodTypeLoader();
        string json = @"
            [
            ""Raw"",
            ""Manifactured"",
            ""Luxury"",
            ""Military"",
            ]";

        string[] result = loader.Deserialize_goodsType(json);

        Assert.AreEqual(4, result.Length);
        Assert.AreEqual("Raw", result[0]);
        Assert.AreEqual("Manifactured", result[1]);
        Assert.AreEqual("Luxury", result[2]);
        Assert.AreEqual("Military", result[3]);

    }
}
