using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_market_manager : MonoBehaviour
{
    public GameObject goodCard;
    public Transform goodCardParent;

    private MarketManager _marketManager;
    private PlayerManager _playerManager;
    private CountriesManager _countriesManager;

    private int countryId;
    private TextMeshProUGUI marketNameText;
    private Button closeButton;

    

    void OnEnable()
    {
        //Make delegate call instead
        //MarketManager.instance.OnMarketUpdated += RefreshUI;
    }

    void OnDisable()
    {
        //Make delegate call instead
        //MarketManager.instance.OnMarketUpdated -= RefreshUI;
    }

    public void CacheReferences(PlayerManager p, MarketManager m, CountriesManager c  )
    {
        _playerManager = p;
        _marketManager = m;
        _countriesManager = c;

        marketNameText = transform.Find("Title/Title_box/T_Market_name")
            .GetComponent<TextMeshProUGUI>();

        closeButton = transform.Find("close_button").GetComponent<Button>();
        closeButton.onClick.AddListener(CloseWindow);
    }

    // ==============================
    //  PUBLIC API FOR UI MANAGER
    // ==============================
    public void Initialize(int id)
    {
        countryId = id;
        RefreshUI();
    }

    public void Open()
    {
        gameObject.SetActive(true);
    }

    public void CloseWindow()
    {
        gameObject.SetActive(false);
    }

    // ==============================
    //  INTERNAL LOGIC
    // ==============================
    private void RefreshUI()
    {
        if (!gameObject.activeInHierarchy) return;

        // Clear cards
        foreach (Transform child in goodCardParent)
            Destroy(child.gameObject);

        var market = _marketManager.marketList[countryId];
        marketNameText.text =
            $"{_countriesManager.GetCountryNameById(countryId)} market";

        foreach (var good in market.goods_list)
        {
            GameObject newGo = Instantiate(goodCard, goodCardParent);
            TextMeshProUGUI[] texts = newGo.GetComponentsInChildren<TextMeshProUGUI>();

            texts[0].text = good.good.name;
            texts[1].text = $"Stockpile: {(good.stockpile / 100f):F2}";
            texts[2].text = $"Supply: {(good.supply / 100f):F2}";
            texts[3].text = $"Demand: {(good.demand / 100f):F2}";
            texts[4].text = $"Price: {(good.price / 100f):F2} £";
        }
    }
}
