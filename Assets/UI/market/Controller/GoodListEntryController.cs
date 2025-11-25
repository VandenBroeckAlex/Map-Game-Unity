using UnityEditor.U2D.Animation;
using UnityEngine;
using UnityEngine.UIElements;

public class GoodListEntryController 
{
    Label m_NameLabel;
    Label goodNameLabel;
    Label goodPriceLabel;
    Label goodSupplyLabel;
    Label goodDemandLabel;
    Label goodStockLabel;
    VisualElement GoodImg;
    VisualElement arrow_img;


    // This function retrieves a reference to the 
    // character name label inside the UI element.
    public void SetVisualElement(VisualElement visualElement)
    {
       // m_NameLabel = visualElement.Q<Label>("character-name");
        //m_NameLabel = visualElement.Q<Label>("goodImg");
        goodNameLabel = visualElement.Q<Label>("GoodName");
        goodPriceLabel = visualElement.Q<Label>("Price");
        goodSupplyLabel = visualElement.Q<Label>("Supply");
        goodDemandLabel = visualElement.Q<Label>("Demand");
        goodStockLabel = visualElement.Q<Label>("Stock");
        GoodImg = visualElement.Q<VisualElement>("Good_Img");
        arrow_img = visualElement.Q<VisualElement>("arrow_img");
    }

    // This function receives the character whose name this list 
    // element is supposed to display. Since the elements list 
    // in a `ListView` are pooled and reused, it's necessary to 
    // have a `Set` function to change which character's data to display.
    public void SetGoodData(Market_object.MarketGood good)
    {
        //m_NameLabel.text = characterData.CharacterName;
         goodNameLabel.text = good.good.name;
         goodPriceLabel.text = good.price.ToString();
         goodSupplyLabel.text = good.supply.ToString();
         goodDemandLabel.text = good.demand.ToString();
         goodStockLabel.text = good.stockpile.ToString();
         //GoodImg = good.good;
         
    }
}
