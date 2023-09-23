using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;


public class KeyboardManager : MonoBehaviour
{
    public static KeyboardManager Instance;
    static InputField textBox;

    public GameObject FullKeyboard;
    public GameObject AlphaNumericKeyboard;
    public GameObject NumericKeyboard;

    private void Start()
    {
        Instance = this;

        //textBox.text = "";
    }

    public void DeleteLetter()
    {
        if (textBox.GetComponent<FieldsValidator>())
        {
            textBox.GetComponent<FieldsValidator>().DeleteLetter();
        }
        else if (textBox.text.Length != 0)
        {
            textBox.text = textBox.text.Remove(textBox.text.Length - 1, 1);
        }
    }

    public void AddLetter(string letter)
    {
        if (textBox)
            textBox.text = textBox.text + letter;
    }

    public void SubmitWord()
    {
        if (textBox)
            textBox.text = "";

        Debug.Log("Text submitted successfully!");
    }

    public void ResetFields()
    {
        if(textBox)
            textBox.text = "";
    }

    public void SetTextBox(InputField inputField)
    {
        textBox = inputField;
    }

    public void activeFullKeyboard()
    {
        FullKeyboard.SetActive(true);
        AlphaNumericKeyboard.SetActive(false);
        NumericKeyboard.SetActive(false);
    }

    public void activeAlphaNUmericKeyboard()
    {
        AlphaNumericKeyboard.SetActive(true);
        FullKeyboard.SetActive(false);
        NumericKeyboard.SetActive(false);
    }

    public void activeNumericKeyboard()
    {
        NumericKeyboard.SetActive(true);
        FullKeyboard.SetActive(false);
        AlphaNumericKeyboard.SetActive(false);
    }
}
