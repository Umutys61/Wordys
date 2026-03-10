using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEditor;

public class StatsPanelFixer : MonoBehaviour
{
    public static void Execute()
    {
        GameObject gameManagerObj = GameObject.Find("GameManager");
        if (gameManagerObj)
        {
            if (!gameManagerObj.GetComponent<StatsManager>())
            {
                gameManagerObj.AddComponent<StatsManager>();
                Debug.Log("StatsManager added to GameManager.");
            }
        }

        GameObject canvas = GameObject.Find("Canvas");
        GameObject statsPanel = GameObject.Find("Canvas/StatsPanel");
        
        if (!statsPanel)
        {
            statsPanel = new GameObject("StatsPanel");
            statsPanel.transform.SetParent(canvas.transform, false);
        }

        RectTransform rt = statsPanel.GetComponent<RectTransform>();
        if (!rt) rt = statsPanel.AddComponent<RectTransform>();
        rt.anchorMin = Vector2.zero;
        rt.anchorMax = Vector2.one;
        rt.offsetMin = Vector2.zero;
        rt.offsetMax = Vector2.zero;

        Image img = statsPanel.GetComponent<Image>();
        if (!img) img = statsPanel.AddComponent<Image>();
        img.color = new Color(0.07f, 0.07f, 0.075f, 1f);

        StatsPanelUI ui = statsPanel.GetComponent<StatsPanelUI>();
        if (!ui) ui = statsPanel.AddComponent<StatsPanelUI>();

        while (statsPanel.transform.childCount > 0)
        {
            DestroyImmediate(statsPanel.transform.GetChild(0).gameObject);
        }

        GameObject header = new GameObject("Header");
        header.transform.SetParent(statsPanel.transform, false);
        TextMeshProUGUI headerText = header.AddComponent<TextMeshProUGUI>();
        headerText.text = "İSTATİSTİKLER";
        headerText.fontSize = 60;
        headerText.alignment = TextAlignmentOptions.Center;
        headerText.color = Color.white;
        headerText.fontStyle = FontStyles.Bold;
        
        RectTransform hrt = header.GetComponent<RectTransform>();
        hrt.anchorMin = new Vector2(0, 1);
        hrt.anchorMax = new Vector2(1, 1);
        hrt.sizeDelta = new Vector2(0, 150);
        hrt.anchoredPosition = new Vector2(0, -75);

        GameObject content = new GameObject("Content");
        content.transform.SetParent(statsPanel.transform, false);
        RectTransform crt = content.AddComponent<RectTransform>();
        crt.anchorMin = new Vector2(0.1f, 0.15f);
        crt.anchorMax = new Vector2(0.9f, 0.85f);
        crt.offsetMin = Vector2.zero;
        crt.offsetMax = Vector2.zero;

        VerticalLayoutGroup cvlg = content.AddComponent<VerticalLayoutGroup>();
        cvlg.spacing = 40;
        cvlg.childAlignment = TextAnchor.UpperCenter;
        cvlg.childControlWidth = true;
        cvlg.childControlHeight = false;

        GameObject statsRow = new GameObject("StatsRow");
        statsRow.transform.SetParent(content.transform, false);
        HorizontalLayoutGroup hlg = statsRow.AddComponent<HorizontalLayoutGroup>();
        hlg.childAlignment = TextAnchor.MiddleCenter;
        hlg.spacing = 50;
        hlg.childControlWidth = false;
        hlg.childControlHeight = false;

        TextMeshProUGUI totalGamesText = CreateStatItem(statsRow.transform, "Oyun", "0");
        TextMeshProUGUI totalWinsText = CreateStatItem(statsRow.transform, "Galibiyet", "0");
        TextMeshProUGUI winPercentText = CreateStatItem(statsRow.transform, "Kazanma %", "0%");
        TextMeshProUGUI currentStreakText = CreateStatItem(statsRow.transform, "Seri", "0");
        TextMeshProUGUI bestStreakText = CreateStatItem(statsRow.transform, "En İyi Seri", "0");

        ui.GetType().GetField("totalGamesText", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).SetValue(ui, totalGamesText);
        ui.GetType().GetField("totalWinsText", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).SetValue(ui, totalWinsText);
        ui.GetType().GetField("winPercentageText", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).SetValue(ui, winPercentText);
        ui.GetType().GetField("currentStreakText", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).SetValue(ui, currentStreakText);
        ui.GetType().GetField("bestStreakText", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).SetValue(ui, bestStreakText);

        GameObject graphArea = new GameObject("GraphArea");
        graphArea.transform.SetParent(content.transform, false);
        VerticalLayoutGroup gvlg = graphArea.AddComponent<VerticalLayoutGroup>();
        gvlg.spacing = 10;
        gvlg.childAlignment = TextAnchor.UpperCenter;
        
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

        GameObject closeBtnObj = new GameObject("CloseBtn");
        closeBtnObj.transform.SetParent(statsPanel.transform, false);
        
        Image closeImg = closeBtnObj.AddComponent<Image>();
        closeImg.sprite = AssetDatabase.LoadAssetAtPath<Sprite>("Assets/Textures/RoundedSquare.png");
        closeImg.type = Image.Type.Sliced;
        closeImg.color = new Color(0.7f, 0.2f, 0.2f);

        Button closeBtn = closeBtnObj.AddComponent<Button>();
        
        RectTransform cbrt = closeBtnObj.GetComponent<RectTransform>();
        cbrt.anchorMin = new Vector2(0.5f, 0.1f);
        cbrt.anchorMax = new Vector2(0.5f, 0.1f);
        cbrt.sizeDelta = new Vector2(200, 60);
        cbrt.anchoredPosition = Vector2.zero;

        GameObject closeTextObj = new GameObject("Text");
        closeTextObj.transform.SetParent(closeBtnObj.transform, false);
        TextMeshProUGUI closeTmp = closeTextObj.AddComponent<TextMeshProUGUI>();
        closeTmp.text = "KAPAT";
        closeTmp.fontSize = 30;
        closeTmp.alignment = TextAlignmentOptions.Center;
        closeTmp.color = Color.white;
        
        RectTransform ctrt = closeTextObj.GetComponent<RectTransform>();
        ctrt.anchorMin = Vector2.zero;
        ctrt.anchorMax = Vector2.one;
        ctrt.offsetMin = Vector2.zero;
        ctrt.offsetMax = Vector2.zero;

        #if UNITY_EDITOR
        UnityEditor.Events.UnityEventTools.AddBoolPersistentListener(closeBtn.onClick, statsPanel.SetActive, false);
        #endif

        GameObject statsBtn = GameObject.Find("Canvas/MainMenuPanel/StatsBtn");
        if (statsBtn)
        {
            Button btn = statsBtn.GetComponent<Button>();
            #if UNITY_EDITOR
            UnityEditor.Events.UnityEventTools.AddBoolPersistentListener(btn.onClick, statsPanel.SetActive, true);
            #endif
        }

        statsPanel.SetActive(false);

        Debug.Log("Stats Panel Fixed and Linked!");
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
