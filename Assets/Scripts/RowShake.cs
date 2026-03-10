using System.Collections;
using UnityEngine;

public class RowShake : MonoBehaviour
{
    [SerializeField] private float duration = 0.25f;
    [SerializeField] private float strength = 15f;

    private RectTransform rectTransform;
    private Vector2 originalPos;
    private Coroutine shakeRoutine;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    public void Shake()
    {
        originalPos = rectTransform.anchoredPosition;

        if (shakeRoutine != null)
            StopCoroutine(shakeRoutine);

        shakeRoutine = StartCoroutine(ShakeRoutine());
    }

    private IEnumerator ShakeRoutine()
    {
        float elapsed = 0f;

        while (elapsed < duration)
        {
            float offset = Random.Range(-1f, 1f) * strength;
            rectTransform.anchoredPosition = originalPos + Vector2.right * offset;

            elapsed += Time.deltaTime;
            yield return null;
        }

        rectTransform.anchoredPosition = originalPos;
    }
}
