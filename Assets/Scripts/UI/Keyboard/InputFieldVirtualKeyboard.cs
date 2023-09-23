using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InputFieldVirtualKeyboard : MonoBehaviour, IPointerClickHandler
{
    public enum KeyboardType
    {
        Full,
        AlphaNumeric,
        Numeric
    }

    public KeyboardType selectedKeyboardType;

    public void OnPointerClick(PointerEventData eventData)
    {
        KeyboardManager.Instance.SetTextBox(gameObject.GetComponent<InputField>());

        switch (selectedKeyboardType)
        {
            case KeyboardType.Full:
                KeyboardManager.Instance.activeFullKeyboard();
                break;

            case KeyboardType.AlphaNumeric:
                KeyboardManager.Instance.activeAlphaNUmericKeyboard();
                break;

            case KeyboardType.Numeric:
                KeyboardManager.Instance.activeNumericKeyboard();
                break;

            default:
                KeyboardManager.Instance.activeFullKeyboard();
                break;
        }
    }
}
