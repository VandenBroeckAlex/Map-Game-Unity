using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.U2D.Animation;
using UnityEngine;
using UnityEngine.UIElements;

public class CharacterListController
{
    // UXML template for list entries
    VisualTreeAsset GoodCard;
    MarketManager m_MarketManager = MarketManager.instance;
    // UI element references
    ListView m_GoodList;
    Label m_CharClassLabel;
    Label goodNameLabel;
    VisualElement GoodImg;

    Label goodPriceLabel;
    Label goodSupplyLabel;
    Label goodDemandLabel;
    Label goodStockLabel;
    VisualElement arrow_img;

    List<Market_object.MarketGood> m_AllGoods;

    public void InitializeCharacterList(VisualElement root, VisualTreeAsset listElementTemplate)
    {
        EnumerateAllCharacters();

        // Store a reference to the template for the list entries
        GoodCard = listElementTemplate;

        // Store a reference to the character list element
        m_GoodList = root.Q<ListView>("good-list-view");

        // Store references to the selected character info elements
        m_CharClassLabel = root.Q<Label>("character-class");
        goodNameLabel = root.Q<Label>("goodNameLabel");
        //GoodImg = root.Q<VisualElement>("character-portrait");

        FillGoodList();

        // Register to get a callback when an item is selected
        m_GoodList.selectionChanged += OnGoodSelected;
    }

    void EnumerateAllCharacters()
    {
        m_AllGoods = new List<Market_object.MarketGood>();
        m_AllGoods.AddRange(m_MarketManager.marketList[0].goods_list);
        Debug.Log("Goods count: " + m_AllGoods.Count);
    }

    void FillGoodList()
    {
        // Set up a make item function for a list entry
        m_GoodList.makeItem = () =>
        {
            // Instantiate the UXML template for the entry
            var newListEntry = GoodCard.Instantiate();

            // Instantiate a controller for the data
            var newListEntryLogic = new GoodListEntryController();

            // Assign the controller script to the visual element
            newListEntry.userData = newListEntryLogic;

            // Initialize the controller script
            newListEntryLogic.SetVisualElement(newListEntry);

            // Return the root of the instantiated visual tree
            return newListEntry;
        };

        // Set up bind function for a specific list entry
        m_GoodList.bindItem = (item, index) =>
        {
            (item.userData as GoodListEntryController)?.SetGoodData(m_AllGoods[index]);
        };

        // Set a fixed item height matching the height of the item provided in makeItem. 
        // For dynamic height, see the virtualizationMethod property.
        m_GoodList.fixedItemHeight = 45;

        // Set the actual item's source list/array
        m_GoodList.itemsSource = m_AllGoods;
    }

    void OnGoodSelected(IEnumerable<object> selectedItems)
    {
        // Get the currently selected item directly from the ListView
        var selectedGood = m_GoodList.selectedItem as Market_object.MarketGood;

        // Handle none-selection (Escape to deselect everything)
        if (selectedGood == null)
        {
            // Clear
            m_CharClassLabel.text = "";
            goodNameLabel.text = "";
            GoodImg.style.backgroundImage = null;

            return;
        }

        // Fill in character details
        //m_CharClassLabel.text = selectedGood.Class.ToString();
        goodNameLabel.text = selectedGood.good.name;
        //GoodImg.style.backgroundImage = new StyleBackground(selectedGood.good.icon);
    }
}
