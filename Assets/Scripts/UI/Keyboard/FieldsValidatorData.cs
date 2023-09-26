using UnityEngine;
using UnityEngine.UI;

public class FieldsValidatorData : FieldsValidator
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
            Debug.Log("Campo válido!");
        }
        else
        {
            if (value.Length > 0)
            {
                textStatus.text = "Digite até 4 caracteres";
                inputField.GetComponent<Image>().color = new Color(1, 0, 0, 0.2f);
                textStatus.gameObject.SetActive(true);

                Debug.Log("Campo inválido!");
            }
        }

        FormatField(value);
    }

    protected override void FormatField(string value)
    {
        inputField.text = value;
    }

    protected override bool IsFieldValid(string value)
    {
        fieldValid = false;

        if (value.Length > 4)
            fieldValid = true;

        return fieldValid;
    }
}
