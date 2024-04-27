using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using TMPro;
using System.Linq;

public class Calendar : MonoBehaviour
{
    [SerializeField] private List<Day> days = new List<Day>();

    public Transform grid;

    public TextMeshProUGUI MonthAndYear;

    public DateTime currDate = DateTime.Now;

    [SerializeField] private Day dayPrefab;

    private Action<Day> onDayClicked;

    public DateTime[] choosedDays;
    public void Init(Action<Day> action,int maxChoosedDays)
    {
        onDayClicked = action;
        choosedDays = new DateTime[maxChoosedDays];
        UpdateCalendar(DateTime.Now.Year, DateTime.Now.Month);
    }
    public void Init(Action<Day> action, int maxChoosedDays, DateTime choosedDay)
    {
        onDayClicked = action;
        choosedDays = new DateTime[maxChoosedDays];
        choosedDays[0] = choosedDay;
        UpdateCalendar(DateTime.Now.Year, DateTime.Now.Month);
    }
    public GameObject Father;
    public void OpenFather()
    {
        Father.SetActive(true);
    }
    public void UpdateCalendar(int year, int month)
    {
        DateTime temp = new DateTime(year, month, 1);
        currDate = temp;
        MonthAndYear.text = temp.ToString("MM") + "." + temp.Year.ToString();
        int startDay = GetMonthStartDay(year,month);
        int endDay = GetTotalNumberOfDays(year, month);

        if (days.Count == 0)
        {
            for (int w = 0; w < 6; w++)
            {
                for (int i = 0; i < 7; i++)
                {
                    Day newDay = Instantiate(dayPrefab, grid);
                    days.Add(newDay);
                }
            }
        }
        for (int i = 0; i < 42; i++)
        {
            if (i < startDay || i - startDay >= endDay)
            {
                days[i].DisableDay();
            }
            else
            {
                days[i].Init(i - startDay + 1, new DateTime(year, month, i - startDay + 1), onDayClicked);

                days[i].EnableDay(false);
            }
        }
        if (DateTime.Now.Year == year && DateTime.Now.Month == month)
        {
            days[(DateTime.Now.Day - 1) + startDay].EnableDay(true);
        }
        SetDayStates();
    }

    public void SetDayStates()
    {
        for(int i = 0;i < choosedDays.Length;i++)
        {
            foreach(var day in days)
            {
                if (choosedDays.Contains(day.DateTime))
                {
                    day.SetDayChoosedState();
                }
                else
                {
                    day.SetDayUnchoosedState();
                }
            }
        }
    }
    int GetMonthStartDay(int year, int month)
    {
        DateTime temp = new DateTime(year, month, 1);

        return (int)temp.DayOfWeek;
    }

    int GetTotalNumberOfDays(int year, int month)
    {
        return DateTime.DaysInMonth(year, month);
    }
    public void SwitchMonth(int direction)
    {
        if(direction < 0)
        {
            currDate = currDate.AddMonths(-1);
        }
        else
        {
            currDate = currDate.AddMonths(1);
        }

        UpdateCalendar(currDate.Year, currDate.Month);
    }
}
