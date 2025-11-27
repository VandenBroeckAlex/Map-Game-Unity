using UnityEngine;

public class MarketButton : MonoBehaviour
{
  
    public UI_market_manager marketUI;  // assigned in inspector

    public void OnButtonClicked()
    {
        marketUI.gameObject.SetActive(true);
    }
}
