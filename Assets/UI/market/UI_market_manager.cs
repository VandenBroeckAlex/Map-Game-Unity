using UnityEngine;
using UnityEngine.UI;
using static TickScript;
using static MarketManager;
using TMPro;
public class UI_market_manager : MonoBehaviour
{

    public GameObject goodCard;
    public Transform goodCardParent;
    MarketManager _marketManager;
    PlayerManager _playerManager;
    CountriesManager _countriesManager;
    int player_country_id;

    TextMeshProUGUI marketNameText;


    void OnEnable()
    {
        RefreshUI();
       
        MarketManager.instance.OnMarketUpdated += RefreshUI;
    }

    void OnDisable()
    {
        MarketManager.instance.OnMarketUpdated -= RefreshUI;
    }


    public void Initilize()
    {
        _playerManager = PlayerManager.instance;
        _marketManager = MarketManager.instance;
        _countriesManager = CountriesManager.instance;
        Player player = _playerManager.GetHumanPlayer();
        player_country_id = player.countryID;
        Transform t = transform.Find("Title/Title_box/T_Market_name");
        if (t == null)
        {
            Debug.LogError("T_Market_name not found! Check hierarchy.");
            return;
        }

         marketNameText = t.GetComponent<TextMeshProUGUI>();
    }
    public void PopulateList()
    {     
        Market_object.Market _market = _marketManager.marketList[player_country_id];        

        marketNameText.text = $"{_countriesManager.GetCountryNameById(_market.countryId)} market";

        foreach (Market_object.MarketGood good in _market.goods_list)
        {
            GameObject newGo = Instantiate(goodCard, goodCardParent);
            TextMeshProUGUI[] texts = newGo.GetComponentsInChildren<TextMeshProUGUI>();
            //Maybe query field by name ?
            texts[0].text = $"{good.good.name}";
            texts[1].text = $"Stockpile : {(good.stockpile / 100f).ToString("F2")}";
            texts[2].text = $"Supply : {(good.supply/100f).ToString("F2")}";
            texts[3].text = $"Demand : {(good.demand/100f).ToString("F2")}";
            texts[4].text = $"Price : {(good.price/100f).ToString("F2")} £";
        }
    }
    
    private void RefreshUI()
    {
        Market_object.Market _market = _marketManager.marketList[1];

        foreach (Transform child in goodCardParent)
            Destroy(child.gameObject);

        PopulateList();
    }
    
}
