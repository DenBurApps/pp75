using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EditPlate : MonoBehaviour
{
    private Properties properties = new Properties();
    [Header("window 1")]
    public InputFieldChanger ProjectName;
    public InputFieldChanger ProjectDescription;
    public Calendar calendar;
    public TextMeshProUGUI EndDateTMP;

    [Header("window 2")]
    public Transform InventorySpawnPlace;
    public Transform MaterialsSpawnPlace;
    public Transform EquipmentSpawnPlace;

    public ResourcePlate ResourcePlate;

    public List<ResourcePlate> InventoryPlates = new List<ResourcePlate>();
    public List<ResourcePlate> MaterialPlates = new List<ResourcePlate>();
    public List<ResourcePlate> EquipmentPlates = new List<ResourcePlate>();

    public Button W2Button;
    [Header("window 3")]
    public Transform TaskSpawnPlace;
    public TaskPlate TaskPlate;
    private List<TaskPlate> TaskPlates = new List<TaskPlate>();
    public Calendar StartCalendar;
    public Calendar EndCalendar;

    public Button ContinueButton;
    private Preview preview;
    public void Init(Properties props,Preview preview)
    {
        properties = new Properties(props);
        this.preview = preview;
        ProjectName.ChangeText(props.ProjectName);
        ProjectDescription.ChangeText(props.Description);

        InventoryPlates = FillResourcePlatesList(props.Inventory, InventorySpawnPlace);
        MaterialPlates = FillResourcePlatesList(props.Materials, MaterialsSpawnPlace);
        EquipmentPlates = FillResourcePlatesList(props.Equipment, EquipmentSpawnPlace);

        foreach(var item in props.Tasks)
        {
            var obj = Instantiate(TaskPlate, TaskSpawnPlace);
            obj.GetComponent<RectTransform>().SetSiblingIndex(1);
            obj.Init(item, StartCalendar, EndCalendar,this);
            TaskPlates.Add(obj);
        }
        if(properties.EndDate != "")
        {
            DateTime.TryParse(properties.EndDate, out DateTime date);
            EndDateTMP.text = properties.EndDate;

            calendar.Init(GetDay, 1, date);
        }

        ContinueButton.onClick.RemoveAllListeners();
        ContinueButton.onClick.AddListener(() => 
        {
            SavePlateData();
            Destroy(gameObject);
        });
        CheckEmpty();

        foreach(var item in marks)
        {
            if(item.status.text == properties.Status)
                item.ReturnStatus();
            else
                item.DeactivateCheck();
        }
    }
    public void FixedUpdate()
    {
        CanContinueW2();
        CanContinueW3();
    }
    public void CanContinueW3()
    {
        if(TaskPlates.Count() < 1)
        {
            ContinueButton.interactable = false;
            return;
        }
        foreach(var item in TaskPlates)
        {
            if(item.NameField.text == "")
            {
                ContinueButton.interactable = false;
                return;
            }
            if (item.DescriptionField.text == "")
            {
                ContinueButton.interactable = false;
                return;
            }
            if (item.PersonField.text == "")
            {
                ContinueButton.interactable = false;
                return;
            }
            if (item.CostField.text == "")
            {
                ContinueButton.interactable = false;
                return;
            }
            if(item.Task.StartDate == "")
            {
                ContinueButton.interactable = false;
                return;
            }
            if (item.Task.EndDate == "")
            {
                ContinueButton.interactable = false;
                return;
            }
        }

        ContinueButton.interactable = true;

    }
    public void CanContinueW2()
    {
        var list = InventoryPlates.Union(MaterialPlates).Union(EquipmentPlates);
        if(list.Count() < 3)
        {
            W2Button.interactable = false;
            return;
        }
        foreach (var item in list)
        {
            if(item.NameField.text == "")
            {
                W2Button.interactable = false;
                return;
            }
            if (item.CountField.text == "")
            {
                W2Button.interactable = false;
                return;
            }
            if (item.UnitNameField.text == "")
            {
                W2Button.interactable = false;
                return;
            }
        }
        W2Button.interactable = true;
    }

    private void Awake()
    {
        calendar.Init(GetDay, 1);

        ContinueButton.onClick.AddListener(() =>
        {
            CreatePlateData();
            Destroy(gameObject);
        });
        CheckEmpty();
        ChangeStatus(marks[0]);
    }

    public CheckMark[] marks;

    public void ChangeStatus(CheckMark checkedMark)
    {
        foreach (var mark in marks)
        {
            mark.DeactivateCheck();
        }
        properties.Status = checkedMark.ReturnStatus();
    }
    public GameObject InventoryEmpty;
    public GameObject MaterialEmpty;
    public GameObject EquipmentEmpty;
    public GameObject TaskEmpty;

    private void CheckEmpty()
    {
        if (InventoryPlates.Count == 0)
            InventoryEmpty.SetActive(true);
        else
            InventoryEmpty.SetActive(false);

        if (MaterialPlates.Count == 0)
            MaterialEmpty.SetActive(true);
        else
            MaterialEmpty.SetActive(false);

        if (EquipmentPlates.Count == 0)
            EquipmentEmpty.SetActive(true);
        else
            EquipmentEmpty.SetActive(false);

        if(TaskPlates.Count == 0)
            TaskEmpty.SetActive(true);
        else
            TaskEmpty.SetActive(false);
    }
    public void SavePlateData()
    {
        FillPlateData();
        DataProcessor.Instance.EditPlate(properties);
        preview.Init(properties);
    }

    public void CreatePlateData()
    {
        FillPlateData();
        DataProcessor.Instance.AddNewPlate(properties);

    }

    private void FillPlateData()
    {
        properties.ProjectName = ProjectName.text;
        properties.Description = ProjectDescription.text;

        properties.Inventory = GetInfoFromResourcePlates(InventoryPlates);
        properties.Materials = GetInfoFromResourcePlates(MaterialPlates);
        properties.Equipment = GetInfoFromResourcePlates(EquipmentPlates);

        properties.Tasks.Clear();
        foreach (var item in TaskPlates)
            properties.Tasks.Add(item.GetData());
        
    }
    private List<Resource> GetInfoFromResourcePlates(List<ResourcePlate> list)
    {
        List<Resource> dataList = new List<Resource>();

        foreach (var item in list)
        {
            dataList.Add(item.GetData());
        }
        return dataList;
    }
    private List<ResourcePlate> FillResourcePlatesList(List<Resource> dataList, Transform spawnPlace)
    {
        List<ResourcePlate> list = new List<ResourcePlate>();

        foreach (var item in dataList)
        {
            var obj = Instantiate(ResourcePlate, spawnPlace);
            obj.GetComponent<RectTransform>().SetSiblingIndex(1);
            obj.Init(item,this);
            list.Add(obj);
        }
        return list;
    }

    private void GetDay(Day day)
    {
        var date = day.DateTime.ToString();

        calendar.choosedDays[0] = day.DateTime;
        properties.EndDate = date.Remove(10);
        EndDateTMP.text = date.Remove(10);
        calendar.SetDayStates();
    }

    public void AddnewRecourcePlate(int listNumber)
    {
        switch (listNumber)
        {
            case 0:
                {
                    var obj = Instantiate(ResourcePlate, InventorySpawnPlace);
                    InventoryPlates.Add(obj);
                    obj.GetComponent<RectTransform>().SetSiblingIndex(1);
                    obj.Init(new Resource(), this);
                    break;
                }
            case 1:
                {
                    var obj = Instantiate(ResourcePlate, MaterialsSpawnPlace);
                    MaterialPlates.Add(obj);
                    obj.Init(new Resource(), this);

                    break;
                }
            case 2:
                {
                    var obj = Instantiate(ResourcePlate, EquipmentSpawnPlace);
                    EquipmentPlates.Add(obj);
                    obj.Init(new Resource(), this);

                    break;
                }
        }
        CheckEmpty();
    }
    public void DeleteRosourcePlate(ResourcePlate plate)
    {
        foreach(var obj in InventoryPlates)
        {
            if(obj == plate)
            {
                Destroy(plate.gameObject);
                properties.Inventory.Remove(obj.Resource);
                InventoryPlates.Remove(obj);
                CheckEmpty();
                return;
            }
        }
        foreach (var obj in MaterialPlates)
        {
            if (obj == plate)
            {
                Destroy(plate.gameObject);
                properties.Materials.Remove(obj.Resource);
                MaterialPlates.Remove(obj);
                CheckEmpty();
                return;
            }
        }
        foreach (var obj in EquipmentPlates)
        {
            if (obj == plate)
            {
                Destroy(plate.gameObject);
                properties.Equipment.Remove(obj.Resource);
                EquipmentPlates.Remove(obj);
                CheckEmpty();
                return;
            }
        }
    }
    public void DeleteTaskPlate(TaskPlate plate)
    {
        foreach (var obj in TaskPlates)
        {
            if (obj == plate)
            {
                Destroy(obj.gameObject);
                properties.Tasks.Remove(obj.Task);
                TaskPlates.Remove(obj);
                CheckEmpty();
                return;
            }
        }
    }
    public void AddNewTaskPlate()
    {
        var obj = Instantiate(TaskPlate, TaskSpawnPlace);
        obj.GetComponent<RectTransform>().SetSiblingIndex(1);
        obj.Init(new TaskL(), StartCalendar, EndCalendar,this);
        TaskPlates.Add(obj);
        CheckEmpty();
    }
}
