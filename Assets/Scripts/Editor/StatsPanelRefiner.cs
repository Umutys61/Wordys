using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEditor;

public class StatsPanelRefiner : MonoBehaviour
{
    public static void Execute()
    {
        GameObject statsPanel = GameObject.Find("Canvas/StatsPanel");
        if (!statsPanel) return;

        Transform content = statsPanel.transform.Find("Content");
        if (!content) return;

        Transform statsRow = content.Find("StatsRow");
        if (!statsRow) return;

        while (statsRow.childCount > 0) DestroyImmediate(statsRow.GetChild(0).gameObject);

        TextMeshProUGUI playedText = CreateStatItem(statsRow, "OYNANAN", "0");
        TextMeshProUGUI winRateText = CreateStatItem(statsRow, "KAZANMA %", "0");
        TextMeshProUGUI currStreakText = CreateStatItem(statsRow, "MEVCUT SERİ", "0");
        TextMeshProUGUI maxStreakText = CreateStatItem(statsRow, "EN İYİ SERİ", "0");

        StatsPanelUI ui = statsPanel.GetComponent<StatsPanelUI>();
        if (ui)
        {
            ui.GetType().GetField("totalGamesText", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).SetValue(ui, playedText);
            ui.GetType().GetField("winPercentageText", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).SetValue(ui, winRateText);
            ui.GetType().GetField("currentStreakText", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).SetValue(ui, currStreakText);
            ui.GetType().GetField("bestStreakText", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).SetValue(ui, maxStreakText);
            
            ui.GetType().GetField("totalWinsText", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).SetValue(ui, null);
        }

        Debug.Log("Stats Panel Refined: Removed redundant stats, kept meaningful ones.");
    }

    private static TextMeshProUGUI CreateStatItem(Transform parent, string label, string value)
    {
        GameObject item = new GameObject(label.Replace(" ", ""));
        item.transform.SetParent(parent, false);
        VerticalLayoutGroup vlg = item.AddComponent<VerticalLayoutGroup>();
        vlg.childAlignment = TextAnchor.MiddleCenter;
        vlg.spacing = 5;

        TextMeshProUGUI valText = new GameObject("Value").AddComponent<TextMeshProUGUI>();
        valText.transform.SetParent(item.transform, false);
        valText.text = value;
        valText.fontSize = 42;
        valText.fontStyle = FontStyles.Bold;
        valText.alignment = TextAlignmentOptions.Center;
        valText.color = Color.white;

        TextMeshProUGUI lblText = new GameObject("Label").AddComponent<TextMeshProUGUI>();
        lblText.transform.SetParent(item.transform, false);
        lblText.text = label;
        lblText.fontSize = 14;
        lblText.alignment = TextAlignmentOptions.Center;
        lblText.color = new Color(0.7f, 0.7f, 0.7f);

        return valText;
    }
}
