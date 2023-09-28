using System;
using System.Linq;
using System.Text.RegularExpressions;

using UnityEngine;
using UnityEngine.UI;

public class FieldsValidatorBirth : FieldsValidator
{
    public override void DeleteLetter()
    {
        string fone = new string(inputField.text.Where(char.IsDigit).ToArray());

        if (fone.Length != 0)
        {
            inputField.text = fone.Remove(fone.Length - 1, 1);
        }
    }

    protected override void ValidateField(string value)
    {
        textStatus.gameObject.SetActive(false);
        inputField.GetComponent<Image>().color = inputColor;

        if (IsFieldValid(value))
        {
            Debug.Log("Data válido!");
        }
        else
        {
            if (value.Length > 0)
            {
                textStatus.text = "Digite uma data válida";
                inputField.GetComponent<Image>().color = new Color(1, 0, 0, 0.2f);
                textStatus.gameObject.SetActive(true);

                Debug.Log("Data invalida!");
            }
        }

        FormatField(value);
    }

    protected override void FormatField(string value)
    {
        string formattedData = "";
        string cleanedPhoneNumber = new string(value.Where(char.IsDigit).ToArray());

        if (cleanedPhoneNumber.Length >= 2)
        {
            formattedData = string.Format("{0}/", cleanedPhoneNumber.Substring(0, 2));
        }
        else
        {
            formattedData = cleanedPhoneNumber;
        }

        if (cleanedPhoneNumber.Length >= 3 && cleanedPhoneNumber.Length < 4)
        {
            formattedData += string.Format("{0}", cleanedPhoneNumber.Substring(2, 1));
        }
        

        if (cleanedPhoneNumber.Length >=4)
        {
            formattedData += string.Format("{0}/", cleanedPhoneNumber.Substring(2, Mathf.Min(cleanedPhoneNumber.Length - 2, 2)));
        }

        if (cleanedPhoneNumber.Length >= 5)
        {
            formattedData += string.Format("{0}", cleanedPhoneNumber.Substring(4, Mathf.Min(cleanedPhoneNumber.Length - 4, 4)));
        }

        inputField.text = formattedData;
    }

    protected override bool IsFieldValid(string dataNascimento)
    {
        string expressaoRegular = @"^(\d{2})/(\d{2})/(\d{4})$";
        Regex regex = new Regex(expressaoRegular);

        if (!regex.IsMatch(dataNascimento))
            return false;

        // Extrair os componentes da data
        Match match = regex.Match(dataNascimento);
        int dia = int.Parse(match.Groups[1].Value);
        int mes = int.Parse(match.Groups[2].Value);
        int ano = int.Parse(match.Groups[3].Value);

        if (ano<1900 || ano>= DateTime.Now.Year)
            return false;

        try
        {
            // Tentar criar uma data
            DateTime data = new DateTime(ano, mes, dia);

            // Verificar se a data criada é igual à data de entrada
            return data.Day == dia && data.Month == mes && data.Year == ano;
        }
        catch (Exception)
        {
            return false;
        }
    }
}