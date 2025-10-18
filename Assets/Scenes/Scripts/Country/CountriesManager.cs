using MyGame.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CountriesManager : MonoBehaviour
{
    [SerializeField] 
    public List<Country> countryList = new();
    public static CountriesManager instance;


    // Start is called before the first frame update
    void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        countryList.Clear();    
        //Load country
      
        countryList.Add(new Country(1, "France", Color.blue, 100f));
        countryList.Add(new Country(2, "Germany", Color.gray, 100f));
        countryList.Add(new Country(3, "Italy", Color.green, 100f));
        countryList.Add(new Country(4, "Belgium", Color.yellow, 100f));
    }

   



    //public Country GetCountryByTag(string tag);

}
