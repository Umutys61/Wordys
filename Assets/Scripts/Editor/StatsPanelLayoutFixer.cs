using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEditor;

public class StatsPanelLayoutFixer : MonoBehaviour
{
    public static void Execute()
    {
        GameObject statsPanel = GameObject.Find("Canvas/StatsPanel");
        if (!statsPanel)
        {
            Debug.LogError("StatsPanel not found!");
            return;
        }

        Transform content = statsPanel.transform.Find("Content");
        if (content)
        {
            RectTransform contentRT = content.GetComponent<RectTransform>();
            contentRT.anchorMin = new Vector2(0.05f, 0.15f);
            contentRT.anchorMax = new Vector2(0.95f, 0.85f);
            contentRT.offsetMin = Vector2.zero;
            contentRT.offsetMax = Vector2.zero;

            VerticalLayoutGroup vlg = content.GetComponent<VerticalLayoutGroup>();
            if (vlg)
            {
                vlg.spacing = 30;
                vlg.padding = new RectOffset(20, 20, 10, 10);
                vlg.childAlignment = TextAnchor.UpperCenter;
                vlg.childControlHeight = false;
                vlg.childControlWidth = true;
                vlg.childForceExpandHeight = false;
            }
        }

        Transform statsRow = content.Find("StatsRow");
        if (statsRow)
        {
            LayoutElement le = statsRow.GetComponent<LayoutElement>();
            if (!le) le = statsRow.gameObject.AddComponent<LayoutElement>();
            le.minHeight = 100;
            le.preferredHeight = 120;
            le.flexibleHeight = 0;
        }

        Transform graphArea = content.Find("GraphArea");
        if (graphArea)
        {
            LayoutElement le = graphArea.GetComponent<LayoutElement>();
            if (!le) le = graphArea.gameObject.AddComponent<LayoutElement>();
            le.flexibleHeight = 1;
            
            VerticalLayoutGroup gvlg = graphArea.GetComponent<VerticalLayoutGroup>();
            if (gvlg)
            {
                gvlg.childAlignment = TextAnchor.MiddleCenter;
                gvlg.spacing = 10;
                gvlg.childControlHeight = false;
                gvlg.childControlWidth = false;
                gvlg.childForceExpandHeight = false;
            }

            Transform bars = graphArea.Find("Bars");
            if (bars)
            {
                RectTransform barsRT = bars.GetComponent<RectTransform>();
                barsRT.sizeDelta = new Vector2(600, 350);

                HorizontalLayoutGroup barsHLG = bars.GetComponent<HorizontalLayoutGroup>();
                if (barsHLG)
                {
                    barsHLG.childAlignment = TextAnchor.LowerCenter;
                    barsHLG.spacing = 20;
                    barsHLG.childControlHeight = false;
                    barsHLG.childControlWidth = false;
                    barsHLG.childForceExpandHeight = false;
                    barsHLG.childForceExpandWidth = false;
                }
            }
        }

        Transform closeBtn = statsPanel.transform.Find("CloseBtn");
        if (closeBtn)
        {
            RectTransform btnRT = closeBtn.GetComponent<RectTransform>();
            btnRT.anchorMin = new Vector2(0.5f, 0.05f);
            btnRT.anchorMax = new Vector2(0.5f, 0.05f);
            btnRT.pivot = new Vector2(0.5f, 0);
            btnRT.anchoredPosition = new Vector2(0, 20);
        }

        Transform header = statsPanel.transform.Find("Header");
        if (header)
        {
            RectTransform headerRT = header.GetComponent<RectTransform>();
            headerRT.anchorMin = new Vector2(0, 1);
            headerRT.anchorMax = new Vector2(1, 1);
            headerRT.pivot = new Vector2(0.5f, 1);
            headerRT.anchoredPosition = new Vector2(0, -30);
            headerRT.sizeDelta = new Vector2(0, 100);
        }

        Debug.Log("Stats Panel Layout Fixed & Balanced!");
    }
}
