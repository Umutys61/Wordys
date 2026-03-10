using UnityEngine;
using TMPro;

public class RenameStatsTitle : MonoBehaviour
{
    public static void Execute()
    {
        GameObject titleObj = GameObject.Find("Canvas/StatsPanel/Content/GraphArea/Title");
        if (titleObj)
        {
            TextMeshProUGUI title = titleObj.GetComponent<TextMeshProUGUI>();
            if (title)
            {
                title.text = "KAÇINCI DENEMEDE BİLDİN?";
                title.fontSize = 24;
                Debug.Log("Renamed Stats Title.");
            }
        }
        else
        {
            Debug.LogError("Title object not found.");
        }
    }
}
