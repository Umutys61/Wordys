using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LetterCell : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private TMP_Text letterText;
    [SerializeField] private Image backgroundImage;

    [Header("Colors")]
    [SerializeField] private Color normalColor = new Color(0.07f, 0.07f, 0.075f);
    [SerializeField] private Color correctColor = new Color(0.325f, 0.553f, 0.306f);
    [SerializeField] private Color wrongPlaceColor = new Color(0.71f, 0.624f, 0.231f);
    [SerializeField] private Color notInWordColor = new Color(0.227f, 0.227f, 0.235f);

    [Header("Border Colors")]
    [SerializeField] private Color emptyBorderColor = new Color(0.227f, 0.227f, 0.235f);
    [SerializeField] private Color filledBorderColor = new Color(0.337f, 0.341f, 0.345f);

    public char Letter { get; private set; }

    private RectTransform rectTransform;
    private Outline outline;
    private Coroutine shakeRoutine;

    private bool locked;
    public bool IsLocked => locked;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        outline = GetComponent<Outline>();

        if (outline == null)
        {
            outline = gameObject.AddComponent<Outline>();
            outline.effectDistance = new Vector2(2, -2);
        }

        Clear();
    }

    public void SetLetter(char letter)
    {
        if (locked) return;

        Letter = letter.ToString()
            .ToUpper(new System.Globalization.CultureInfo("tr-TR"))[0];

        letterText.text = Letter.ToString();

        backgroundImage.color = normalColor;

        outline.enabled = true;
        outline.effectColor = filledBorderColor;
    }

    public void SetLetterForced(char letter)
    {
        Letter = letter.ToString()
            .ToUpper(new System.Globalization.CultureInfo("tr-TR"))[0];

        letterText.text = Letter.ToString();
    }

    public void LockCell()
    {
        locked = true;
    }

    public bool IsEmpty()
    {
        return Letter == '\0';
    }

    public void Clear()
    {
        Letter = '\0';
        letterText.text = string.Empty;
        backgroundImage.color = normalColor;

        locked = false;

        if (outline != null)
        {
            outline.enabled = true;
            outline.effectColor = emptyBorderColor;
        }
    }

    public void SetState(LetterState state)
    {
        backgroundImage.color = state switch
        {
            LetterState.Correct => correctColor,
            LetterState.WrongPlace => wrongPlaceColor,
            LetterState.NotInWord => notInWordColor,
            _ => normalColor
        };

        if (outline != null)
        {
            outline.enabled = false;
        }
    }

    public void Shake(float strength = 10f, float duration = 0.15f)
    {
        if (shakeRoutine != null)
            StopCoroutine(shakeRoutine);

        shakeRoutine = StartCoroutine(ShakeRoutine(strength, duration));
    }

    private IEnumerator ShakeRoutine(float strength, float duration)
    {
        Vector2 startPos = rectTransform.anchoredPosition;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            float offset = Random.Range(-1f, 1f) * strength;
            rectTransform.anchoredPosition = startPos + Vector2.right * offset;

            elapsed += Time.deltaTime;
            yield return null;
        }

        rectTransform.anchoredPosition = startPos;
    }
}

public enum LetterState
{
    None = 0,
    NotInWord = 1,
    WrongPlace = 2,
    Correct = 3
}
