using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEditor;

public class StatsButtonCreator : MonoBehaviour
{
    public static void Execute()
    {
        GameObject mainMenu = GameObject.Find("Canvas/MainMenuPanel");
        if (!mainMenu)
        {
            Debug.LogError("MainMenuPanel not found!");
            return;
        }

        Transform existingBtn = mainMenu.transform.Find("StatsBtn");
        if (existingBtn)
        {
            Debug.Log("Stats Button already exists.");
            return;
        }

        GameObject btnObj = new GameObject("StatsBtn");
        btnObj.transform.SetParent(mainMenu.transform, false);
        
        Image img = btnObj.AddComponent<Image>();
        img.sprite = AssetDatabase.LoadAssetAtPath<Sprite>("Assets/Textures/RoundedSquare.png");
        img.type = Image.Type.Sliced;
        img.color = new Color(0.2f, 0.4f, 0.6f);

        Button btn = btnObj.AddComponent<Button>();
        
        RectTransform rt = btnObj.GetComponent<RectTransform>();
        rt.anchorMin = new Vector2(0.5f, 0.2f);
        rt.anchorMax = new Vector2(0.5f, 0.2f);
        rt.sizeDelta = new Vector2(200, 60);
        rt.anchoredPosition = new Vector2(0, 50);

        GameObject textObj = new GameObject("Text");
        textObj.transform.SetParent(btnObj.transform, false);
        
        TextMeshProUGUI tmp = textObj.AddComponent<TextMeshProUGUI>();
        tmp.text = "İSTATİSTİKLER";
        tmp.fontSize = 24;
        tmp.alignment = TextAlignmentOptions.Center;
        tmp.color = Color.white;
        tmp.fontStyle = FontStyles.Bold;
        
        RectTransform textRT = textObj.GetComponent<RectTransform>();
        textRT.anchorMin = Vector2.zero;
        textRT.anchorMax = Vector2.one;
        textRT.offsetMin = Vector2.zero;
        textRT.offsetMax = Vector2.zero;

        GameObject statsPanel = GameObject.Find("Canvas/StatsPanel");
        if (statsPanel)
        {
            #if UNITY_EDITOR
            UnityEditor.Events.UnityEventTools.AddBoolPersistentListener(btn.onClick, statsPanel.SetActive, true);
            #endif
        }
        else
        {
            Debug.LogError("StatsPanel not found! Cannot link button.");
        }

        Debug.Log("Stats Button Created!");
    }
}
