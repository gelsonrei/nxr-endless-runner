using System.Text.RegularExpressions;

using UnityEngine;
using UnityEngine.UI;

public class FieldsValidatorEmail : FieldsValidator
{
    public override void DeleteLetter()
    {
        string value = inputField.text;

        if (value.Length != 0)
        {
            inputField.text = value.Remove(value.Length - 1, 1);
        }
    }

    protected override void ValidateField(string value)
    {
        textStatus.gameObject.SetActive(false);
        inputField.GetComponent<Image>().color = inputColor;

        if (IsFieldValid(value))
        {
            Debug.Log("Email válido!");
        }
        else
        {
            if (value.Length > 0)
            {
                textStatus.text = "Digite um Email Válido";
                inputField.GetComponent<Image>().color = new Color(1, 0, 0, 0.2f);
                textStatus.gameObject.SetActive(true);

                Debug.Log("Email inválido!");
            }
        }

        FormatField(value);
    }

    protected override void FormatField(string value)
    {
        string formattedValue = value;
        inputField.text = formattedValue;
    }

    protected override bool IsFieldValid(string value)
    {
        string pattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";

        fieldValid =  Regex.IsMatch(value, pattern);
        return fieldValid;
    }
}