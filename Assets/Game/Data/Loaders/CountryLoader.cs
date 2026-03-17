
using A_VDB.Definition;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;


public class CountryLoader
{
    
   

    public class CountryLoaderData
    {
        public Country[] countries;
        public string[] countriesTag;
    }


    public CountryLoaderData DeserializeCountries(string json)
    {
       
        

        List<Country> countryDef = JsonConvert.DeserializeObject<List<Country>>(json);

        string[] tags = new string[countryDef.Count];
        Country[] countryList = new Country[countryDef.Count];

        int indexer = 0;
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

            countryList[indexer] = country;
            tags[indexer] = _c.tag;
            indexer++;
        }
        ;

        CountryLoaderData cld = new CountryLoaderData();
        cld.countries = countryList;
        cld.countriesTag = tags;

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
    }
}
