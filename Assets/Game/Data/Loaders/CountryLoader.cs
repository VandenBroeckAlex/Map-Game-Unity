
using A_VDB.Definition;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;


public class CountryLoader
{
     //{
     //       ""id"": 2,
     //       ""name"": ""Germany"",
     //       ""color"": [107, 107, 107],
     //       ""treasury"": 1,
     //       ""tag"": ""GER"",
     //       ""flag"": ""path""
     //     }
     public class CountryTest
    {
        public int id;
        public string name;
        public int[] color;
        public int treasury;
        public string tag;
        public string flag;
    }


    public Dictionary<int, Country> countryList = new();

    public Dictionary<int, Country> DeserializeCountries(string json)
    {
        Dictionary<int, Country> _countryList = new();
        List<Country> countryDef = JsonConvert.DeserializeObject<List<Country>>(json);
        
        foreach (Country _c in countryDef)
        {
            int[] color = { _c.color[0], _c.color[1], _c.color[2], 255 };
            Country country = new Country(
                _c.id,
                _c.name,
                color,
                _c.treasury,
                _c.tag
            );

            _countryList.Add(_c.id, country);
        };
        return _countryList;
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
