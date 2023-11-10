using System.Linq;

using UnityEngine;
using UnityEngine.UI;

public abstract class FieldsValidator : MonoBehaviour
{
    [SerializeField] protected InputField inputField;
    [SerializeField] protected Text textStatus;

    protected bool fieldValid = false;
    protected Color inputColor;
    protected Color validColor = new (0.57f, 0.924f, 0.62f, 0.39f);
    protected Color invalidColor = new(0.98f, 0.87f, 0.87f, 1.0f);
    protected Color blankColor = new(1.0f, 1.0f, 1.0f, 1.0f);

    public virtual void DeleteLetter()
    {
        string value = new string(inputField.text.Where(char.IsDigit).ToArray());

        if (value.Length != 0)
        {
            inputField.text = value.Remove(value.Length - 1, 1);
        }
    }

    protected abstract void ValidateField(string value);

    protected abstract void FormatField(string value);

    protected abstract bool IsFieldValid(string value);

    public bool GetIsFieldValid()
    {
        return IsFieldValid(inputField.text);
    }

    protected virtual void Start()
    {
        fieldValid = false;
        textStatus.gameObject.SetActive(false);
        inputColor = inputField.GetComponent<Image>().color;
    }

    private void OnEnable() 
    {
        inputField.onValueChanged.AddListener(ValidateField);
        inputField.onValueChanged.AddListener(FormatTextPosition);
    }

    private void OnDisable() 
    {
        inputField.onValueChanged.RemoveListener(ValidateField);
        inputField.onValueChanged.RemoveListener(FormatTextPosition);
    }

    private void FormatTextPosition(string value)
    {
        inputField.caretPosition = inputField.text.Length;
    }

}
