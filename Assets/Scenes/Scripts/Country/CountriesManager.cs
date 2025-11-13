using MyGame.Data;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CountriesManager : MonoBehaviour
{
    [SerializeField] 
    public List<Country> countryList = new();
    public static CountriesManager instance;


    // Start is called before the first frame update
    private void Awake()
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

    void Start()
    {
        //countryList.Clear();    
        //Load country
      
        countryList.Add(new Country(1, "France", Color.blue, 100f));
        countryList.Add(new Country(2, "Germany", Color.gray, 100f));
        countryList.Add(new Country(3, "Italy", Color.green, 100f));
        countryList.Add(new Country(4, "Belgium", Color.yellow, 100f));
    }

   
    public Color GetCountryColorById(int _id)
    {
        Country country = countryList.Where(c => c.id == _id ).FirstOrDefault();


        return country.color;
    }


    //public Country GetCountryByTag(string tag);

}
