
using A_VDB.Definition;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;


public class CountryLoader
{
    
   

    public class CountryLoaderData
    {
        public Dictionary<int, Country> countryDictionnary;
        public Dictionary<string, int> countryReversedDictionnary;
    }


    public CountryLoaderData DeserializeCountries(string json)
    {
        Dictionary<int, Country> _countryList = new();
        Dictionary<string, int> reversed = new();

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
            reversed.Add(_c.tag, _c.id);
        }
        ;

        CountryLoaderData cld = new CountryLoaderData();
        cld.countryDictionnary = _countryList;
        cld.countryReversedDictionnary = reversed;

        return cld;
    }
     
    public void TestInitialize(string jsonPath)
    {
        InitializeDefaultCountry(jsonPath);
    }


    private void InitializeDefaultCountry(string jsonPath)
    {
        Dictionary<int,Country> countryList = new Dictionary<int, Country>();
        if (File.Exists(jsonPath))
        {
            string jsonText = File.ReadAllText(jsonPath);
            var countryDef = JsonConvert.DeserializeObject<List<DTOCountry>>(jsonText);
            int indexer = 0;
            foreach (var _c in countryDef)
            {
                int[] color = { _c.color[0], _c.color[1], _c.color[2], 255 };
                Country country = new Country(
                    indexer,
                    _c.name,
                    color,
                    _c.treasury,
                    _c.tag
                );
                countryList.Add(indexer, country);
                indexer++;
            }
        }
        else
        {
            //Debug.LogError($"json not found at " + jsonPath);
        }
    }
}
