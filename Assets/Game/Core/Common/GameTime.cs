using System;
using static TickSystem;


public class GameTime 
{




    public event Action OnMonth;
    public event Action<string> OnDateChanged;

    private readonly TickSystem tickSystem;

    private readonly string[] strWeekDayList = { "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday", "Sunday" };
    private readonly string[] strMonthsList = { "January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December" };
    
    public byte day;
    private int month = 1;
    private int year = 1814;
    private byte weekDay = 0;
    private string strMonth = "";

    public GameTime(TickSystem tickSystem)
    {
        this.tickSystem = tickSystem;
        tickSystem.OnTick += HandleTick;
    }

    public void Dispose()
    {
        tickSystem.OnTick -= HandleTick;
    }
    private void HandleTick()
    {
        AdvanceDay();
        OnDateChanged?.Invoke(GetDateString());
    }

    private void AdvanceDay()
    {
        day++;
        HandleMonthDayChange();
        string _string = GetDateString();
        OnDateChanged?.Invoke(_string);
    }

    public string GetDateString()
    {
        return $"{GetStrMonth()} {day}, {year}";
    }

    private void HandleMonthDayChange()
    {
        

        if (day == 2)
        {
            OnMonth?.Invoke();
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
        if (day > 29)
        {
            day = 1;
            month++;
        }

    }
    private void ThirtyDaysMonth()
    {
        if (day > 30)
        {
            day = 1;
            month++;
        }

    }
    private void ThirtyOneDaysMonth()
    {
        if (day > 31)
        {
            day = 1;

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


