using System.Linq;

using UnityEngine;
using UnityEngine.UI;

public class FieldsValidatorPhone : FieldsValidator
{

    public override void DeleteLetter()
    {
        string phone = new string(inputField.text.Where(char.IsDigit).ToArray());

        if (phone.Length != 0)
        {
            inputField.text = phone.Remove(phone.Length - 1, 1);
        }
    }

    protected override void ValidateField(string value)
    {
        value = new string(value.Where(char.IsDigit).ToArray());

        textStatus.gameObject.SetActive(false);
        inputField.GetComponent<Image>().color = inputColor;
        
        if (IsFieldValid(value))
        {
            Debug.Log("Fone válido!");
        }
        else
        {
            if (value.Length > 0)
            {
                textStatus.text = "Digite um telefone Válido";
                textStatus.gameObject.SetActive(true);
                inputField.GetComponent<Image>().color = new Color(1, 0, 0, 0.2f);
                
                Debug.Log("Fone inválido!");
            }
        }

        FormatField(value);
    }

    protected override void FormatField(string value)
    {
        string formattedPhoneNumber = "";
        string cleanedPhoneNumber = new string(value.Where(char.IsDigit).ToArray());

        if (cleanedPhoneNumber.Length >= 2)
        {
            formattedPhoneNumber = string.Format("({0}", cleanedPhoneNumber.Substring(0, 2));
        }
        else
        {
            formattedPhoneNumber = cleanedPhoneNumber;
        }

        if (cleanedPhoneNumber.Length >= 3)
        {
            formattedPhoneNumber += string.Format(") {0}", cleanedPhoneNumber.Substring(2, 1));
        }

        if (cleanedPhoneNumber.Length >= 4)
        {
            formattedPhoneNumber += string.Format(" {0}", cleanedPhoneNumber.Substring(3, Mathf.Min(cleanedPhoneNumber.Length - 3, 4)));
        }

        if (cleanedPhoneNumber.Length > 7)
        {
            formattedPhoneNumber += string.Format(" {0}", cleanedPhoneNumber.Substring(7, Mathf.Min(cleanedPhoneNumber.Length - 7, 4)));
        }

        inputField.text = formattedPhoneNumber;
    }

    protected override bool IsFieldValid(string value)
    {
        value = new string(value.Where(char.IsDigit).ToArray());
        fieldValid = false;

        if (value.Length == 11 && value.Distinct().Count() != 1)
            fieldValid = true;

        return fieldValid;
    }
}
