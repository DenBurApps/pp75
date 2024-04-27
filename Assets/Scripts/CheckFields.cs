using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CheckFields : MonoBehaviour
{
    public InputFieldChanger[] Fields;
    public Button Button;

    private void FixedUpdate()
    {
        bool canEnableButton = true;
        foreach (var field in Fields)
            if(field.text == "")
                canEnableButton = false;

        if (canEnableButton) Button.interactable = true;

        else Button.interactable = false;
    }
}
