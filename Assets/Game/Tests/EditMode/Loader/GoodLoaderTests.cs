using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;
using static GoodLoader;

public class GoodLoaderTest
{
    [Test]
    public static void GoodLoader_Load_goods_True()
    {
        DataRegistery registery = new DataRegistery();
        string[] type = new string[1];
        type[0] = "Raw";

        registery.goodTypes = type;

        GoodLoader loader = new GoodLoader(registery);
        string json = @"
            [
              {
                ""name"": ""Wood"",
                ""tag"": ""Wood"",
                ""basePrice"": 100,
                ""baseProductionModdifier"": 1,
                ""type"": ""Raw"",
                ""iconPath"": ""icons/wood"",
                ""color"": ""#b65344"",
                ""isRGO"":""true""
              }
            ]";

  

        GoodLoadedData data = loader.Load_goods(json);
        Good[] goodList = data.goodList;

        Assert.IsNotNull(goodList);
  
        Assert.AreEqual("Wood", goodList[0].name);
        Assert.AreEqual(100, goodList[0].basePrice);
   
        if (goodList[0].baseProductionModdifier != 1)
        {
            Assert.Fail($"Expected baseProductionModdifier = 1, received: {goodList[0].type}");
        }

        if (goodList[0].type != 0)
        {
            Assert.Fail($"Expected type = 1, received: {goodList[0].type}");
        }

        Assert.AreEqual("icons/wood", goodList[0].iconPath);
        Assert.AreEqual("#b65344", goodList[0].color);
    }

    [Test]
    public static void GoodLoader_Load_goods_NumberGoodRecived_True()
    {
        string[] type = new string[3];

        type[0] = "Manifactured";
        type[1] = "Luxury";
        type[2] = "Raw";

        DataRegistery registery = new DataRegistery();

        registery.goodTypes = type;

        GoodLoader loader = new GoodLoader(registery);
        string json = @"
            [
              {
                ""name"": ""Wood"",
                ""tag"": ""Wood"",
                ""basePrice"": 100,
                ""baseProductionModdifier"": 1,
                ""type"": ""Raw"",
                ""iconPath"": ""icons/wood"",
                ""color"": ""#b65344"",
                ""isRGO"":""true"",
              },
              {
                ""name"": ""Grain"",
                ""tag"": ""Grain"",
                ""basePrice"": 100,
                ""baseProductionModdifier"": 1,
                ""type"": ""Raw"",
                ""iconPath"": ""icons/wood"",
                ""color"": ""#b65344"",
                ""isRGO"":""true"",
              },
               {
                ""name"": ""Horses"",
                ""tag"": ""Horses"",
                ""basePrice"": 100,
                ""baseProductionModdifier"": 1,
                ""type"": ""Raw"",
                ""iconPath"": ""icons/wood"",
                ""color"": ""#b65344"",
                ""isRGO"":""true"",
              },
            ]";

    

        GoodLoadedData data = loader.Load_goods(json);

        Good[] goodList = data.goodList;

        Assert.IsNotNull(goodList);

        if (goodList.Length != 3)
        {
            Assert.Fail($"Expected GoodList length = 3, received: {goodList.Length}");
        }
        if (goodList[0].type != 2) 
        {
            Assert.Fail($"Expected GoodType to be : 2, recived: {goodList[0].type}");
        }

        Assert.AreEqual(data.rgoTag.Count, 3);
        Assert.IsTrue(data.rgoTag.TryGetValue("Wood", out int val));
    }
}
