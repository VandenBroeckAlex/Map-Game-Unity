using MyGame.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CountriesManager : MonoBehaviour
{
    [SerializeField] public List<Country> countryList = new();
    public static CountriesManager instance;

    
    // Start is called before the first frame update
    void Start()
    {
        countryList.Clear();    
        //Load country
        Country country = new Country(1,"France",Color.blue,100f);
        countryList.Add(country);
    }

   

    public List<MyGame.Data.Country> countries;

    void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    //public Country GetCountryByTag(string tag);

}
