using System.IO;
using UnityEngine;

public class FilePath
{
    public static string MapInfo => Path.Combine(Application.persistentDataPath, "map/map_info.json");
    public static string MapEdge => Path.Combine(Application.persistentDataPath, "map/mapEdge.json");
    public static string ColorId => Path.Combine(Application.persistentDataPath, "ColorId.json");
    public static string ProvincesSplit => Path.Combine(Application.persistentDataPath, "Provinces_split");
    public static string ProvinceMapImg => Path.Combine(Application.persistentDataPath, "Province_Map.png");
    public static string Goods => Path.Combine(Application.persistentDataPath, "Economy/GoodsDefinitions.json");
    public static string SpritesInfos => Path.Combine(Application.persistentDataPath, "map/spritesInfos.json");
    public static string TilesInfos => Path.Combine(Application.persistentDataPath, "map/tilesInfos.json");
}
