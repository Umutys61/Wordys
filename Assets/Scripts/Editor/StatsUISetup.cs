using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using TMPro;

public class StatsUISetup : MonoBehaviour
{
    public static void Execute()
    {
        GameObject canvas = GameObject.Find("Canvas");
        if (!canvas) return;

        GameObject statsPanel = GameObject.Find("Canvas/StatsPanel");
        if (!statsPanel)
        {
            statsPanel = new GameObject("StatsPanel");
            statsPanel.transform.SetParent(canvas.transform, false);
            
            RectTransform rt = statsPanel.AddComponent<RectTransform>();
            rt.anchorMin = Vector2.zero;
            rt.anchorMax = Vector2.one;
            rt.offsetMin = Vector2.zero;
            rt.offsetMax = Vector2.zero;

            Image img = statsPanel.AddComponent<Image>();
            img.color = new Color(0.07f, 0.07f, 0.075f, 1f);
        }

        Transform header = statsPanel.transform.Find("Header");
        if (!header)
        {
            GameObject h = new GameObject("Header");
            h.transform.SetParent(statsPanel.transform, false);
            header = h.transform;
            
            TextMeshProUGUI tmp = h.AddComponent<TextMeshProUGUI>();
            tmp.text = "İSTATİSTİKLER";
            tmp.fontSize = 60;
            tmp.alignment = TextAlignmentOptions.Center;
            tmp.color = Color.white;
            tmp.fontStyle = FontStyles.Bold;

            RectTransform hrt = h.GetComponent<RectTransform>();
            hrt.anchorMin = new Vector2(0, 1);
            hrt.anchorMax = new Vector2(1, 1);
            hrt.sizeDelta = new Vector2(0, 150);
            hrt.anchoredPosition = new Vector2(0, -75);
        }

        Transform content = statsPanel.transform.Find("Content");
        if (!content)
        {
            GameObject c = new GameObject("Content");
            c.transform.SetParent(statsPanel.transform, false);
            content = c.transform;
            
            RectTransform crt = c.AddComponent<RectTransform>();
            crt.anchorMin = new Vector2(0.1f, 0.2f);
            crt.anchorMax = new Vector2(0.9f, 0.8f);
            crt.offsetMin = Vector2.zero;
            crt.offsetMax = Vector2.zero;

            VerticalLayoutGroup vlg = c.AddComponent<VerticalLayoutGroup>();
            vlg.spacing = 20;
            vlg.childAlignment = TextAnchor.UpperCenter;
            vlg.childControlWidth = true;
            vlg.childControlHeight = false;
        }

        Transform closeBtn = statsPanel.transform.Find("CloseBtn");
        if (!closeBtn)
        {
            GameObject btnObj = new GameObject("CloseBtn");
            btnObj.transform.SetParent(statsPanel.transform, false);
            closeBtn = btnObj.transform;

            Image img = btnObj.AddComponent<Image>();
            img.sprite = AssetDatabase.LoadAssetAtPath<Sprite>("Assets/Textures/RoundedSquare.png");
            img.type = Image.Type.Sliced;
            img.color = new Color(0.7f, 0.2f, 0.2f);

            Button btn = btnObj.AddComponent<Button>();
            
            RectTransform brt = btnObj.GetComponent<RectTransform>();
            brt.anchorMin = new Vector2(0.5f, 0.1f);
            brt.anchorMax = new Vector2(0.5f, 0.1f);
            brt.sizeDelta = new Vector2(200, 60);
            brt.anchoredPosition = Vector2.zero;

            GameObject textObj = new GameObject("Text");
            textObj.transform.SetParent(btnObj.transform, false);
            TextMeshProUGUI tmp = textObj.AddComponent<TextMeshProUGUI>();
            tmp.text = "KAPAT";
            tmp.fontSize = 30;
            tmp.alignment = TextAlignmentOptions.Center;
            tmp.color = Color.white;
            
            RectTransform trt = textObj.GetComponent<RectTransform>();
            trt.anchorMin = Vector2.zero;
            trt.anchorMax = Vector2.one;
            trt.offsetMin = Vector2.zero;
            trt.offsetMax = Vector2.zero;
        }

        GameObject gameManagerObj = GameObject.Find("GameManager");
        StatisticsController sc = gameManagerObj.GetComponent<StatisticsController>();
        if (!sc) sc = gameManagerObj.AddComponent<StatisticsController>();

        sc.GetType().GetField("statsPanel", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).SetValue(sc, statsPanel);
        sc.GetType().GetField("gameManager", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).SetValue(sc, gameManagerObj.GetComponent<GameManager>());
        sc.GetType().GetField("statsContent", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).SetValue(sc, content);

        Button cBtn = closeBtn.GetComponent<Button>();
        #if UNITY_EDITOR
        UnityEditor.Events.UnityEventTools.AddPersistentListener(cBtn.onClick, sc.HideStats);
        #endif

        statsPanel.SetActive(false);

        GameObject mainMenu = GameObject.Find("Canvas/MainMenuPanel");
        Transform statsBtnTr = mainMenu.transform.Find("StatsBtn");
        if (!statsBtnTr)
        {
            GameObject btnObj = new GameObject("StatsBtn");
            btnObj.transform.SetParent(mainMenu.transform, false);
            
            Image img = btnObj.AddComponent<Image>();
            img.sprite = AssetDatabase.LoadAssetAtPath<Sprite>("Assets/Textures/RoundedSquare.png");
            img.type = Image.Type.Sliced;
            img.color = new Color(0.2f, 0.4f, 0.6f);

            Button btn = btnObj.AddComponent<Button>();
            
            RectTransform brt = btnObj.GetComponent<RectTransform>();
            brt.anchorMin = new Vector2(0.5f, 0.2f);
            brt.anchorMax = new Vector2(0.5f, 0.2f);
            brt.sizeDelta = new Vector2(200, 60);
            brt.anchoredPosition = new Vector2(0, 0);

            GameObject textObj = new GameObject("Text");
            textObj.transform.SetParent(btnObj.transform, false);
            TextMeshProUGUI tmp = textObj.AddComponent<TextMeshProUGUI>();
            tmp.text = "İSTATİSTİKLER";
            tmp.fontSize = 24;
            tmp.alignment = TextAlignmentOptions.Center;
            tmp.color = Color.white;
            
            RectTransform trt = textObj.GetComponent<RectTransform>();
            trt.anchorMin = Vector2.zero;
            trt.anchorMax = Vector2.one;
            trt.offsetMin = Vector2.zero;
            trt.offsetMax = Vector2.zero;

            #if UNITY_EDITOR
            UnityEditor.Events.UnityEventTools.AddPersistentListener(btn.onClick, sc.ShowStats);
            #endif
        }

        GameObject gamePanel = GameObject.Find("Canvas/GamePanel");
        Transform backBtnTr = gamePanel.transform.Find("BackBtn");
        if (!backBtnTr)
        {
            GameObject btnObj = new GameObject("BackBtn");
            btnObj.transform.SetParent(gamePanel.transform, false);
            
            Image img = btnObj.AddComponent<Image>();
            img.sprite = AssetDatabase.LoadAssetAtPath<Sprite>("Assets/Textures/RoundedSquare.png");
            img.type = Image.Type.Sliced;
            img.color = new Color(0.3f, 0.3f, 0.3f); 

            Button btn = btnObj.AddComponent<Button>();
            
            RectTransform brt = btnObj.GetComponent<RectTransform>();
            brt.anchorMin = new Vector2(0, 1);
            brt.anchorMax = new Vector2(0, 1);
            brt.pivot = new Vector2(0, 1);
            brt.sizeDelta = new Vector2(60, 60);
            brt.anchoredPosition = new Vector2(20, -20);

            GameObject textObj = new GameObject("Text");
            textObj.transform.SetParent(btnObj.transform, false);
            TextMeshProUGUI tmp = textObj.AddComponent<TextMeshProUGUI>();
            tmp.text = "<";
            tmp.fontSize = 40;
            tmp.alignment = TextAlignmentOptions.Center;
            tmp.color = Color.white;
            
            RectTransform trt = textObj.GetComponent<RectTransform>();
            trt.anchorMin = Vector2.zero;
            trt.anchorMax = Vector2.one;
            trt.offsetMin = Vector2.zero;
            trt.offsetMax = Vector2.zero;

            GameManager gm = gameManagerObj.GetComponent<GameManager>();
            #if UNITY_EDITOR
            UnityEditor.Events.UnityEventTools.AddPersistentListener(btn.onClick, gm.ReturnToMenu);
            #endif
        }

        Debug.Log("Stats UI and Back Button Setup Complete!");
    }
}
