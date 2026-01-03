using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;


public class TestCountry
{
    string[] countryNameList = { "Belgium", "France", "Germany", "Italy" };
    // FunctionName_Scenario_ExpectedBehavior
    [Test]
    public void CountryManager_OnInitialize_ReturnAmmountCountry()
    {
        // 1. Arrange //Set Up And Initialize
        GameContext context = new GameContext();
        CountriesManager _countryManager = new CountriesManager(context);

        string jsonPath = FilePath.TestCountryDef;

        _countryManager.TestInitialize(jsonPath);
        // 2. Act // Call Methode
        int countryNumber = _countryManager.NumberOfCountry();

        // 3. Assert
        Assert.AreEqual(4, countryNumber, "Country Manager does not have the same ammount of country than the json");
    }


    [Test]
    public void CountryManager_GetCountryNameById_ReturnIdName()
    {

        GameContext context = new GameContext();
        CountriesManager _countryManager = new CountriesManager(context);

        string jsonPath = FilePath.TestCountryDef;

        _countryManager.TestInitialize(jsonPath);

      
        for (int i = 0; i < countryNameList.Length; i++) 
        {
            string countryName = _countryManager.GetCountryNameById(i);
            Assert.AreEqual(countryNameList[i], countryName, $"Country of Id : 0 should have been: Belgium but it returned :{countryNameList[i]}");
        }
       
    }

    [Test]
    public void CountryManager_GetCountryById_ReturnCountry()
    {
        GameContext context = new GameContext();
        CountriesManager _countryManager = new CountriesManager(context);

        string jsonPath = FilePath.TestCountryDef;
        _countryManager.TestInitialize(jsonPath);

        Country country = _countryManager.GetCountryById(1);

        Assert.AreEqual(country.id, 1, $"The returned country have the wrong id. Expected 1, Received {country.id}");
        Assert.AreEqual(country.name, "France", $"The returned country have the wrong name. Expected France, Received {country.name}");
        Color countryColor = new Color32((byte)0, (byte)30, (byte)179, 255);
        Assert.AreEqual(country.color, countryColor, $"The returned country have the wrong color. Expected {countryColor}, Received {country.color}");
        Assert.AreEqual(country.tag, "FRA", $"The returned country have the wrong Tag. Expected \"FRA\", Received {country.tag}");
    }

    [Test]
    public void CountryManager_GetCountryById_ReturnError()
    {
        GameContext context = new GameContext();
        CountriesManager _countryManager = new CountriesManager(context);

        string jsonPath = FilePath.TestCountryDef;
        _countryManager.TestInitialize(jsonPath);

        Country country = _countryManager.GetCountryById(10);

        Assert.AreEqual(country, null, $"Expected null, Received {country}");
    }
}
