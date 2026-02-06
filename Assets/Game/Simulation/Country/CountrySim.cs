using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;




public class CountriesManager
{

    public Dictionary<int, Country> countryList = new();

    public int[] GetCountryColorById(int _id)
    {
        Country country = countryList.Where(c => c.Key == _id).FirstOrDefault().Value;


        return country.color;
    }

    public string GetCountryNameById(int _id)
    {
        Country country = countryList.Where(c => c.Key == _id).FirstOrDefault().Value;
        return country.name;
    }

    public Country GetCountryById(int _id)
    {
        return countryList.Where(c => c.Key.Equals(_id)).FirstOrDefault().Value;
    }

    public int NumberOfCountry()
    {
        return countryList.Count;
    }

}
