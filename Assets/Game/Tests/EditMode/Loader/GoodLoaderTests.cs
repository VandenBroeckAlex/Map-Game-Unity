using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;

public class GoodLoaderTest
{
    [Test]
    public static void GoodLoader_Load_goods_True()
    {
        GoodLoader loader = new GoodLoader();
        string json = @"
            [
              {
                ""name"": ""Wood"",
                ""basePrice"": 100,
                ""baseProductionModdifier"": 1,
                ""type"": ""Raw"",
                ""iconPath"": ""icons/wood"",
                ""color"": ""#b65344""
              }
            ]";
        Dictionary<int,string> type = new Dictionary<int,string>();

        type[0] = "Raw";

        Good[] goodList = loader.Load_goods(json, type);

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
        GoodLoader loader = new GoodLoader();
        string json = @"
            [
              {
                ""name"": ""Wood"",
                ""basePrice"": 100,
                ""baseProductionModdifier"": 1,
                ""type"": ""Raw"",
                ""iconPath"": ""icons/wood"",
                ""color"": ""#b65344""
              },
              {
                ""name"": ""Grain"",
                ""basePrice"": 100,
                ""baseProductionModdifier"": 1,
                ""type"": ""Raw"",
                ""iconPath"": ""icons/wood"",
                ""color"": ""#b65344""
              },
               {
                ""name"": ""Horses"",
                ""basePrice"": 100,
                ""baseProductionModdifier"": 1,
                ""type"": ""Raw"",
                ""iconPath"": ""icons/wood"",
                ""color"": ""#b65344""
              },
            ]";
        Dictionary<int, string> type = new Dictionary<int, string>();

        type[0] = "Raw";

        Good[] goodList = loader.Load_goods(json, type);

        Assert.IsNotNull(goodList);

        if (goodList.Length != 3)
        {
            Assert.Fail($"Expected GoodList length = 3, received: {goodList.Length}");
        }
    }
}
