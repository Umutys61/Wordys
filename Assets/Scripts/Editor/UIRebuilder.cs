using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using TMPro;

public class UIRebuilder : MonoBehaviour
{
    public static void Execute()
    {
        Sprite roundedSprite = AssetDatabase.LoadAssetAtPath<Sprite>("Assets/Textures/RoundedSquare.png");
        TMP_FontAsset font = Resources.Load<TMP_FontAsset>("Fonts & Materials/LiberationSans SDF");
        
        Color darkOverlayColor = new Color(0, 0, 0, 0.8f);
        Color panelColor = new Color(0.12f, 0.12f, 0.13f);
        Color winColor = new Color(0.325f, 0.553f, 0.306f);
        Color loseColor = new Color(0.7f, 0.2f, 0.2f);
        Color buttonTextColor = Color.white;

        GameObject popupCanvas = GameObject.Find("Canvas/GamePanel/PopupCanvas");
        if (!popupCanvas)
        {
            Debug.LogError("PopupCanvas not found!");
            return;
        }

        while (popupCanvas.transform.childCount > 0)
        {
            DestroyImmediate(popupCanvas.transform.GetChild(0).gameObject);
        }

        GameObject overlay = new GameObject("DarkOverlay");
        overlay.transform.SetParent(popupCanvas.transform, false);
        RectTransform overlayRT = overlay.AddComponent<RectTransform>();
        overlayRT.anchorMin = Vector2.zero;
        overlayRT.anchorMax = Vector2.one;
        overlayRT.offsetMin = Vector2.zero;
        overlayRT.offsetMax = Vector2.zero;
        
        Image overlayImg = overlay.AddComponent<Image>();
        overlayImg.color = darkOverlayColor;
        overlay.SetActive(false);

        GameObject winPopup = CreatePopupPanel("WinPopup", popupCanvas.transform, roundedSprite, panelColor);
        CreatePopupHeader(winPopup.transform, "TEBRİKLER!", winColor, roundedSprite);
        
        GameObject winContent = new GameObject("Content");
        winContent.transform.SetParent(winPopup.transform, false);
        RectTransform winContentRT = winContent.AddComponent<RectTransform>();
        winContentRT.anchorMin = new Vector2(0, 0);
        winContentRT.anchorMax = new Vector2(1, 1);
        winContentRT.offsetMin = new Vector2(20, 80);
        winContentRT.offsetMax = new Vector2(-20, -80);
        
        TextMeshProUGUI winText = winContent.AddComponent<TextMeshProUGUI>();
        winText.text = "Bölüm Tamamlandı!";
        winText.fontSize = 36;
        winText.alignment = TextAlignmentOptions.Center;
        winText.color = Color.white;

        CreatePopupButtons(winPopup.transform, roundedSprite, "ANA MENÜ", "SONRAKİ BÖLÜM", winColor);


        GameObject losePopup = CreatePopupPanel("LosePopup", popupCanvas.transform, roundedSprite, panelColor);
        CreatePopupHeader(losePopup.transform, "KAYBETTİN", loseColor, roundedSprite);

        GameObject loseContent = new GameObject("Content");
        loseContent.transform.SetParent(losePopup.transform, false);
        RectTransform loseContentRT = loseContent.AddComponent<RectTransform>();
        loseContentRT.anchorMin = new Vector2(0, 0);
        loseContentRT.anchorMax = new Vector2(1, 1);
        loseContentRT.offsetMin = new Vector2(20, 80);
        loseContentRT.offsetMax = new Vector2(-20, -80);
        
        VerticalLayoutGroup loseVLG = loseContent.AddComponent<VerticalLayoutGroup>();
        loseVLG.childAlignment = TextAnchor.MiddleCenter;
        loseVLG.spacing = 10;

        TextMeshProUGUI loseLabel = CreateText(loseContent.transform, "Doğru Kelime:", 24, new Color(0.7f, 0.7f, 0.7f));
        TextMeshProUGUI loseWord = CreateText(loseContent.transform, "KELIME", 48, winColor);
        loseWord.fontStyle = FontStyles.Bold;
        loseWord.name = "WordText";

        CreatePopupButtons(losePopup.transform, roundedSprite, "ANA MENÜ", "TEKRAR DENE", loseColor);

        PopupController controller = GameObject.Find("PopupController").GetComponent<PopupController>();
        
        controller.GetType().GetField("darkOverlay", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).SetValue(controller, overlay);
        controller.GetType().GetField("winPopup", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).SetValue(controller, winPopup);
        controller.GetType().GetField("losePopup", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).SetValue(controller, losePopup);
        controller.GetType().GetField("winInfoText", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).SetValue(controller, winText);
        controller.GetType().GetField("loseInfoText", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).SetValue(controller, loseWord);

        winPopup.SetActive(false);
        losePopup.SetActive(false);

        Debug.Log("UI Rebuilt Successfully!");
    }

    private static GameObject CreatePopupPanel(string name, Transform parent, Sprite sprite, Color color)
    {
        GameObject panel = new GameObject(name);
        panel.transform.SetParent(parent, false);
        RectTransform rt = panel.AddComponent<RectTransform>();
        rt.anchorMin = new Vector2(0.5f, 0.5f);
        rt.anchorMax = new Vector2(0.5f, 0.5f);
        rt.sizeDelta = new Vector2(500, 400);
        rt.anchoredPosition = Vector2.zero;

        Image img = panel.AddComponent<Image>();
        img.sprite = sprite;
        img.type = Image.Type.Sliced;
        img.color = color;

        Outline outline = panel.AddComponent<Outline>();
        outline.effectColor = new Color(0, 0, 0, 0.5f);
        outline.effectDistance = new Vector2(2, -2);

        return panel;
    }

    private static void CreatePopupHeader(Transform parent, string text, Color color, Sprite sprite)
    {
        GameObject header = new GameObject("Header");
        header.transform.SetParent(parent, false);
        RectTransform rt = header.AddComponent<RectTransform>();
        rt.anchorMin = new Vector2(0, 1);
        rt.anchorMax = new Vector2(1, 1);
        rt.sizeDelta = new Vector2(0, 80);
        rt.anchoredPosition = new Vector2(0, -40);

        Image img = header.AddComponent<Image>();
        img.sprite = sprite;
        img.type = Image.Type.Sliced;
        img.color = color;
        
        GameObject textObj = new GameObject("TitleText");
        textObj.transform.SetParent(header.transform, false);
        RectTransform textRT = textObj.AddComponent<RectTransform>();
        textRT.anchorMin = Vector2.zero;
        textRT.anchorMax = Vector2.one;
        textRT.offsetMin = Vector2.zero;
        textRT.offsetMax = Vector2.zero;

        TextMeshProUGUI tmp = textObj.AddComponent<TextMeshProUGUI>();
        tmp.text = text;
        tmp.fontSize = 40;
        tmp.alignment = TextAlignmentOptions.Center;
        tmp.fontStyle = FontStyles.Bold;
        tmp.color = Color.white;
    }

    private static TextMeshProUGUI CreateText(Transform parent, string content, float size, Color color)
    {
        GameObject obj = new GameObject("Text");
        obj.transform.SetParent(parent, false);
        TextMeshProUGUI tmp = obj.AddComponent<TextMeshProUGUI>();
        tmp.text = content;
        tmp.fontSize = size;
        tmp.color = color;
        tmp.alignment = TextAlignmentOptions.Center;
        return tmp;
    }

    private static void CreatePopupButtons(Transform parent, Sprite sprite, string menuText, string actionText, Color actionColor)
    {
        GameObject container = new GameObject("Buttons");
        container.transform.SetParent(parent, false);
        RectTransform rt = container.AddComponent<RectTransform>();
        rt.anchorMin = new Vector2(0, 0);
        rt.anchorMax = new Vector2(1, 0);
        rt.sizeDelta = new Vector2(0, 80);
        rt.anchoredPosition = new Vector2(0, 40);

        HorizontalLayoutGroup hlg = container.AddComponent<HorizontalLayoutGroup>();
        hlg.spacing = 20;
        hlg.childAlignment = TextAnchor.MiddleCenter;
        hlg.childControlWidth = false;
        hlg.childControlHeight = false;

        CreateButton(container.transform, sprite, menuText, new Color(0.4f, 0.4f, 0.4f), "OnMainMenu");

        CreateButton(container.transform, sprite, actionText, actionColor, "OnContinue");
    }

    private static void CreateButton(Transform parent, Sprite sprite, string text, Color color, string methodName)
    {
        GameObject btnObj = new GameObject("Btn_" + methodName);
        btnObj.transform.SetParent(parent, false);
        
        Image img = btnObj.AddComponent<Image>();
        img.sprite = sprite;
        img.type = Image.Type.Sliced;
        img.color = color;

        Button btn = btnObj.AddComponent<Button>();
        
        RectTransform rt = btnObj.GetComponent<RectTransform>();
        rt.sizeDelta = new Vector2(180, 50);

        GameObject textObj = new GameObject("Text");
        textObj.transform.SetParent(btnObj.transform, false);
        RectTransform textRT = textObj.AddComponent<RectTransform>();
        textRT.anchorMin = Vector2.zero;
        textRT.anchorMax = Vector2.one;
        textRT.offsetMin = Vector2.zero;
        textRT.offsetMax = Vector2.zero;

        TextMeshProUGUI tmp = textObj.AddComponent<TextMeshProUGUI>();
        tmp.text = text;
        tmp.fontSize = 20;
        tmp.alignment = TextAlignmentOptions.Center;
        tmp.color = Color.white;
        tmp.fontStyle = FontStyles.Bold;

        PopupController pc = GameObject.Find("PopupController").GetComponent<PopupController>();
        #if UNITY_EDITOR
        if (methodName == "OnMainMenu")
            UnityEditor.Events.UnityEventTools.AddPersistentListener(btn.onClick, pc.OnMainMenu);
        else if (methodName == "OnContinue")
            UnityEditor.Events.UnityEventTools.AddPersistentListener(btn.onClick, pc.OnContinue);
        #endif
    }
}
