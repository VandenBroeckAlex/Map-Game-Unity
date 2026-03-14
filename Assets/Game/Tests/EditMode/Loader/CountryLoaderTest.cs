using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static CountryLoader;


public class CountryLoaderTest
{
    [Test]
    public void CountryLoader_Deserialize_Countries()
    {
        string json = @"
                   [{
            ""id"": 0,
            ""name"": ""Belgium"",
            ""color"": [ 255, 191, 0 ],
            ""treasury"": 1,
            ""tag"": ""BEL"",
            ""flag"" :  ""path""
          },
          {
            ""id"": 1,
            ""name"": ""France"",
            ""color"": [ 0, 30, 179 ],
            ""treasury"": 1,
            ""tag"": ""FRA"",
            ""flag"": ""path""
          },
          {
            ""id"": 2,
            ""name"": ""Germany"",
            ""color"": [ 107, 107, 107 ],
            ""treasury"": 1,
            ""tag"": ""GER"",
            ""flag"": ""path""
          }]";

       
     
        CountryLoader countryLoader = new CountryLoader();
        CountryLoaderData _result = countryLoader.DeserializeCountries(json);
        Dictionary<int, Country> result = _result.countryDictionnary;
        Debug.Log(result.Count);

        Assert.IsNotNull(result);
        Assert.AreEqual(3, result.Count);
        Assert.AreEqual("Belgium", result[0].name);
        Assert.AreEqual(1, result[0].treasury);
        Assert.AreEqual("BEL", result[0].tag);
    }
}
