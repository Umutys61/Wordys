using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEditor;

public class StatsPanelBeautifier : MonoBehaviour
{
    public static void Execute()
    {
        GameObject statsPanel = GameObject.Find("Canvas/StatsPanel");
        if (!statsPanel)
        {
            Debug.LogError("StatsPanel not found! Run Fixer first.");
            return;
        }

        Image bg = statsPanel.GetComponent<Image>();
        bg.color = new Color(0.1f, 0.1f, 0.12f, 0.98f);

        Transform header = statsPanel.transform.Find("Header");
        if (header)
        {
            TextMeshProUGUI headerText = header.GetComponent<TextMeshProUGUI>();
            headerText.fontSize = 48;
            headerText.color = new Color(1f, 0.8f, 0.2f);
            headerText.fontStyle = FontStyles.Bold | FontStyles.UpperCase;
            
            GameObject separator = new GameObject("Separator");
            separator.transform.SetParent(header, false);
            Image sepImg = separator.AddComponent<Image>();
            sepImg.color = new Color(1f, 1f, 1f, 0.2f);
            RectTransform sepRT = separator.GetComponent<RectTransform>();
            sepRT.anchorMin = new Vector2(0.2f, 0);
            sepRT.anchorMax = new Vector2(0.8f, 0);
            sepRT.sizeDelta = new Vector2(0, 2);
            sepRT.anchoredPosition = new Vector2(0, -10);
        }

        Transform content = statsPanel.transform.Find("Content");
        if (content)
        {
            VerticalLayoutGroup vlg = content.GetComponent<VerticalLayoutGroup>();
            vlg.padding = new RectOffset(40, 40, 20, 20);
            vlg.spacing = 50;

            Transform statsRow = content.Find("StatsRow");
            if (statsRow)
            {
                foreach (Transform child in statsRow)
                {
                    TextMeshProUGUI valText = child.Find("Value")?.GetComponent<TextMeshProUGUI>();
                    TextMeshProUGUI lblText = child.Find("Label")?.GetComponent<TextMeshProUGUI>();

                    if (valText)
                    {
                        valText.fontSize = 42;
                        valText.color = Color.white;
                    }
                    if (lblText)
                    {
                        lblText.fontSize = 14;
                        lblText.color = new Color(0.7f, 0.7f, 0.7f);
                        lblText.fontStyle = FontStyles.UpperCase;
                    }
                }
            }

            Transform graphArea = content.Find("GraphArea");
            if (graphArea)
            {
                TextMeshProUGUI title = graphArea.Find("Title")?.GetComponent<TextMeshProUGUI>();
                if (title)
                {
                    title.fontSize = 20;
                    title.color = Color.white;
                    title.fontStyle = FontStyles.Bold;
                    title.alignment = TextAlignmentOptions.Left;
                }

                Transform bars = graphArea.Find("Bars");
                if (bars)
                {
                    foreach (Transform barGroup in bars)
                    {
                        Transform barObj = barGroup.Find("Bar");
                        if (barObj)
                        {
                            Image barImg = barObj.GetComponent<Image>();
                            barImg.color = new Color(0.3f, 0.7f, 0.4f);
                        }

                        TextMeshProUGUI count = barGroup.Find("Count")?.GetComponent<TextMeshProUGUI>();
                        if (count)
                        {
                            count.fontSize = 18;
                            count.fontStyle = FontStyles.Bold;
                        }
                    }
                }
            }
        }

        Transform closeBtn = statsPanel.transform.Find("CloseBtn");
        if (closeBtn)
        {
            Image btnImg = closeBtn.GetComponent<Image>();
            btnImg.color = new Color(0.2f, 0.6f, 0.8f);

            TextMeshProUGUI btnText = closeBtn.Find("Text")?.GetComponent<TextMeshProUGUI>();
            if (btnText)
            {
                btnText.fontSize = 24;
                btnText.fontStyle = FontStyles.Bold;
            }
        }

        Debug.Log("Stats Panel Beautified!");
    }
}
