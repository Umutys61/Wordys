using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEditor;

public class CenterGraphHorizontal : MonoBehaviour
{
    public static void Execute()
    {
        GameObject graphArea = GameObject.Find("Canvas/StatsPanel/Content/GraphArea");
        if (!graphArea)
        {
            Debug.LogError("GraphArea not found!");
            return;
        }

        LayoutElement le = graphArea.GetComponent<LayoutElement>();
        if (!le) le = graphArea.AddComponent<LayoutElement>();
        le.flexibleWidth = 1;

        VerticalLayoutGroup vlg = graphArea.GetComponent<VerticalLayoutGroup>();
        if (vlg)
        {
            vlg.childAlignment = TextAnchor.UpperCenter;
        }

        Transform bars = graphArea.transform.Find("Bars");
        if (bars)
        {
            RectTransform barsRT = bars.GetComponent<RectTransform>();
            
            HorizontalLayoutGroup barsHLG = bars.GetComponent<HorizontalLayoutGroup>();
            if (barsHLG)
            {
                barsHLG.childAlignment = TextAnchor.LowerCenter;
            }
        }

        Transform title = graphArea.transform.Find("Title");
        if (title)
        {
            TextMeshProUGUI txt = title.GetComponent<TextMeshProUGUI>();
            if (txt) txt.alignment = TextAlignmentOptions.Center;
        }

        Debug.Log("Graph Area Centered Horizontally!");
    }
}
