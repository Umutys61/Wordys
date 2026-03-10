using UnityEngine;
using UnityEngine.UI;

public class KeyboardSpecialKey : MonoBehaviour
{
    public enum KeyType { Enter, Backspace }

    [SerializeField] private KeyType keyType;
    [SerializeField] private KeyboardManager keyboardManager;
    [SerializeField] private Button button;

    public void OnClick()
    {
        if (keyType == KeyType.Enter)
            keyboardManager.PressEnter();
        else
            keyboardManager.PressBackspace();
    }

    public void SetInteractable(bool value)
    {
        button.interactable = value;
    }
}
