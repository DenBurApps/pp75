using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TaskPlate : MonoBehaviour
{
    public TaskL Task;

    public InputFieldChanger NameField;
    public InputFieldChanger DescriptionField;
    public InputFieldChanger PersonField;
    public InputFieldChanger CostField;

    public GameObject Blocker;
    public TaskL GetData()
    {
        Task.Name = NameField.text;
        Task.Description = DescriptionField.text;
        Task.Person = PersonField.text;

        int.TryParse(CostField.text, out int Cost);
        Task.Cost = Cost;

        return Task;
    }

    private Calendar StartCalendar;
    private Calendar EndCalendar;
    public Button DeleteButton;

    public void Init(TaskL task,Calendar startCalendar,Calendar endCalendar, EditPlate editPlate)
    {
        if (editPlate == null)
        {
            DeleteButton.gameObject.SetActive(false);
        }
        else
        {
            DeleteButton.onClick.AddListener(() =>
            {
                editPlate.DeleteTaskPlate(this);
            });
        }
        Task = new TaskL(task);
        NameField.ChangeText(task.Name);
        DescriptionField.ChangeText(task.Description);
        PersonField.ChangeText(task.Person);
        CostField.ChangeText(task.Cost.ToString());

        StartCalendar = startCalendar;
        EndCalendar = endCalendar;

        StartDateTMP.text = task.StartDate;
        EndDateTMP.text = task.EndDate;

        foreach (var mark in marks)
        {
            if (mark.status.text == Task.Status)
            {
                ChangeStatus(mark);
                break;
            }
        }
    }
    public CheckMark[] marks;
    public void ChangeStatus(CheckMark checkedMark)
    {
        foreach (var mark in marks)
        {
            mark.DeactivateCheck();
        }
        Task.Status = checkedMark.ReturnStatus();
    }
    private void Awake()
    {
        foreach (var mark in marks)
        {
            mark.GetComponent<Button>().onClick.AddListener(() => { ChangeStatus(mark); });
        }
    }
    public void OpenStartCalendar()
    {
        if(Task.StartDate != "")
        {
            DateTime.TryParse(Task.StartDate, out DateTime date);

            StartCalendar.Init(GetStartDay, 1, date);
        }
        else
            StartCalendar.Init(GetStartDay, 1);

        StartCalendar.SetDayStates();
        StartCalendar.OpenFather();
    }
    public void OpenEndCalendar()
    {
        if (Task.EndDate != "")
        {
            DateTime.TryParse(Task.EndDate, out DateTime date);

            EndCalendar.Init(GetEndDay, 1, date);
        }
        else
            EndCalendar.Init(GetEndDay, 1);

        EndCalendar.SetDayStates();
        EndCalendar.OpenFather();
    }

    public TextMeshProUGUI EndDateTMP;
    public TextMeshProUGUI StartDateTMP;

    private void GetStartDay(Day day)
    {
        var date = day.DateTime.ToString();

        Task.StartDate = date.Remove(10);
        StartDateTMP.text = date.Remove(10);
        StartCalendar.choosedDays[0] = day.DateTime;
        StartCalendar.SetDayStates();

    }
    private void GetEndDay(Day day)
    {
        var date = day.DateTime.ToString();

        Task.EndDate = date.Remove(10);
        EndDateTMP.text = date.Remove(10);
        EndCalendar.choosedDays[0] = day.DateTime;
        EndCalendar.SetDayStates();

    }
}
