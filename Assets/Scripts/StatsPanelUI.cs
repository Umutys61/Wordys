using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;
using System.Collections;

public class StatsPanelUI : MonoBehaviour
{
    [Header("Text References")]
    [SerializeField] private TextMeshProUGUI totalGamesText;
    [SerializeField] private TextMeshProUGUI totalWinsText;
    [SerializeField] private TextMeshProUGUI winPercentageText;
    [SerializeField] private TextMeshProUGUI currentStreakText;
    [SerializeField] private TextMeshProUGUI bestStreakText;

    [Header("Graph References")]
    [SerializeField] private RectTransform[] guessBars;
    [SerializeField] private TextMeshProUGUI[] guessCountTexts;

    [Header("Settings")]
    [SerializeField] private float maxBarHeight = 300f;
    [SerializeField] private Color defaultBarColor = new Color(0.5f, 0.5f, 0.5f);
    [SerializeField] private Color highlightBarColor = new Color(0.3f, 0.8f, 0.3f);
    [SerializeField] private float animationDuration = 0.5f;

    private void OnEnable()
    {
        transform.localScale = Vector3.zero;
        StartCoroutine(AnimatePop());
        
        RefreshUI();
    }

    private IEnumerator AnimatePop()
    {
        float t = 0;
        while(t < 1)
        {
            t += Time.deltaTime * 5f;
            transform.localScale = Vector3.Lerp(Vector3.zero, Vector3.one, t);
            yield return null;
        }
        transform.localScale = Vector3.one;
    }

    public void RefreshUI()
    {
        if (StatsManager.Instance == null) return;

        GameStats stats = StatsManager.Instance.CurrentStats;

        if (totalGamesText) totalGamesText.text = stats.totalGames.ToString();
        if (totalWinsText) totalWinsText.text = stats.totalWins.ToString();
        
        if (winPercentageText)
        {
            float winRate = stats.GetWinPercentage();
            winPercentageText.text = $"{winRate:F0}%";
        }

        if (currentStreakText) currentStreakText.text = stats.currentStreak.ToString();
        if (bestStreakText) bestStreakText.text = stats.bestStreak.ToString();

        StartCoroutine(AnimateGraph(stats.guessDistribution));
    }

    private IEnumerator AnimateGraph(int[] distribution)
    {
        if (guessBars == null || guessBars.Length == 0) yield break;

        int maxVal = distribution.Max();
        if (maxVal == 0) maxVal = 1;

        foreach(var bar in guessBars)
        {
            if(bar) bar.sizeDelta = new Vector2(bar.sizeDelta.x, 0);
        }

        float t = 0;
        while (t < 1f)
        {
            t += Time.deltaTime / animationDuration;
            float curve = Mathf.SmoothStep(0, 1, t);

            for (int i = 0; i < guessBars.Length; i++)
            {
                if (i >= distribution.Length) break;
                if (guessBars[i] == null) continue;

                int count = distribution[i];
                float targetHeight = ((float)count / maxVal) * maxBarHeight;
                if (count > 0 && targetHeight < 20f) targetHeight = 20f;

                Vector2 size = guessBars[i].sizeDelta;
                size.y = Mathf.Lerp(0, targetHeight, curve);
                guessBars[i].sizeDelta = size;

                Image barImg = guessBars[i].GetComponent<Image>();
                if (barImg)
                {
                    bool isMax = (count == maxVal && count > 0);
                    barImg.color = isMax ? highlightBarColor : defaultBarColor;
                }
                
                if (guessCountTexts != null && i < guessCountTexts.Length && guessCountTexts[i] != null)
                {
                    guessCountTexts[i].text = count.ToString();
                    guessCountTexts[i].gameObject.SetActive(count > 0);
                }
            }
            yield return null;
        }
    }
}
