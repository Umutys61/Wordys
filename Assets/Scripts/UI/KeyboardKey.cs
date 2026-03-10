using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class KeyboardKey : MonoBehaviour
{
    [SerializeField] private TMP_Text keyText;
    [SerializeField] private KeyboardManager keyboardManager;
    [SerializeField] private Button button;

    [Header("Colors")]
    [SerializeField] private Color defaultColor = new Color(0.506f, 0.514f, 0.518f);
    [SerializeField] private Color correctColor = new Color(0.325f, 0.553f, 0.306f);
    [SerializeField] private Color wrongPlaceColor = new Color(0.71f, 0.624f, 0.231f);
    [SerializeField] private Color notInWordColor = new Color(0.227f, 0.227f, 0.235f);
    [SerializeField] private Color disabledColor = new Color(0.15f, 0.15f, 0.15f, 0.8f);

    private char keyChar;
    private LetterState currentState = LetterState.NotInWord;

    public char KeyChar => keyChar;

    private void Awake()
    {
        keyChar = keyText.text[0];
        ResetState();
    }

    public void OnClick()
    {
        keyboardManager.PressLetter(keyChar);
    }

    public void SetState(LetterState newState)
    {

        if (newState < currentState)
            return;

        currentState = newState;
        button.image.color = GetColor(newState);
    }

    public void ResetState()
    {
        currentState = LetterState.NotInWord;
        button.image.color = defaultColor;
        button.interactable = true;
        keyText.color = Color.white;
    }

    public void DisableKey()
    {
        button.interactable = false;
        button.image.color = disabledColor;
        keyText.color = new Color(0.85f, 0.85f, 0.85f);
    }

    private Color GetColor(LetterState state)
    {
        return state switch
        {
            LetterState.Correct => correctColor,
            LetterState.WrongPlace => wrongPlaceColor,
            LetterState.NotInWord => notInWordColor,
            _ => defaultColor
        };
    }
}
