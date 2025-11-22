using System.IO;
using UnityEngine;

public class FilePath
{
    public static string MapInfo => Path.Combine(Application.persistentDataPath, "Map/Map_info.json");
    public static string MapEdge => Path.Combine(Application.persistentDataPath, "Map/MapEdge.json");
    public static string ColorId => Path.Combine(Application.persistentDataPath, "ColorId.json");
    public static string ProvincesSplit => Path.Combine(Application.persistentDataPath, "Provinces_split");
    public static string ProvinceMapImg => Path.Combine(Application.persistentDataPath, "Province_Map.png");
    public static string Goods => Path.Combine(Application.persistentDataPath, "Economy/GoodsDefinitions.json");
    public static string SpritesInfos => Path.Combine(Application.persistentDataPath, "Map/SpritesInfos.json");
    public static string TilesInfos => Path.Combine(Application.persistentDataPath, "Map/TilesInfos.json");

    public static string CountryDef = Path.Combine(Application.persistentDataPath, "Country/CountryDef.json");
}
