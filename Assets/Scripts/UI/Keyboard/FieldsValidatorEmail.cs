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
            Debug.Log("Email v�lido!");
        }
        else
        {
            if (value.Length > 0)
            {
                textStatus.text = "Digite um Email Válido";
                inputField.GetComponent<Image>().color = new Color(1, 0, 0, 0.2f);
                textStatus.gameObject.SetActive(true);
                Debug.Log("Email inv�lido!");
            }
            
            buttonAvancar.interactable = false;
        }
        FormatField(value);
    }

    protected override void FormatField(string value)
    {
        //sem formatação para email
        string formattedCPF = value;
        // Atualiza o texto do campo com o CPF formatado
        inputField.text = formattedCPF;
    }

    protected override bool IsFieldValid(string value)
    {
        // Padrão de expressão regular para validar o e-mail
        string pattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";

        // Verifica se o e-mail corresponde ao padrão
        fieldValid =  Regex.IsMatch(value, pattern);
        return fieldValid;
    }


}
