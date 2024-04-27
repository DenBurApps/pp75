using System;
using System.Collections.Generic;

[Serializable]
public class Root
{
    public bool onBoarding;

    public List<Properties> properties = new List<Properties>();
}

[Serializable]
public class Properties
{
    public Properties() 
    {
        Inventory = new List<Resource>();
        Materials = new List<Resource>();
        Equipment = new List<Resource>();
    }
    public Properties(Properties props)
    {
        ProjectName = props.ProjectName;
        ID = props.ID;
        Description = props.Description;
        EndDate = props.EndDate;
        Status = props.Status;

        Inventory = new List<Resource>(props.Inventory);
        Materials = new List<Resource>(props.Materials);
        Equipment = new List<Resource>(props.Equipment);

        Tasks = props.Tasks;
    }

    public string ProjectName;
    public int ID;
    public string Description;
    public string EndDate;

    public string Status;
    public List<Resource> Inventory = new List<Resource>();
    public List<Resource> Materials = new List<Resource>();
    public List<Resource> Equipment = new List<Resource>();

    public List<TaskL> Tasks = new List<TaskL>();
}
[Serializable]
public class Resource
{
    public Resource() { }
    public Resource(Resource resource)
    {
        Name = resource.Name;
        Count = resource.Count;
        UnitName = resource.UnitName;
        Status = resource.Status;
    }
    public string Name;
    public int Count;
    public string UnitName;
    public string Status = "In stock";
}
[Serializable]
public class TaskL
{
    public TaskL() { }  
    public TaskL(TaskL task)
    {
        Name = task.Name;
        Description = task.Description;
        Person = task.Person;
        Cost = task.Cost;
        Status = task.Status;
        StartDate = task.StartDate;
        EndDate = task.EndDate;
    }
    public string Name;
    public string Description;
    public string Person;
    public float Cost;
    public string Status = "Not started";
    public string StartDate;
    public string EndDate;
}
