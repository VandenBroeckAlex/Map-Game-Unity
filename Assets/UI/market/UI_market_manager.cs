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
    int player_country_id;

  
    void OnEnable()
    {
        BuildUI();
        MarketManager.instance.OnMarketUpdated += RefreshUI;
    }

    void OnDisable()
    {
        MarketManager.instance.OnMarketUpdated -= RefreshUI;
    }


    public void Initilize()
    {
        _playerManager = PlayerManager.Instance;
        _marketManager = MarketManager.instance;
        Player player = _playerManager.GetHumanPlayer();
        player_country_id = player.countryID;
    }
    public void PopulateList()
    {     
        Debug.Log($"Their is {_marketManager.marketList.Count} market in ref");
        Market_object.Market _market = _marketManager.marketList[1];
        
        foreach(Market_object.MarketGood good in _market.goods_list)
        {
            GameObject newGo = Instantiate(goodCard, goodCardParent);
            TextMeshProUGUI[] texts = newGo.GetComponentsInChildren<TextMeshProUGUI>();
            //Maybe query field by name ?
            texts[0].text = $"{good.good.name}";
            texts[1].text = $"Stockpile : {good.stockpile.ToString("F2")}";
            texts[2].text = $"Supply : {good.supply.ToString("F2")}";
            texts[3].text = $"Demand : {good.demand.ToString("F2")}";
            texts[4].text = $"Price : {good.price.ToString("F2")} £";
        }
    }
    
    private void RefreshUI()
    {
        Market_object.Market _market = _marketManager.marketList[1];

        foreach (Transform child in goodCardParent)
            Destroy(child.gameObject);

        PopulateList();
    }
    private void BuildUI()
    {
        RefreshUI();
    }
}
