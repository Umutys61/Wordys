using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEditor;

public class StatsPanelSetup : MonoBehaviour
{
    public static void Execute()
    {
        GameObject statsPanel = GameObject.Find("Canvas/StatsPanel");
        if (!statsPanel)
        {
            Debug.LogError("StatsPanel not found! Please run StatsUISetup first.");
            return;
        }

        StatsPanelUI ui = statsPanel.GetComponent<StatsPanelUI>();
        if (!ui) ui = statsPanel.AddComponent<StatsPanelUI>();

        Transform content = statsPanel.transform.Find("Content");
        if (!content) return;

        while (content.childCount > 0) DestroyImmediate(content.GetChild(0).gameObject);

        GameObject statsRow = new GameObject("StatsRow");
        statsRow.transform.SetParent(content, false);
        HorizontalLayoutGroup hlg = statsRow.AddComponent<HorizontalLayoutGroup>();
        hlg.childAlignment = TextAnchor.MiddleCenter;
        hlg.spacing = 40;
        hlg.childControlWidth = false;
        hlg.childControlHeight = false;

        TextMeshProUGUI totalGamesText = CreateStatItem(statsRow.transform, "Oyun", "0");
        TextMeshProUGUI winPercentText = CreateStatItem(statsRow.transform, "Kazanma %", "0%");
        TextMeshProUGUI currentStreakText = CreateStatItem(statsRow.transform, "Seri", "0");
        TextMeshProUGUI bestStreakText = CreateStatItem(statsRow.transform, "En İyi Seri", "0");

        ui.GetType().GetField("totalGamesText", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).SetValue(ui, totalGamesText);
        ui.GetType().GetField("winPercentageText", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).SetValue(ui, winPercentText);
        ui.GetType().GetField("currentStreakText", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).SetValue(ui, currentStreakText);
        ui.GetType().GetField("bestStreakText", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).SetValue(ui, bestStreakText);
        TextMeshProUGUI totalWinsText = CreateStatItem(statsRow.transform, "Galibiyet", "0");
        totalWinsText.transform.SetSiblingIndex(1);
        ui.GetType().GetField("totalWinsText", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).SetValue(ui, totalWinsText);


        GameObject graphArea = new GameObject("GraphArea");
        graphArea.transform.SetParent(content, false);
        RectTransform grt = graphArea.AddComponent<RectTransform>();
        grt.sizeDelta = new Vector2(500, 300);
        
        VerticalLayoutGroup vlg = graphArea.AddComponent<VerticalLayoutGroup>();
        vlg.spacing = 10;
        vlg.childAlignment = TextAnchor.UpperCenter;

        TextMeshProUGUI graphTitle = new GameObject("Title").AddComponent<TextMeshProUGUI>();
        graphTitle.transform.SetParent(graphArea.transform, false);
        graphTitle.text = "TAHMİN DAĞILIMI";
        graphTitle.fontSize = 24;
        graphTitle.alignment = TextAlignmentOptions.Center;
        graphTitle.color = Color.white;
        graphTitle.fontStyle = FontStyles.Bold;

        GameObject barsContainer = new GameObject("Bars");
        barsContainer.transform.SetParent(graphArea.transform, false);
        HorizontalLayoutGroup barsHLG = barsContainer.AddComponent<HorizontalLayoutGroup>();
        barsHLG.spacing = 20;
        barsHLG.childAlignment = TextAnchor.LowerCenter;
        barsHLG.childControlHeight = false;
        barsHLG.childControlWidth = false;
        
        RectTransform barsRT = barsContainer.GetComponent<RectTransform>();
        barsRT.sizeDelta = new Vector2(500, 250);

        RectTransform[] bars = new RectTransform[6];
        TextMeshProUGUI[] counts = new TextMeshProUGUI[6];

        for (int i = 0; i < 6; i++)
        {
            GameObject barGroup = new GameObject($"Bar_{i+1}");
            barGroup.transform.SetParent(barsContainer.transform, false);
            VerticalLayoutGroup barVLG = barGroup.AddComponent<VerticalLayoutGroup>();
            barVLG.childAlignment = TextAnchor.LowerCenter;
            barVLG.spacing = 5;

            TextMeshProUGUI countText = new GameObject("Count").AddComponent<TextMeshProUGUI>();
            countText.transform.SetParent(barGroup.transform, false);
            countText.text = "0";
            countText.fontSize = 20;
            countText.alignment = TextAlignmentOptions.Center;
            countText.color = Color.white;
            counts[i] = countText;

            GameObject barObj = new GameObject("Bar");
            barObj.transform.SetParent(barGroup.transform, false);
            Image barImg = barObj.AddComponent<Image>();
            barImg.color = new Color(0.3f, 0.6f, 0.3f);
            
            RectTransform barRT = barObj.GetComponent<RectTransform>();
            barRT.sizeDelta = new Vector2(40, 20);
            bars[i] = barRT;

            TextMeshProUGUI label = new GameObject("Label").AddComponent<TextMeshProUGUI>();
            label.transform.SetParent(barGroup.transform, false);
            label.text = (i + 1).ToString();
            label.fontSize = 20;
            label.alignment = TextAlignmentOptions.Center;
            label.color = Color.gray;
        }

        ui.GetType().GetField("guessBars", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).SetValue(ui, bars);
        ui.GetType().GetField("guessCountTexts", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).SetValue(ui, counts);

        Debug.Log("Stats Panel UI Setup Complete!");
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
        valText.fontSize = 36;
        valText.fontStyle = FontStyles.Bold;
        valText.alignment = TextAlignmentOptions.Center;
        valText.color = Color.white;

        TextMeshProUGUI lblText = new GameObject("Label").AddComponent<TextMeshProUGUI>();
        lblText.transform.SetParent(item.transform, false);
        lblText.text = label;
        lblText.fontSize = 18;
        lblText.alignment = TextAlignmentOptions.Center;
        lblText.color = Color.gray;

        return valText;
    }
}
