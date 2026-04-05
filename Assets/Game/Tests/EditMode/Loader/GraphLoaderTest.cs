using NUnit.Framework;
using UnityEngine;
using static ColorUtilities;

public class GraphLoaderTest
{
    public string  json = @"
                   [
  {
    ""id"": ""#5159FF"",
    ""neighbors"": {
      ""#65F18C"": 346,
      ""#F17A65"": 269,
      ""#9B8C51"": 208,
      ""#67504C"": 394
    },
    ""pivot"": [
      24,
      24
    ]
  },
  {
    ""id"": ""#65F18C"",
    ""neighbors"": {
      ""#5159FF"": 346
    },
    ""pivot"": [
      9,
      13
    ]
  },
  {
    ""id"": ""#F17A65"",
    ""neighbors"": {
      ""#5159FF"": 269
    },
    ""pivot"": [
      37,
      14
    ]
  },
  {
    ""id"": ""#9B8C51"",
    ""neighbors"": {
      ""#5159FF"": 208
    },
    ""pivot"": [
      32,
      36
    ]}]";
    

    [Test]
    public void GraphDataLoader_Deserialze_test()
    {
        GraphDataLoder graphDataLoder = new GraphDataLoder();
        DataRegistery _registery = new DataRegistery();
        IResolutionErrorHandler _errorHandle = new ThrowErrorHandler();
        string[] presentHex = new string[] { "#5159FF", "#65F18C", "#F17A65", "#9B8C51" };

        foreach(string tag in _registery.countriesTag)
        {
            Debug.Log(tag);
        }
   

        foreach(string hex in presentHex)
        {
            LandTileBuilder builder = new LandTileBuilder()
            .WithID(hex)
            .WithSpriteColor(hex);

            _registery.tiles[HexToInt(hex)] = builder.Build(_registery, _errorHandle);
        }
     
        GraphDataLoder gdl = new GraphDataLoder();

        _registery = gdl.Load(json, _registery, _errorHandle);

        Assert.AreEqual(4,_registery.mapGraphNodes.Count);


        //Dictionary<int, Tile> tileDictionary = new Dictionary<int, Tile>();


    }
}
