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

    private TickScript m_tickScript;
    public void Initilize()
    {
        m_tickScript = TickScript.instance;
        _marketManager = MarketManager.instance;
    }
    public void OnGamePausePress()
    {
        Debug.Log("Pause clicked");
        // m_tickScript.PauseGame();
        Debug.Log(_marketManager.marketList.Count);
        Market_object.Market _market = _marketManager.marketList[0];
        
        foreach(Market_object.MarketGood good in _market.goods_list)
        {
            GameObject newGo = Instantiate(goodCard, goodCardParent);
            TextMeshProUGUI[] texts = newGo.GetComponentsInChildren<TextMeshProUGUI>();
            texts[0].text = $"{good.good.name} : {good.price} £ ";
        }
    }
    public void OnGameResumePress()
    {
        Debug.Log("Resume cliked");
        m_tickScript.ThreeSpeed();
    }
 
}
