
using System;
using UnityEngine;
using static Tick_script;

public class DateHandeler : MonoBehaviour
{
    public delegate void OnMonth();
    public static OnMonth onMonth;

    private string[] strWeekDayList = { "Monday", "Tuesday", "Wednesday", "Thursday","Friday", "Saturday", "Sunday" };
    private string[] strMonthsList = { "January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December" };
    private byte monthDay;
    private int month = 1;
    private int year = 1;
    private byte weekDay = 0;
    private string strMonth = "";



    private void OnEnable()
    {
        Tick_script.onTick += LogWord;
        Tick_script.onTick += HandleDate;
    }

    private void OnDisable()
    {
        Tick_script.onTick -= LogWord;
        Tick_script.onTick -= HandleDate;

    }




    private void HandleDate()
    {
        HandleMonthDayChange();
        Debug.Log("The date is: " + GetStrDay() + " - " + monthDay + "/" + GetStrMonth() + "/ " + year);

    }



    private void LogWord()
    {
        Debug.Log("success !");
    }

    private void HandleMonthDayChange()
    {
        monthDay++;

        if (monthDay == 2)
        {
            DateHandeler.onMonth?.Invoke();
        }


        switch (month)
        {
            case 1:
            case 3:
            case 5:
            case 7:
            case 8:
            case 10:
            case 12:
                ThirtyOneDaysMonth();
                break;
            case 2:
                TwentyNineDaysMonth();
                break;
            case 4:
            case 6:
            case 9:
            case 11:

                ThirtyDaysMonth();
                break;

        }                   
    }

    // keep tracks of the day of the week  
    // return a string
    private string GetStrDay()
    {

        weekDay++;

        if (weekDay > 7)
        {
            weekDay = 1;

        }       
        int num = weekDay - 1;
        string StrDay = strWeekDayList[num];
        return StrDay;
    }
    private string GetStrMonth()
    {
        int num = month - 1;
        strMonth = strMonthsList[num];
        return strMonth;
    }
    private void TwentyNineDaysMonth()
    {
        if (monthDay == 29)
        {
            monthDay = 1;
            month++;
        }

    }
    private void ThirtyDaysMonth()
    {
        if (monthDay == 31)
        {
            monthDay = 1;
            month++;
        }

    }
    private void ThirtyOneDaysMonth()
    {
        if (monthDay > 31)
        {
            monthDay = 1;

            if (month != 12)
            {
                month++;

            }
            else
            {
                month = 1;
                year++;
            }
        }

    }

    
}
  

