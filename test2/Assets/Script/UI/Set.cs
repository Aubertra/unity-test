using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Set : MonoBehaviour
{
    public TMP_InputField[] inputFields;
    private int maxValue = 100, minValue = 0;

    private void Start()
    {
        foreach (TMP_InputField inputField in inputFields)
        {
            inputField.onValueChanged.AddListener((value) => onValueChanged(value, inputField));
            inputField.onEndEdit.AddListener((value) => onEndEdit(value, inputField));
        }
    }

    private void onValueChanged(string value,TMP_InputField inputField)
    {
        string digitsOnly = "";
        foreach(char c in value)
        {
            if (char.IsDigit(c))
            {
                digitsOnly += c;
            }
        }
        if(digitsOnly != value)
        {
            inputField.text = digitsOnly;
        }
    }

    private void onEndEdit(string value,TMP_InputField inputField)
    {
        if (string.IsNullOrEmpty(value))
        {
            inputField.text = minValue.ToString();
            return;
        }

        if (int.TryParse(value, out int number))
        {
            if (number < minValue)
                number = minValue;
            else if (number > maxValue)
                number = maxValue;

            inputField.text = number.ToString();
        }
        else
        {
            inputField.text = minValue.ToString();
        }
    }

    public void Exit()
    {
        Destroy(this.gameObject);
    }
}
