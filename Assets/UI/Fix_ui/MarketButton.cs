using MyGame.Data;
using UnityEngine;

public class MarketButton : MonoBehaviour
{
  
    public UI_market_manager marketUI;  // assigned in inspector

    public void OnButtonClicked()
    {
        int id = PlayerManager.instance.GetHumanPlayer().countryID;
        UIManager.instance.RequestOpenMarket(0);
    }
}
