using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEditor;

public class StatsPanelRedesign : MonoBehaviour
{
    public static void Execute()
    {
        GameObject statsPanel = GameObject.Find("Canvas/StatsPanel");
        if (!statsPanel)
        {
            Debug.LogError("StatsPanel not found!");
            return;
        }

        Image bg = statsPanel.GetComponent<Image>();
        if (bg)
        {
            bg.color = new Color(0.15f, 0.15f, 0.18f, 0.98f);
        }
        
        if (!statsPanel.GetComponent<Shadow>())
        {
            Shadow shadow = statsPanel.AddComponent<Shadow>();
            shadow.effectColor = new Color(0, 0, 0, 0.5f);
            shadow.effectDistance = new Vector2(5, -5);
        }

        Transform header = statsPanel.transform.Find("Header");
        if (header)
        {
            TextMeshProUGUI headerText = header.GetComponent<TextMeshProUGUI>();
            headerText.fontSize = 50;
            headerText.color = new Color(1f, 0.85f, 0.3f);
            headerText.fontStyle = FontStyles.Bold;
            headerText.characterSpacing = 5;
        }

        Transform content = statsPanel.transform.Find("Content");
        if (content)
        {
            VerticalLayoutGroup vlg = content.GetComponent<VerticalLayoutGroup>();
            vlg.padding = new RectOffset(50, 50, 30, 30);
            vlg.spacing = 60;

            Transform statsRow = content.Find("StatsRow");
            if (statsRow)
            {
                HorizontalLayoutGroup hlg = statsRow.GetComponent<HorizontalLayoutGroup>();
                hlg.spacing = 40;
                hlg.childAlignment = TextAnchor.MiddleCenter;

                foreach (Transform child in statsRow)
                {
                    TextMeshProUGUI valText = child.Find("Value")?.GetComponent<TextMeshProUGUI>();
                    if (valText)
                    {
                        valText.fontSize = 48;
                        valText.fontStyle = FontStyles.Bold;
                        valText.color = Color.white;
                    }

                    TextMeshProUGUI lblText = child.Find("Label")?.GetComponent<TextMeshProUGUI>();
                    if (lblText)
                    {
                        lblText.fontSize = 14;
                        lblText.color = new Color(0.7f, 0.7f, 0.7f);
                        lblText.fontStyle = FontStyles.UpperCase;
                        lblText.characterSpacing = 2;
                    }
                }
            }

            Transform graphArea = content.Find("GraphArea");
            if (graphArea)
            {
                TextMeshProUGUI title = graphArea.Find("Title")?.GetComponent<TextMeshProUGUI>();
                if (title)
                {
                    title.fontSize = 22;
                    title.color = Color.white;
                    title.alignment = TextAlignmentOptions.Center;
                    title.fontStyle = FontStyles.Bold;
                    title.text = "GUESS DISTRIBUTION";
                    title.text = "TAHMİN DAĞILIMI";
                }

                Transform bars = graphArea.Find("Bars");
                if (bars)
                {
                    RectTransform barsRT = bars.GetComponent<RectTransform>();
                    barsRT.sizeDelta = new Vector2(600, 300);

                    HorizontalLayoutGroup barsHLG = bars.GetComponent<HorizontalLayoutGroup>();
                    barsHLG.spacing = 15;

                    foreach (Transform barGroup in bars)
                    {
                        RectTransform barGroupRT = barGroup.GetComponent<RectTransform>();

                        Transform barObj = barGroup.Find("Bar");
                        if (barObj)
                        {
                            RectTransform barRT = barObj.GetComponent<RectTransform>();
                            barRT.sizeDelta = new Vector2(50, 0);
                            
                            Image barImg = barObj.GetComponent<Image>();
                        }

                        TextMeshProUGUI count = barGroup.Find("Count")?.GetComponent<TextMeshProUGUI>();
                        if (count)
                        {
                            count.fontSize = 20;
                            count.fontStyle = FontStyles.Bold;
                            count.color = Color.white;
                            count.gameObject.SetActive(true);
                        }

                        TextMeshProUGUI label = barGroup.Find("Label")?.GetComponent<TextMeshProUGUI>();
                        if (label)
                        {
                            label.fontSize = 20;
                            label.fontStyle = FontStyles.Bold;
                            label.color = Color.white;
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
            
            RectTransform btnRT = closeBtn.GetComponent<RectTransform>();
            btnRT.sizeDelta = new Vector2(250, 70);

            TextMeshProUGUI btnText = closeBtn.Find("Text")?.GetComponent<TextMeshProUGUI>();
            if (btnText)
            {
                btnText.fontSize = 28;
                btnText.fontStyle = FontStyles.Bold;
                btnText.characterSpacing = 3;
            }
        }

        Debug.Log("Stats Panel Redesigned!");
    }
}
