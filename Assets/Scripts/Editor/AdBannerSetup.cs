using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using TMPro;

public class AdBannerSetup : MonoBehaviour
{
    public static void Execute()
    {
        GameObject canvas = GameObject.Find("Canvas");
        if (!canvas)
        {
            Debug.LogError("Canvas not found!");
            return;
        }

        GameObject adBanner = GameObject.Find("Canvas/AdBanner");
        if (!adBanner)
        {
            adBanner = new GameObject("AdBanner");
            adBanner.transform.SetParent(canvas.transform, false);
        }

        RectTransform rt = adBanner.GetComponent<RectTransform>();
        if (!rt) rt = adBanner.AddComponent<RectTransform>();
        
        rt.anchorMin = new Vector2(0, 0);
        rt.anchorMax = new Vector2(1, 0);
        rt.pivot = new Vector2(0.5f, 0);
        rt.sizeDelta = new Vector2(0, 150);
        rt.anchoredPosition = Vector2.zero;

        Image img = adBanner.GetComponent<Image>();
        if (!img) img = adBanner.AddComponent<Image>();
        img.color = new Color(0.1f, 0.1f, 0.1f, 1f);

        Transform textObj = adBanner.transform.Find("AdText");
        if (!textObj)
        {
            GameObject t = new GameObject("AdText");
            t.transform.SetParent(adBanner.transform, false);
            textObj = t.transform;
        }
        
        TextMeshProUGUI tmp = textObj.GetComponent<TextMeshProUGUI>();
        if (!tmp) tmp = textObj.gameObject.AddComponent<TextMeshProUGUI>();
        
        tmp.text = "REKLAM ALANI";
        tmp.fontSize = 40;
        tmp.alignment = TextAlignmentOptions.Center;
        tmp.color = new Color(0.5f, 0.5f, 0.5f);
        
        RectTransform textRT = textObj.GetComponent<RectTransform>();
        textRT.anchorMin = Vector2.zero;
        textRT.anchorMax = Vector2.one;
        textRT.offsetMin = Vector2.zero;
        textRT.offsetMax = Vector2.zero;


        GameObject gamePanel = GameObject.Find("Canvas/GamePanel");
        if (gamePanel)
        {
            RectTransform gpRT = gamePanel.GetComponent<RectTransform>();
            if (gpRT)
            {
                gpRT.offsetMin = new Vector2(gpRT.offsetMin.x, 150);
            }
        }

        GameObject mainMenu = GameObject.Find("Canvas/MainMenuPanel");
        if (mainMenu)
        {
            RectTransform mmRT = mainMenu.GetComponent<RectTransform>();
            if (mmRT)
            {
                mmRT.offsetMin = new Vector2(mmRT.offsetMin.x, 150);
            }
        }

        Debug.Log("Ad Banner Created and Layout Adjusted!");
    }
}
