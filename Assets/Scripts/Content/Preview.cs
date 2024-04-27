using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Preview : MonoBehaviour
{
    public static Preview Instance;

    public Properties properties;

    public TextMeshProUGUI Name;
    public TextMeshProUGUI Description;
    public TextMeshProUGUI EndDate;
    public TextMeshProUGUI Cost;
    [Header("Task")]
    public TextMeshProUGUI TaskCount;

    public TMP_Dropdown dropdown;
    public TaskPlate TaskPlate;

    public void Init(Properties props)
    {
        properties = props;
        Name.text = props.ProjectName;
        Description.text = props.Description;
        EndDate.text = props.EndDate;
        TaskCount.text = props.Tasks.Count.ToString();
        float cost = 0;
        foreach (var obj in props.Tasks)
        {
            cost += obj.Cost;
        }
        Cost.text = cost.ToString() + "$";

        InitTask();

        ClearResourcesList(InventoryPlates);
        ClearResourcesList(MaterialPlates);
        ClearResourcesList(EquipmentPlates);

        InventoryPlates = FillResourcePlatesList(props.Inventory, InventorySpawnPlace);
        MaterialPlates = FillResourcePlatesList(props.Materials, MaterialsSpawnPlace);
        EquipmentPlates = FillResourcePlatesList(props.Equipment, EquipmentSpawnPlace);

        CheckEmpty();
    }
    public void DeleteProject()
    {
        DataProcessor.Instance.DeletePlate(properties);
        Destroy(gameObject);
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

        if (properties.Tasks.Count == 0)
            TaskEmpty.SetActive(true);
        else
            TaskEmpty.SetActive(false);
    }
    private void InitTask()
    {
        TaskPlate.Blocker.SetActive(true);

        List<string> list = new List<string>();

        foreach(var obj in properties.Tasks)
        {
            list.Add(obj.Name);
        }
        dropdown.ClearOptions();
        dropdown.AddOptions(list);

        dropdown.onValueChanged.RemoveAllListeners();
        dropdown.onValueChanged.AddListener(ChangeTask);

        if (properties.Tasks.Count != 0)
        {
            TaskPlate.gameObject.SetActive(true);
            TaskPlate.Init(properties.Tasks[0], null, null,null);
        }
        else
            TaskPlate.gameObject.SetActive(false);
    }
    public void ChangeTask(int i)
    {
        TaskPlate.Init(properties.Tasks[i], null, null, null);
    }

    [Header("Resources")]
    public ResourcePlate ResourcePlate;

    private List<ResourcePlate> InventoryPlates = new List<ResourcePlate>();
    private List<ResourcePlate> MaterialPlates = new List<ResourcePlate>();
    private List<ResourcePlate> EquipmentPlates = new List<ResourcePlate>();

    public Transform InventorySpawnPlace;
    public Transform MaterialsSpawnPlace;
    public Transform EquipmentSpawnPlace;
    private List<ResourcePlate> FillResourcePlatesList(List<Resource> dataList, Transform spawnPlace)
    {
        List<ResourcePlate> list = new List<ResourcePlate>();

        foreach (var item in dataList)
        {
            var obj = Instantiate(ResourcePlate, spawnPlace);
            obj.GetComponent<RectTransform>().SetSiblingIndex(1);
            obj.Init(item,null);
            list.Add(obj);

            obj.Blocker.SetActive(true);
        }
        return list;
    }
    private void ClearResourcesList(List<ResourcePlate> list)
    {
        foreach(var obj in list)
        {
            Destroy(obj.gameObject);
        }
        list.Clear();
    }
    private void Awake()
    {
        Instance = this;
    }
    public EditPlate editPlateWindow;
    public void OpenEditWindow()
    {
        var obj = Instantiate(editPlateWindow, transform);
        obj.Init(properties,this);
    }
}
