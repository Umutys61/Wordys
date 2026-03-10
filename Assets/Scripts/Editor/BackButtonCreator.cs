using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEditor;

public class BackButtonCreator : MonoBehaviour
{
    public static void Execute()
    {
        GameObject gamePanel = GameObject.Find("Canvas/GamePanel");
        if (!gamePanel)
        {
            Debug.LogError("GamePanel not found!");
            return;
        }

        if (gamePanel.transform.Find("BackBtn"))
        {
            Debug.Log("Back Button already exists.");
            return;
        }

        GameObject btnObj = new GameObject("BackBtn");
        btnObj.transform.SetParent(gamePanel.transform, false);
        
        Image img = btnObj.AddComponent<Image>();
        img.sprite = AssetDatabase.LoadAssetAtPath<Sprite>("Assets/Textures/RoundedSquare.png");
        if (!img.sprite) img.color = new Color(0.2f, 0.2f, 0.2f, 0.8f);
        else img.color = new Color(0.2f, 0.2f, 0.2f, 1f);

        Button btn = btnObj.AddComponent<Button>();
        
        RectTransform rt = btnObj.GetComponent<RectTransform>();
        rt.anchorMin = new Vector2(0, 1);
        rt.anchorMax = new Vector2(0, 1);
        rt.pivot = new Vector2(0, 1);
        rt.sizeDelta = new Vector2(60, 60);
        rt.anchoredPosition = new Vector2(20, -20);

        GameObject textObj = new GameObject("Icon");
        textObj.transform.SetParent(btnObj.transform, false);
        
        TextMeshProUGUI tmp = textObj.AddComponent<TextMeshProUGUI>();
        tmp.text = "<";
        tmp.fontSize = 40;
        tmp.alignment = TextAlignmentOptions.Center;
        tmp.color = Color.white;
        tmp.fontStyle = FontStyles.Bold;
        
        RectTransform textRT = textObj.GetComponent<RectTransform>();
        textRT.anchorMin = Vector2.zero;
        textRT.anchorMax = Vector2.one;
        textRT.offsetMin = Vector2.zero;
        textRT.offsetMax = Vector2.zero;

        GameManager gm = GameObject.FindObjectOfType<GameManager>();
        if (gm)
        {
            #if UNITY_EDITOR
            UnityEditor.Events.UnityEventTools.AddPersistentListener(btn.onClick, gm.ReturnToMenu);
            #endif
        }
        else
        {
            Debug.LogError("GameManager not found! Cannot link button.");
        }

        Debug.Log("Back Button Created!");
    }
}
