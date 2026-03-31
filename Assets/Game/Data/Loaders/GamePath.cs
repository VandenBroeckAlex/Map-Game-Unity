using System.IO;
using UnityEngine;

public static class GamePaths
{
    // Centralize the root. 
    private static readonly string Root = Path.Combine(Application.dataPath, "Game", "StreamingAssets", "BaseGame");

    public static string Definitions => Path.Combine(Root, "Def");
    public static string Runtime => Root;

    public static string GetDefinition(string fileName)
        => Path.Combine(Definitions, $"{fileName}.json"); 

    public static string GetProvinceData(string fileName)
        => Path.Combine(Runtime, "Provinces", $"{fileName}.json");

    public static string GetTileData(string fileName)
        => Path.Combine(Runtime, "Tiles", $"{fileName}.json");
}
