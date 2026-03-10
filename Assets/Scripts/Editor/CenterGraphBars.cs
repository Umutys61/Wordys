using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEditor;

public class CenterGraphBars : MonoBehaviour
{
    public static void Execute()
    {
        GameObject barsContainer = GameObject.Find("Canvas/StatsPanel/Content/GraphArea/Bars");
        if (!barsContainer)
        {
            Debug.LogError("Bars container not found!");
            return;
        }

        HorizontalLayoutGroup hlg = barsContainer.GetComponent<HorizontalLayoutGroup>();
        if (hlg)
        {
            hlg.childAlignment = TextAnchor.LowerCenter;
            hlg.childControlWidth = false;
            hlg.childControlHeight = false;
            hlg.childForceExpandWidth = false;
            hlg.childForceExpandHeight = false;
            hlg.spacing = 25;
        }

        foreach (Transform barGroup in barsContainer.transform)
        {
            VerticalLayoutGroup vlg = barGroup.GetComponent<VerticalLayoutGroup>();
            if (vlg)
            {
                vlg.childAlignment = TextAnchor.LowerCenter;
                vlg.childControlWidth = false;
                vlg.childControlHeight = false;
                vlg.childForceExpandWidth = false;
                vlg.childForceExpandHeight = false;
            }

            Transform label = barGroup.Find("Label");
            if (label)
            {
                TextMeshProUGUI txt = label.GetComponent<TextMeshProUGUI>();
                if (txt) txt.alignment = TextAlignmentOptions.Center;
            }

            Transform count = barGroup.Find("Count");
            if (count)
            {
                TextMeshProUGUI txt = count.GetComponent<TextMeshProUGUI>();
                if (txt) txt.alignment = TextAlignmentOptions.Center;
            }
        }

        Debug.Log("Graph Bars Centered!");
    }
}
