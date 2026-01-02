using System.IO;
using UnityEngine;

public class FilePath
{
    public static string MapInfo => Path.Combine(Application.persistentDataPath, "Tiles/Map_info.json");
    public static string MapEdge => Path.Combine(Application.persistentDataPath, "Tiles/MapEdge.json");
    public static string ColorId => Path.Combine(Application.persistentDataPath, "ColorId.json");
    public static string ProvincesSplit => Path.Combine(Application.persistentDataPath, "Provinces_split");
    public static string ProvinceMapImg => Path.Combine(Application.persistentDataPath, "Province_Map.png");
    public static string Goods => Path.Combine(Application.persistentDataPath, "Economy/GoodsDefinitions.json");
    public static string SpritesInfos => Path.Combine(Application.persistentDataPath, "Tiles/SpritesInfos.json");
    public static string TilesInfos => Path.Combine(Application.persistentDataPath, "Tiles/TilesInfos.json");

    public static string CountryDef = Path.Combine(Application.persistentDataPath, "Country/CountryDef.json");


    //-------- Path for test file ______
    public static string TestCountryDef = Path.Combine(Application.persistentDataPath, "Tests/CountryDef.json");

}
