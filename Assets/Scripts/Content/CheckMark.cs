using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CheckMark : MonoBehaviour
{
    [SerializeField] private GameObject check;
    public TextMeshProUGUI status;

    public string ReturnStatus()
    {
        check.SetActive(true);
        return status.text;
    }
    public void DeactivateCheck()
    {
        check.SetActive(false);
    }
}
