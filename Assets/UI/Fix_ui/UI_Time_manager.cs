using TMPro;
using UnityEngine;
using static Goods;

public class UI_Time_manager : MonoBehaviour
{
    public static UI_Time_manager instance;
    public GameObject UI_time;
    public Transform fixed_ui;

    public void Initialize()
    {
        instance = this;
    }
    private void OnEnable()
    {
        DateHandeler.onDateChanged += DisplayStringDate;
    }
    private void OnDisable()
    {
        DateHandeler.onDateChanged -= DisplayStringDate;
    }

     void DisplayStringDate(string date)
    {
        TextMeshProUGUI[] texts = UI_time.GetComponentsInChildren<TextMeshProUGUI>();
        texts[0].text = date;
        Debug.Log(date);

    }
}