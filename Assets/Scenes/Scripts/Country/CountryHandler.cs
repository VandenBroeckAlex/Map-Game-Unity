using MyGame.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CountryHandler : MonoBehaviour
{
    [SerializeField] public List<Country> CountryList = new();
    public static CountryHandler Instance;

    [System.Serializable]
    public class Country
    {
        public string countryName;
        public Color color; 
        public List<Province> ownedProvinces;
        public List<Pop> population; 
        public float treasury;
        public float income;
        public float expenses;
        //public GovernmentType governmentType;
        //public Dictionary<GoodsType, float> nationalStockpile;

        //public Dictionary<Country, DiplomaticRelation> diplomaticRelations;

        // Methods
        //public void CalculateIncome();
        //public void HandleTrade();
 
    }

    // Start is called before the first frame update
    void Start()
    {
        CountryList.Clear();    
        CountryList.Add(new Country());
    }

   

    public List<Country> countries;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    //public Country GetCountryByTag(string tag);

}
