using NUnit.Framework;


public class GoodTypeLoaderTest
{
    IResolutionErrorHandler errorHandler = new ThrowErrorHandler();
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

        string[] result = loader.Deserialize_goodsType(json, errorHandler);

        Assert.AreEqual(4, result.Length);
        Assert.AreEqual("Raw", result[0]);
        Assert.AreEqual("Manifactured", result[1]);
        Assert.AreEqual("Luxury", result[2]);
        Assert.AreEqual("Military", result[3]);

    }
}
