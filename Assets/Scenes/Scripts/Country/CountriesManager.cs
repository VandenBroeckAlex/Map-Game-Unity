using MyGame.Data;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

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
        countryList.Add(0,new Country(0, "Belgium", Color.yellow, 100f));
        countryList.Add(1, new Country(1, "France", Color.blue, 100f));
        countryList.Add(2, new Country(2, "Germany", Color.grey, 100f));
        countryList.Add(3, new Country(3, "Italy", Color.green, 100f));
       
    } 
   
    public Color GetCountryColorById(int _id)
    {
        Country country = countryList.Where(c => c.Key == _id ).FirstOrDefault().Value;


        return country.color;
    }


    //public Country GetCountryByTag(string tag);

}
