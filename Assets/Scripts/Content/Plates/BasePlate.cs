using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BasePlate : MonoBehaviour
{
    public Properties properties;

    [SerializeField] private TextMeshProUGUI Name;
    [SerializeField] private TextMeshProUGUI Description;
    [SerializeField] private TextMeshProUGUI EndDate;

    [SerializeField] private Preview preview;
    private Transform previewSpawnPlace;
    public void Init(Properties props,Transform previewSpawnPlace)
    {
        properties = props;
        this.previewSpawnPlace = previewSpawnPlace;

        Name.text = props.ProjectName;
        Name.gameObject.GetComponent<TruncateText>().Truncate();

        Description.text = props.Description;
        Description.gameObject.GetComponent<TruncateText>().Truncate();

        EndDate.text = props.EndDate;
    }

    public void OpenPreview()
    {
        var obj = Instantiate(preview,previewSpawnPlace);
        obj.Init(properties);
    }
}
