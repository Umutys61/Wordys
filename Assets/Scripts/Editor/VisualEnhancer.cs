using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using TMPro;

public class VisualEnhancer : MonoBehaviour
{
    public static void Execute()
    {
        Sprite roundedSprite = AssetDatabase.LoadAssetAtPath<Sprite>("Assets/Textures/RoundedSquare.png");
        if (roundedSprite == null)
        {
            Debug.LogError("RoundedSquare sprite not found!");
            return;
        }

        Color darkBg = new Color(0.07f, 0.07f, 0.075f);
        Color buttonColor = new Color(0.506f, 0.514f, 0.518f);
        Color accentColor = new Color(0.325f, 0.553f, 0.306f);

        GameObject mainMenu = GameObject.Find("Canvas/MainMenuPanel");
        if (mainMenu)
        {
            Image bg = mainMenu.GetComponent<Image>();
            if (bg) bg.color = darkBg;

            Transform title = mainMenu.transform.Find("Title");
            if (title)
            {
                TextMeshProUGUI tmp = title.GetComponent<TextMeshProUGUI>();
                if (tmp)
                {
                    tmp.color = Color.white;
                    tmp.fontStyle = FontStyles.Bold;
                    tmp.fontSize = 100;
                }
            }

            Transform buttons = mainMenu.transform.Find("Buttons");
            if (buttons)
            {
                foreach (Transform child in buttons)
                {
                    Image btnImg = child.GetComponent<Image>();
                    if (btnImg)
                    {
                        btnImg.sprite = roundedSprite;
                        btnImg.type = Image.Type.Sliced;
                        btnImg.color = buttonColor;
                    }
                }
                
                VerticalLayoutGroup vlg = buttons.GetComponent<VerticalLayoutGroup>();
                if (vlg)
                {
                    vlg.spacing = 30;
                    vlg.padding = new RectOffset(0, 0, 20, 20);
                }
            }
        }

        GameObject popupCanvas = GameObject.Find("Canvas/GamePanel/PopupCanvas");
        if (popupCanvas)
        {
            StylePopup(popupCanvas.transform.Find("WinPopup"), roundedSprite, accentColor);
            StylePopup(popupCanvas.transform.Find("LosePopup"), roundedSprite, new Color(0.7f, 0.2f, 0.2f));
        }

        GameObject keyboardArea = GameObject.Find("Canvas/GamePanel/GameLayout/KeybordArea");
        if (keyboardArea)
        {
            Button[] keys = keyboardArea.GetComponentsInChildren<Button>(true);
            foreach (var key in keys)
            {
                Image img = key.GetComponent<Image>();
                if (img)
                {
                    img.sprite = roundedSprite;
                    img.type = Image.Type.Sliced;
                }
            }
        }
        
        GameObject gridArea = GameObject.Find("Canvas/GamePanel/GameLayout/GridArea");
        if (gridArea)
        {
             string prefabPath = "Assets/Prefabs/LetterCell.prefab";
             GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(prefabPath);
             if (prefab)
             {
                 Image img = prefab.GetComponent<Image>();
                 if (img)
                 {
                     img.sprite = roundedSprite;
                     img.type = Image.Type.Sliced;
                 }
                 
                 Outline outline = prefab.GetComponent<Outline>();
                 if (outline)
                 {
                 }
                 
                 PrefabUtility.SavePrefabAsset(prefab);
             }
        }

        Debug.Log("Visual Enhancements Applied!");
    }

    private static void StylePopup(Transform popup, Sprite sprite, Color headerColor)
    {
        if (!popup) return;

        Image bg = popup.GetComponent<Image>();
        if (bg)
        {
            bg.sprite = sprite;
            bg.type = Image.Type.Sliced;
            bg.color = new Color(0.15f, 0.15f, 0.16f, 0.98f);
        }

        Button[] buttons = popup.GetComponentsInChildren<Button>();
        foreach (var btn in buttons)
        {
            Image img = btn.GetComponent<Image>();
            if (img)
            {
                img.sprite = sprite;
                img.type = Image.Type.Sliced;
            }
        }
    }
}
