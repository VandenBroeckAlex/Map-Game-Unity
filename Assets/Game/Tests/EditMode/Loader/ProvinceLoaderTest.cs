using NUnit.Framework;

using System;
using System.Collections.Generic;
using System.IO;

public class ProvinceLoaderTest
{
    [Test]
    public void ProvinceLoader_Deserialize_GetException()
    {
        ProvincesLoader loader = new ProvincesLoader();
        Dictionary<string,int> countryTagId = new Dictionary<string,int>();
        countryTagId["Bel"] = 0;
        // data + tagId
        string json = @"
                   [{
            ""tag"": ""Lie"",
            ""ownerTag"": ""Bel"",
            ""name"": ""Liège"",
            ""isOccupied"": false,
            ""occupierTag"": ""."",

          }]";

        List<Province> result = loader.LoadProvince(json, countryTagId);
        Assert.IsNotNull(result);
        Assert.AreEqual(1, result.Count);
        Assert.AreEqual(0, result[0].ownerId);
        Assert.AreEqual("Liège", result[0].name);
        Assert.AreEqual(false, result[0].isOccupied);
    }


    [Test]
    public void ProvinceLoader_Deserialize_Error()
    {
        ProvincesLoader loader = new ProvincesLoader();
        Dictionary<string, int> countryTagId = new Dictionary<string, int>();
        countryTagId["Bel"] = 0;
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

         Assert.Throws<InvalidDataException>(MethodThatThrows);
        
        void MethodThatThrows()
        {
            loader.LoadProvince(json, countryTagId);
        }
    }
}
