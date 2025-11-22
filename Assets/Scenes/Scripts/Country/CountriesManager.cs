using MyGame.Data;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static ObjJSON;
using System.IO;
using Newtonsoft.Json;

public class CountriesManager : MonoBehaviour
{
    [SerializeField] 
    public Dictionary<int,Country> countryList = new();
    public static CountriesManager instance;


    // Start is called before the first frame update
    public void Initialize()
    {
        CreateSingleton();
        InitializeDefaultCountry();
        //LoadCountry every country formable should be loaded
    }

  
    private void CreateSingleton()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            Debug.Log("An instence of Country manager already exist");
        }
    }

    

    private void InitializeDefaultCountry()
    {
        string jsonPath = FilePath.CountryDef;
        if (File.Exists(jsonPath))
        {
            string jsonText = File.ReadAllText(jsonPath);
            var countryDef = JsonConvert.DeserializeObject<List<CountryDef>>(jsonText);

            foreach (var _c in countryDef)
            {
                Country country = new Country(
                    _c.id,
                    _c.name,
                    new Color32((byte)_c.color[0], (byte)_c.color[1], (byte)_c.color[2], 255),
                    _c.treasury
                );

                countryList.Add(_c.id, country);
            }
        }
        else
        {
            Debug.LogError($"json not found at " + jsonPath);
        }


        foreach (var country in countryList) 
        { 
            Debug.Log(country.Value.name);
        }
    } 
   
    public Color GetCountryColorById(int _id)
    {
        Country country = countryList.Where(c => c.Key == _id ).FirstOrDefault().Value;


        return country.color;
    }

    public class CountryDef
    {
        public int id { get; set; }
        public string name { get; set; }
        public int[] color { get; set; }
        public int treasury { get; set; }
        public string tag { get; set; }
        public string flag { get; set; }
    }

    //public Country GetCountryByTag(string tag);

}
