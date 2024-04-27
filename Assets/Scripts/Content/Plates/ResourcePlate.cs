using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResourcePlate : MonoBehaviour
{
    public Resource Resource;

    public InputFieldChanger NameField;
    public InputFieldChanger CountField;
    public InputFieldChanger UnitNameField;

    public GameObject Blocker;

    public Resource GetData() 
    {
        Resource.Name = NameField.text;
        int.TryParse(CountField.text, out int Count);
        Resource.Count = Count;
        Resource.UnitName = UnitNameField.text;
        
        return Resource;
    }
    private void Awake()
    {
        foreach(var mark in marks)
        {
            mark.GetComponent<Button>().onClick.AddListener(() => { ChangeStatus(mark); });
        }
    }
    public Button DeleteButton;
    public void Init(Resource resource,EditPlate editPlate)
    {
        if(editPlate == null)
        {
            DeleteButton.gameObject.SetActive(false);
        }
        else
        {
            DeleteButton.onClick.AddListener(() =>
            {
                editPlate.DeleteRosourcePlate(this);

            });
        }
        Resource = resource;
        NameField.ChangeText(resource.Name);
        CountField.ChangeText(resource.Count.ToString());
        UnitNameField.ChangeText(resource.UnitName);

        foreach(var mark in marks)
        {
            if(mark.status.text == resource.Status)
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
        Resource.Status = checkedMark.ReturnStatus();
    }
}
