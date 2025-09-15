using MyGame.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CountriesManager : MonoBehaviour
{
    [SerializeField] public List<Country> CountryList = new();
    public static CountriesManager Instance;

    
    // Start is called before the first frame update
    void Start()
    {
        CountryList.Clear();    
        //Load country
        Country country = new Country(1,"France",Color.blue,100f);
        CountryList.Add(country);
    }

   

    public List<MyGame.Data.Country> countries;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    //public Country GetCountryByTag(string tag);

}
