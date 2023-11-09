using System.Linq;

using UnityEngine;
using UnityEngine.UI;

public class FieldsValidatorCpf : FieldsValidator
{
    [SerializeField] protected Toggle noCPFToggle;

    void Start()
    {
        fieldValid = false;
        textStatus.gameObject.SetActive(false);
        inputColor = inputField.GetComponent<Image>().color;

        noCPFToggle.onValueChanged.AddListener(delegate 
        {
            if (noCPFToggle.isOn)
            {
                inputField.interactable = false;
                inputField.text = "000.000.000-00";
                
            }
            else
            {
                inputField.interactable = true;
                inputField.text = "";
            }
        });
    }

    protected override void ValidateField(string value)
    {
        textStatus.gameObject.SetActive(false);
        inputField.GetComponent<Image>().color = inputColor;

        if (IsFieldValid(value))
        {
            //bool existsInLeadsPremio = LeadSorteioManager.GetOneToday(value) != null;
            bool existsInLeadsPremio = false;

            if (existsInLeadsPremio && value != "000.000.000-00")
            {
                textStatus.text = "Você já recebeu seu prêmio hoje ;-)";
                textStatus.gameObject.SetActive(true);
            }

            Debug.Log("CPF válido!");
            
            inputField.GetComponent<Image>().color = validColor;
        }
        else
        {
            if (value.Length == 14)
            {
                textStatus.text = "Digite um CPF Válido";
                inputField.GetComponent<Image>().color = new Color(1, 0, 0, 0.2f);
                textStatus.gameObject.SetActive(true);
                
                Debug.Log("CPF inválido!");
            }
        }

        FormatField(value);
    }

    protected override void FormatField(string value)
    {
        value = new string(value.Where(char.IsDigit).ToArray());
        value = value.Substring(0, Mathf.Min(value.Length, 11));

        if (value.Length > 0 && !noCPFToggle.isOn)
        {
            noCPFToggle.gameObject.SetActive(false);
        }
        else
        {
            noCPFToggle.gameObject.SetActive(true);
        }

        string formattedCPF = "";
        for (int i = 0; i < value.Length; i++)
        {
            formattedCPF += value[i];

            if ((i == 2 && value.Length > 2) || (i == 5 && value.Length > 5))
            {
                formattedCPF += ".";
            }
            else if (i == 8 && value.Length > 8)
            {
                formattedCPF += "-";
            }
        }

        inputField.text = formattedCPF;
    }

    protected override bool IsFieldValid(string value)
    {
        value = new string(value.Where(char.IsDigit).ToArray());

        if (value.Length != 11)
        {
            return false;
        }

        if (value.Distinct().Count() == 1)
        {
            if (value[0]=='0' && !inputField.interactable) //permite o "000.000.000-00" para o "não possuo cpf"
                return true;
            return false;
        }

        int sum = 0;
        for (int i = 0; i < 9; i++)
        {
            sum += int.Parse(value[i].ToString()) * (10 - i);
        }
        int firstVerifierDigit = (sum * 10) % 11;
        if (firstVerifierDigit == 10)
        {
            firstVerifierDigit = 0;
        }

        sum = 0;
        for (int i = 0; i < 10; i++)
        {
            sum += int.Parse(value[i].ToString()) * (11 - i);
        }
        int secondVerifierDigit = (sum * 10) % 11;
        if (secondVerifierDigit == 10)
        {
            secondVerifierDigit = 0;
        }

        fieldValid = firstVerifierDigit == int.Parse(value[9].ToString()) && secondVerifierDigit == int.Parse(value[10].ToString());
        
        return fieldValid;
    }
}
