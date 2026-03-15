using NUnit.Framework;

using System;
using System.Collections.Generic;
using System.IO;

public class ProvinceLoaderTest
{
    [Test]
    public void ProvinceLoader_Deserialize_ResultInfo_true()
    {
        ProvincesLoader loader = new ProvincesLoader();
        string[] countryTagId = new string[1];
        countryTagId[0] = "Bel";
        // data + tagId
        string json = @"
                   [{
            ""tag"": ""Lie"",
            ""ownerTag"": ""Bel"",
            ""name"": ""Liège"",
            ""isOccupied"": false,
            ""occupierTag"": ""."",

          }]";

        ProvincesLoader.ProvinceData _result = loader.LoadProvince(json, countryTagId);
        List<Province> result = _result.provincesList;
        Assert.IsNotNull(result);
        Assert.AreEqual(1, result.Count);
        Assert.AreEqual(0, result[0].ownerId);
        Assert.AreEqual("Liège", result[0].name);
        Assert.AreEqual(false, result[0].isOccupied);
    }


    [Test]
    public void ProvinceLoader_Deserialize_ThrowError()
    {
        ProvincesLoader loader = new ProvincesLoader();
        string[] countryTagId = new string[2];
        countryTagId[0] = "Bel";
        // data + tagId
        string json = @"
                   [{
            ""tag"": ""Lie"",
            ""ownerTag"": ""Bel"",
            ""name"": ""Liège"",
            ""isOccupied"": false,
            ""occupierTag"": ""."",

          },
          {
            ""tag"": ""Nam"",
            ""ownerTag"": ""Bul"",
            ""name"": ""Namur"",
            ""isOccupied"": false,
            ""occupierTag"": ""."",

          }]";

         Assert.Throws<InvalidDataException>(LoaderThrows);
        
        void LoaderThrows()
        {
            loader.LoadProvince(json, countryTagId);
        }
    }
    [Test]
    public void ProvinceLoader_Deserialize_ProvincesCount_true()
    {
        ProvincesLoader loader = new ProvincesLoader();
        string[] countryTagId = new string[2];
        countryTagId[0] = "Bel";
        // data + tagId
        string json = @"
                   [{
            ""tag"": ""Lie"",
            ""ownerTag"": ""Bel"",
            ""name"": ""Liège"",
            ""isOccupied"": false,
            ""occupierTag"": ""."",

          },
          {
            ""tag"": ""Nam"",
            ""ownerTag"": ""Bel"",
            ""name"": ""Namur"",
            ""isOccupied"": false,
            ""occupierTag"": ""."",

          }]";

        ProvincesLoader.ProvinceData _result = loader.LoadProvince(json, countryTagId);
        List<Province> result = _result.provincesList;
        Assert.IsNotNull(result);
        Assert.AreEqual(2, result.Count);
        Assert.AreEqual(0, result[0].ownerId);
        Assert.AreEqual("Liège", result[0].name);
        Assert.AreEqual(false, result[0].isOccupied);
        Assert.AreEqual("Namur", result[1].name);
    }
}
