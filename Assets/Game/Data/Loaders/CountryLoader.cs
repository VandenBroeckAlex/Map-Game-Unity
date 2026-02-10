
using A_VDB.Definition;
using Mono.Cecil;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using static Codice.Client.BaseCommands.Import.Commit;

public class CountryLoader
{

    public Dictionary<int, Country> countryList = new();

    public void LoadCountry()
    {
        string jsonPath = FilePath.CountryDef;
        InitializeDefaultCountry(jsonPath);
    }

    public void TestInitialize(string jsonPath)
    {
        InitializeDefaultCountry(jsonPath);
    }


    private void InitializeDefaultCountry(string jsonPath)
    {

        if (File.Exists(jsonPath))
        {
            string jsonText = File.ReadAllText(jsonPath);
            var countryDef = JsonConvert.DeserializeObject<List<DefCountry>>(jsonText);

            foreach (var _c in countryDef)
            {
                int[] color = { _c.color[0], _c.color[1], _c.color[2], 255 };
                Country country = new Country(
                    _c.id,
                    _c.name,
                    color,
                    _c.treasury,
                    _c.tag
                );

                countryList.Add(_c.id, country);
            }
        }
        else
        {
            //Debug.LogError($"json not found at " + jsonPath);
        }
    }
}
