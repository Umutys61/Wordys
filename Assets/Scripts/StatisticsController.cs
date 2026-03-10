using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StatisticsController : MonoBehaviour
{
    [SerializeField] private GameObject statsPanel;
    [SerializeField] private GameManager gameManager;
    [SerializeField] private Transform statsContent;
    [SerializeField] private GameObject statRowPrefab;

    public void ShowStats()
    {
        statsPanel.SetActive(true);
        UpdateStatsUI();
    }

    public void HideStats()
    {
        statsPanel.SetActive(false);
    }

    private void UpdateStatsUI()
    {
        foreach (Transform child in statsContent)
        {
            Destroy(child.gameObject);
        }

        int[] lengths = { 4, 5, 6, 7 };
        foreach (int len in lengths)
        {
            var (level, played) = gameManager.GetStats(len);
            CreateStatRow(len, level, played);
        }
    }

    private void CreateStatRow(int length, int level, int played)
    {
        GameObject row = new GameObject($"Row_{length}");
        row.transform.SetParent(statsContent, false);
        
        HorizontalLayoutGroup hlg = row.AddComponent<HorizontalLayoutGroup>();
        hlg.childAlignment = TextAnchor.MiddleLeft;
        hlg.childControlWidth = true;
        hlg.childControlHeight = true;
        hlg.padding = new RectOffset(20, 20, 0, 0);
        
        LayoutElement le = row.AddComponent<LayoutElement>();
        le.minHeight = 60;
        le.preferredHeight = 60;

        TextMeshProUGUI tmp = new GameObject("Text").AddComponent<TextMeshProUGUI>();
        tmp.transform.SetParent(row.transform, false);
        tmp.text = $"{length} Harf:  Seviye {level}  |  Oynanan {played}";
        tmp.fontSize = 28;
        tmp.color = Color.white;
        tmp.alignment = TextAlignmentOptions.Left;
    }
}
