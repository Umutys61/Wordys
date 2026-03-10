using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEditor;
using UnityEngine.Events;

public class SceneSetup : MonoBehaviour
{
    public static void Execute()
    {
        GameObject canvas = GameObject.Find("Canvas");
        if (!canvas) { Debug.LogError("Canvas not found"); return; }

        GameObject gamePanel = GameObject.Find("Canvas/GamePanel");
        if (!gamePanel) { Debug.LogError("GamePanel not found"); return; }

        GameObject mainMenuPanel = GameObject.Find("Canvas/MainMenuPanel");
        if (!mainMenuPanel)
        {
            mainMenuPanel = new GameObject("MainMenuPanel");
            mainMenuPanel.transform.SetParent(canvas.transform, false);
            RectTransform rt = mainMenuPanel.AddComponent<RectTransform>();
            rt.anchorMin = Vector2.zero;
            rt.anchorMax = Vector2.one;
            rt.offsetMin = Vector2.zero;
            rt.offsetMax = Vector2.zero;
            
            Image img = mainMenuPanel.AddComponent<Image>();
            img.color = new Color(0.1f, 0.1f, 0.1f, 1f);
        }

        GameObject titleObj = GameObject.Find("Canvas/MainMenuPanel/Title");
        if (!titleObj)
        {
            titleObj = new GameObject("Title");
            titleObj.transform.SetParent(mainMenuPanel.transform, false);
            TextMeshProUGUI titleText = titleObj.AddComponent<TextMeshProUGUI>();
            titleText.text = "WORDYS";
            titleText.fontSize = 80;
            titleText.alignment = TextAlignmentOptions.Center;
            titleText.color = Color.white;
            
            RectTransform rt = titleObj.GetComponent<RectTransform>();
            rt.anchorMin = new Vector2(0.5f, 0.8f);
            rt.anchorMax = new Vector2(0.5f, 0.8f);
            rt.sizeDelta = new Vector2(500, 100);
            rt.anchoredPosition = Vector2.zero;
        }

        GameObject buttonsContainer = GameObject.Find("Canvas/MainMenuPanel/Buttons");
        if (!buttonsContainer)
        {
            buttonsContainer = new GameObject("Buttons");
            buttonsContainer.transform.SetParent(mainMenuPanel.transform, false);
            RectTransform rt = buttonsContainer.AddComponent<RectTransform>();
            rt.anchorMin = new Vector2(0.5f, 0.5f);
            rt.anchorMax = new Vector2(0.5f, 0.5f);
            rt.sizeDelta = new Vector2(300, 400);
            rt.anchoredPosition = Vector2.zero;
            
            VerticalLayoutGroup vlg = buttonsContainer.AddComponent<VerticalLayoutGroup>();
            vlg.spacing = 20;
            vlg.childAlignment = TextAnchor.MiddleCenter;
            vlg.childControlWidth = true;
            vlg.childControlHeight = false;
        }

        GameObject gameManagerObj = GameObject.Find("GameManager");
        if (!gameManagerObj) { Debug.LogError("GameManager object not found"); return; }

        MainMenuController menuController = gameManagerObj.GetComponent<MainMenuController>();
        if (!menuController) menuController = gameManagerObj.AddComponent<MainMenuController>();

        menuController.GetType().GetField("menuPanel", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).SetValue(menuController, mainMenuPanel);
        menuController.GetType().GetField("gamePanel", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).SetValue(menuController, gamePanel);
        menuController.GetType().GetField("gameManager", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).SetValue(menuController, gameManagerObj.GetComponent<GameManager>());

        while(buttonsContainer.transform.childCount > 0) {
            DestroyImmediate(buttonsContainer.transform.GetChild(0).gameObject);
        }

        CreateMenuButton(buttonsContainer.transform, "4 Harf", 4);
        CreateMenuButton(buttonsContainer.transform, "5 Harf", 5);
        CreateMenuButton(buttonsContainer.transform, "6 Harf", 6);
        CreateMenuButton(buttonsContainer.transform, "7 Harf", 7);

        GameManager gm = gameManagerObj.GetComponent<GameManager>();
        gm.GetType().GetField("mainMenuController", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).SetValue(gm, menuController);

        GameObject gridArea = GameObject.Find("Canvas/GamePanel/GameLayout/GridArea");
        GridManager gridManager = gridArea.GetComponent<GridManager>();
        
        while (gridArea.transform.childCount > 0)
        {
            DestroyImmediate(gridArea.transform.GetChild(0).gameObject);
        }

        LetterCell cellPrefab = AssetDatabase.LoadAssetAtPath<LetterCell>("Assets/Prefabs/LetterCell.prefab");
        gridManager.GetType().GetField("cellPrefab", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).SetValue(gridManager, cellPrefab);
        gridManager.GetType().GetField("gridContainer", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).SetValue(gridManager, gridArea.transform);

        WordManager wordManager = GameObject.Find("WordManager").GetComponent<WordManager>();
        var wordListsField = wordManager.GetType().GetField("wordLists", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        
        var entryType = wordManager.GetType().GetNestedType("WordListEntry");
        var array = System.Array.CreateInstance(entryType, 4);

        SetWordListEntry(array, entryType, 0, 4, "Assets/Data/words_tr_4.txt");
        SetWordListEntry(array, entryType, 1, 5, "Assets/Data/words_tr_5.txt");
        SetWordListEntry(array, entryType, 2, 6, "Assets/Data/words_tr_6.txt");
        SetWordListEntry(array, entryType, 3, 7, "Assets/Data/words_tr_7.txt");

        wordListsField.SetValue(wordManager, array);

        UpdatePopups();

        Debug.Log("Scene Setup Completed!");
    }

    private static void CreateMenuButton(Transform parent, string text, int length)
    {
        GameObject btnObj = new GameObject("Btn_" + length);
        btnObj.transform.SetParent(parent, false);
        
        Image img = btnObj.AddComponent<Image>();
        img.color = new Color(0.2f, 0.6f, 0.3f);

        Button btn = btnObj.AddComponent<Button>();
        
        GameObject textObj = new GameObject("Text");
        textObj.transform.SetParent(btnObj.transform, false);
        TextMeshProUGUI tmp = textObj.AddComponent<TextMeshProUGUI>();
        tmp.text = text;
        tmp.fontSize = 32;
        tmp.alignment = TextAlignmentOptions.Center;
        tmp.color = Color.white;
        
        RectTransform rt = textObj.GetComponent<RectTransform>();
        rt.anchorMin = Vector2.zero;
        rt.anchorMax = Vector2.one;
        rt.offsetMin = Vector2.zero;
        rt.offsetMax = Vector2.zero;

        btnObj.AddComponent<LayoutElement>().preferredHeight = 60;

        GameObject gameManagerObj = GameObject.Find("GameManager");
        MainMenuController menuController = gameManagerObj.GetComponent<MainMenuController>();
        
        UnityAction<int> action = menuController.OnWordLengthSelected;
        
        #if UNITY_EDITOR
        UnityEditor.Events.UnityEventTools.AddIntPersistentListener(btn.onClick, menuController.OnWordLengthSelected, length);
        #endif
    }

    private static void SetWordListEntry(System.Array array, System.Type type, int index, int length, string path)
    {
        object entry = System.Activator.CreateInstance(type);
        type.GetField("length").SetValue(entry, length);
        type.GetField("file").SetValue(entry, AssetDatabase.LoadAssetAtPath<TextAsset>(path));
        array.SetValue(entry, index);
    }

    private static void UpdatePopups()
    {
        GameObject popupCanvas = GameObject.Find("Canvas/GamePanel/PopupCanvas");
        if (!popupCanvas) return;

        Transform winPopup = popupCanvas.transform.Find("WinPopup");
        if (winPopup)
        {
            CreateMainMenuButton(winPopup);
            StylePopup(winPopup, new Color(0.2f, 0.4f, 0.2f, 0.95f));
        }

        Transform losePopup = popupCanvas.transform.Find("LosePopup");
        if (losePopup)
        {
            CreateMainMenuButton(losePopup);
            StylePopup(losePopup, new Color(0.4f, 0.2f, 0.2f, 0.95f));
        }
    }

    private static void StylePopup(Transform popup, Color color)
    {
        Image bg = popup.GetComponent<Image>();
        if (bg) bg.color = color;
        
        Button continueBtn = popup.GetComponentInChildren<Button>();
        if (continueBtn)
        {
            continueBtn.image.color = new Color(1f, 1f, 1f, 0.2f);
            var text = continueBtn.GetComponentInChildren<TextMeshProUGUI>();
            if (text) text.text = "DEVAM ET";
        }
    }

    private static void CreateMainMenuButton(Transform popup)
    {
        if (popup.Find("MainMenuBtn")) return;

        GameObject btnObj = new GameObject("MainMenuBtn");
        btnObj.transform.SetParent(popup, false);
        
        Image img = btnObj.AddComponent<Image>();
        img.color = new Color(1f, 1f, 1f, 0.2f);

        Button btn = btnObj.AddComponent<Button>();
        
        GameObject textObj = new GameObject("Text");
        textObj.transform.SetParent(btnObj.transform, false);
        TextMeshProUGUI tmp = textObj.AddComponent<TextMeshProUGUI>();
        tmp.text = "ANA MENÜ";
        tmp.fontSize = 24;
        tmp.alignment = TextAlignmentOptions.Center;
        tmp.color = Color.white;
        
        RectTransform rt = btnObj.GetComponent<RectTransform>();
        rt.sizeDelta = new Vector2(200, 50);
            rt.anchoredPosition = new Vector2(0, -120);
        RectTransform textRt = textObj.GetComponent<RectTransform>();
        textRt.anchorMin = Vector2.zero;
        textRt.anchorMax = Vector2.one;
        textRt.offsetMin = Vector2.zero;
        textRt.offsetMax = Vector2.zero;

        GameObject popupControllerObj = GameObject.Find("PopupController");
        PopupController pc = popupControllerObj.GetComponent<PopupController>();
        
        #if UNITY_EDITOR
        UnityEditor.Events.UnityEventTools.AddPersistentListener(btn.onClick, pc.OnMainMenu);
        #endif
    }
}
