using NUnit.Framework;
using System.Collections.Generic;
using static ColorUtilities;
public class TileLoaderTest
{

    [Test]
    public void TileLoder_Deserialize()
    {

        TileLoader loader = new TileLoader();
        DataRegistery _registery = new DataRegistery();
        IResolutionErrorHandler _errorHandler = new ThrowErrorHandler();
        string json = @"
            [{
            ""tag"": ""MER"",
            ""name"": ""la mer"",
            ""typeTag"": ""MER"",
            ""spriteColor"": ""#5159FF"",
            ""superficy"": 1805,
            ""isLand"": false,
            ""isPassable"": true,
            ""ownerTag"": """",
            ""occupierTag"": """",
            ""rgoTag"": ""cow"",
            ""isCoast"": false,
            ""climatTag"": ""temp"",
            ""provinceTag"": """"
          },
          {
            ""tag"": ""B"",
            ""name"": ""Liège"",
            ""typeTag"": ""pla"",
            ""spriteColor"": ""#65F18C"",
            ""superficy"": 169,
            ""isLand"": true,
            ""isPassable"": true,
            ""ownerTag"": ""BEL"",
            ""occupierTag"": """",
            ""rgoTag"": ""cow"",
            ""isCoast"": true,
            ""climatTag"": ""temp"",
            ""provinceTag"": ""IDF""
          }
            ]";
        //initialize tags

        _registery.climateTypesTags = new string[] { "temp", "cont" };
        _registery.provincesTag = new string[] { "IDF" };
        _registery.rgoTag = new Dictionary<string, int>();
        _registery.rgoTag["cow"] = 1;
        _registery.countriesTag = new string[] {"BEL"};
        _registery.terrainTypesTags = new string[] { "pla" };
        //test
        Dictionary<int, Tile> result = loader.DeserializeTiles(json, _registery, _errorHandler);

        Assert.IsNotNull(result);
        Assert.AreEqual(2, result.Count);

        // 1 = waterTile
        Tile waterTile = result[HexToInt("#5159FF")];
        Assert.IsInstanceOf<WaterTile>(waterTile);
        Assert.AreEqual(waterTile.isLand, false);
        // 2 = LandTile
        Tile landTile = result[HexToInt("#65F18C")];
        Assert.IsInstanceOf<LandTile>(landTile);
        Assert.AreEqual(landTile.name, "Liège");
        Assert.AreEqual(landTile.isLand, true);
    }

}
