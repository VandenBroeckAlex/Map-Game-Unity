using System.Collections.Generic;
using UnityEditor.PackageManager.UI;
using UnityEngine;
using static UnityEngine.UIElements.UxmlAttributeDescription;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    [Header("Layers")]
    public Transform fixedLayer;
    public Transform windowLayer;
    public Transform modalLayer;
    public Transform overlayLayer;

    private List<UIWindow> openedWindows = new();
    private ModalWindow currentModal = null;



    public UI_market_manager marketWindow;

    void Awake()
    {
        instance = this;
    }

    // ---------------------------
    // Modeless Windows
    // ---------------------------
    public void OpenWindow(UIWindow window)
    {
        if (!openedWindows.Contains(window))
        {
            window.transform.SetParent(windowLayer, false);
            window.Show();          // user-defined animation
            openedWindows.Add(window);
        }
    }

    //UIManager.instance.OpenWindow(marketWindow);
    public void CloseWindow(UIWindow window)
    {
        if (openedWindows.Contains(window))
        {
            window.Hide();
            openedWindows.Remove(window);
        }
    }

    // ---------------------------
    // Modal Windows
    // ---------------------------
    public void OpenModal(ModalWindow modal)
    {
        if (currentModal != null) return;   // only one modal at a time

        modal.transform.SetParent(modalLayer, false);
        modal.Show();
        currentModal = modal;
    }

    public void CloseModal()
    {
        if (currentModal == null) return;

        currentModal.Hide();
        currentModal = null;
    }


    public void RequestOpenMarket(int countryId)
    {
        Debug.Log("RequestOpenMarket");
        marketWindow.Initialize(countryId);
        marketWindow.Open();
    }
}

// ---------------------------
// Good Practice
// ---------------------------


/* 
- Never use GameObject.Find()

Use serialized references or transform.Find() inside the prefab.

- Each window is a prefab

-Windows should NEVER reference each other directly

Use events or managers:

diplomacy -> event: “open country window for X”

production window -> event: “select building Y”

This avoids circular dependencies.


DiplomacyWindow:
-> player clicked France
-> raises OnCountryRequested(France)


-> listens -> opens CountryWindow
-> passes France ID to the window
-> CountryWindow fetches data from CountryManager
*/