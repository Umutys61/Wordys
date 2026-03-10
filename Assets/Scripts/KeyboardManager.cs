using System.Collections.Generic;
using UnityEngine;

public class KeyboardManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GridManager gridManager;
    [SerializeField] private KeyboardKey[] letterKeys;

    private static readonly HashSet<char> TurkishLetters = new HashSet<char>
    {
        'A','B','C','Ç','D','E','F','G','Ğ','H','I','İ',
        'J','K','L','M','N','O','Ö','P','R','S','Ş',
        'T','U','Ü','V','Y','Z'
    };

    private void Start()
    {
        DisableNonTurkishKeys();
    }

    #region Input Forwarding

    public void PressLetter(char letter)
    {
        if (gridManager == null)
            return;

        gridManager.AddLetter(letter);
    }

    public void PressEnter()
    {
        if (gridManager == null)
            return;

        gridManager.SubmitRow();
    }

    public void PressBackspace()
    {
        if (gridManager == null)
            return;

        gridManager.RemoveLetter();
    }

    #endregion

    #region Keyboard State

    private void DisableNonTurkishKeys()
    {
        if (letterKeys == null || letterKeys.Length == 0)
            return;

        foreach (var key in letterKeys)
        {
            if (key == null)
                continue;

            if (!TurkishLetters.Contains(key.KeyChar))
            {
                key.DisableKey();
            }
        }
    }

    public void UpdateKeyStates(string guess, LetterState[] states)
    {
        if (letterKeys == null || letterKeys.Length == 0)
            return;

        for (int i = 0; i < guess.Length; i++)
        {
            char c = guess[i];

            foreach (var key in letterKeys)
            {
                if (key != null && key.KeyChar == c)
                {
                    key.SetState(states[i]);
                    break;
                }
            }
        }
    }
    public void ResetKeyboard()
    {
        foreach (var key in letterKeys)
        {
            if (key == null) continue;

            char c = key.KeyChar;
            if (c == 'Q' || c == 'W' || c == 'X' || !TurkishLetters.Contains(c))
            {
                key.DisableKey();
            }
            else
            {
                key.ResetState();
            }
        }
    }


    #endregion
}
