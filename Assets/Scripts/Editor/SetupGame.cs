using UnityEngine;
using UnityEditor;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;

public class SetupGame
{
    public static void Execute()
    {
        Debug.Log("Starting Game Setup...");

        var gameManagerGO = GameObject.Find("GameManager");
        var wordManagerGO = GameObject.Find("WordManager");
        var keyboardManagerGO = GameObject.Find("KeyboardManager");
        var popupControllerGO = GameObject.Find("PopupController");
        
        var canvas = GameObject.Find("Canvas");
        if (canvas == null) { Debug.LogError("Canvas not found!"); return; }

        var gamePanel = canvas.transform.Find("GamePanel")?.gameObject;
        var mainMenuPanel = canvas.transform.Find("MainMenuPanel")?.gameObject;
        
        if (gamePanel == null) { Debug.LogError("GamePanel not found!"); return; }
        
        var gridArea = gamePanel.transform.Find("GameLayout/GridArea")?.gameObject;
        var levelText = gamePanel.transform.Find("GameLayout/LevelText")?.GetComponent<TMP_Text>();
        var keyboardLayout = gamePanel.transform.Find("GameLayout/KeybordArea/KeyboardLayout")?.gameObject;

        if (gameManagerGO == null) { Debug.LogError("GameManager not found!"); return; }
        if (wordManagerGO == null) { Debug.LogError("WordManager not found!"); return; }
        if (keyboardManagerGO == null) { Debug.LogError("KeyboardManager not found!"); return; }
        if (gridArea == null) { Debug.LogError("GridArea not found!"); return; }

        var gameManager = gameManagerGO.GetComponent<GameManager>();
        var soGameManager = new SerializedObject(gameManager);
        soGameManager.Update();
        soGameManager.FindProperty("gridManager").objectReferenceValue = gridArea.GetComponent<GridManager>();
        soGameManager.FindProperty("wordManager").objectReferenceValue = wordManagerGO.GetComponent<WordManager>();
        soGameManager.FindProperty("levelText").objectReferenceValue = levelText;
        soGameManager.FindProperty("popupController").objectReferenceValue = popupControllerGO.GetComponent<PopupController>();
        soGameManager.FindProperty("mainMenuController").objectReferenceValue = gameManagerGO.GetComponent<MainMenuController>();
        soGameManager.ApplyModifiedProperties();

        Debug.Log("GameManager configured.");

        var mainMenuController = gameManagerGO.GetComponent<MainMenuController>();
        var soMainMenu = new SerializedObject(mainMenuController);
        soMainMenu.Update();
        soMainMenu.FindProperty("gameManager").objectReferenceValue = gameManager;
        soMainMenu.FindProperty("gamePanel").objectReferenceValue = gamePanel;
        soMainMenu.FindProperty("menuPanel").objectReferenceValue = mainMenuPanel;
        soMainMenu.ApplyModifiedProperties();

        Debug.Log("MainMenuController configured.");

        var wordManager = wordManagerGO.GetComponent<WordManager>();
        var soWordManager = new SerializedObject(wordManager);
        soWordManager.Update();
        var wordListsProp = soWordManager.FindProperty("wordLists");
        wordListsProp.ClearArray();
        
        string[] files = { "words_tr_4", "words_tr_5", "words_tr_6", "words_tr_7" };
        int[] lengths = { 4, 5, 6, 7 };

        for (int i = 0; i < files.Length; i++)
        {
            var textAsset = AssetDatabase.LoadAssetAtPath<TextAsset>($"Assets/Data/{files[i]}.txt");
            if (textAsset != null)
            {
                wordListsProp.InsertArrayElementAtIndex(i);
                var element = wordListsProp.GetArrayElementAtIndex(i);
                element.FindPropertyRelative("length").intValue = lengths[i];
                element.FindPropertyRelative("file").objectReferenceValue = textAsset;
            }
            else
            {
                Debug.LogError($"Could not load {files[i]}.txt");
            }
        }
        soWordManager.ApplyModifiedProperties();

        Debug.Log("WordManager configured.");

        var keyboardManager = keyboardManagerGO.GetComponent<KeyboardManager>();
        var soKeyboardManager = new SerializedObject(keyboardManager);
        soKeyboardManager.Update();
        soKeyboardManager.FindProperty("gridManager").objectReferenceValue = gridArea.GetComponent<GridManager>();
        
        var keys = keyboardLayout.GetComponentsInChildren<KeyboardKey>(true);
        var letterKeysProp = soKeyboardManager.FindProperty("letterKeys");
        letterKeysProp.ClearArray();
        for (int i = 0; i < keys.Length; i++)
        {
            letterKeysProp.InsertArrayElementAtIndex(i);
            letterKeysProp.GetArrayElementAtIndex(i).objectReferenceValue = keys[i];
        }
        soKeyboardManager.ApplyModifiedProperties();

        Debug.Log($"KeyboardManager configured with {keys.Length} keys.");

        var gridManager = gridArea.GetComponent<GridManager>();
        var soGridManager = new SerializedObject(gridManager);
        soGridManager.Update();
        soGridManager.FindProperty("cellPrefab").objectReferenceValue = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/LetterCell.prefab").GetComponent<LetterCell>();
        soGridManager.FindProperty("gridContainer").objectReferenceValue = gridArea.transform;
        soGridManager.FindProperty("wordManager").objectReferenceValue = wordManager;
        soGridManager.FindProperty("keyboardManager").objectReferenceValue = keyboardManager;
        soGridManager.FindProperty("gameManager").objectReferenceValue = gameManager;
        soGridManager.ApplyModifiedProperties();

        Debug.Log("GridManager configured.");

        var statsController = gameManagerGO.GetComponent<StatisticsController>();
        if (statsController != null)
        {
            var soStats = new SerializedObject(statsController);
            soStats.Update();
            soStats.FindProperty("gameManager").objectReferenceValue = gameManager;
            
            var statsPanel = mainMenuPanel.transform.Find("StatsPanel")?.gameObject;
            if (statsPanel == null)
            {
                Debug.Log("Creating StatsPanel...");
                statsPanel = new GameObject("StatsPanel");
                statsPanel.transform.SetParent(mainMenuPanel.transform, false);
                var rt = statsPanel.AddComponent<RectTransform>();
                rt.anchorMin = Vector2.zero;
                rt.anchorMax = Vector2.one;
                rt.offsetMin = Vector2.zero;
                rt.offsetMax = Vector2.zero;
                
                var img = statsPanel.AddComponent<Image>();
                img.color = new Color(0, 0, 0, 0.9f);

                var content = new GameObject("Content");
                content.transform.SetParent(statsPanel.transform, false);
                var contentRT = content.AddComponent<RectTransform>();
                contentRT.anchorMin = new Vector2(0.1f, 0.1f);
                contentRT.anchorMax = new Vector2(0.9f, 0.9f);
                
                var vlg = content.AddComponent<VerticalLayoutGroup>();
                vlg.childControlHeight = false;
                vlg.childControlWidth = true;
                vlg.spacing = 10;
                
                var closeBtn = new GameObject("CloseButton");
                closeBtn.transform.SetParent(statsPanel.transform, false);
                var closeRT = closeBtn.AddComponent<RectTransform>();
                closeRT.anchorMin = new Vector2(0.9f, 0.9f);
                closeRT.anchorMax = new Vector2(1, 1);
                closeRT.sizeDelta = new Vector2(50, 50);
                
                var closeImg = closeBtn.AddComponent<Image>();
                closeImg.color = Color.red;
                
                var btn = closeBtn.AddComponent<Button>();
                
                soStats.FindProperty("statsContent").objectReferenceValue = content.transform;
            }
            else
            {
                var content = statsPanel.transform.Find("Content");
                if (content != null)
                    soStats.FindProperty("statsContent").objectReferenceValue = content;
            }

            soStats.FindProperty("statsPanel").objectReferenceValue = statsPanel;
            soStats.ApplyModifiedProperties();
        }

        var popupController = popupControllerGO.GetComponent<PopupController>();
        var soPopup = new SerializedObject(popupController);
        soPopup.Update();
        
        var popupCanvas = gamePanel.transform.Find("PopupCanvas");
        if (popupCanvas != null)
        {
            var winPopup = popupCanvas.Find("WinPopup");
            var losePopup = popupCanvas.Find("LosePopup");
            
            if (winPopup != null)
            {
                soPopup.FindProperty("winPopup").objectReferenceValue = winPopup.gameObject;
                var winText = winPopup.Find("Content")?.GetComponent<TMP_Text>();
                if (winText != null) soPopup.FindProperty("winInfoText").objectReferenceValue = winText;
            }
            
            if (losePopup != null)
            {
                soPopup.FindProperty("losePopup").objectReferenceValue = losePopup.gameObject;
                var content = losePopup.Find("Content");
                if (content != null)
                {
                    var loseText = content.GetComponentInChildren<TMP_Text>(true);
                    if (loseText == null)
                    {
                        var textObj = new GameObject("WordText");
                        textObj.transform.SetParent(content, false);
                        loseText = textObj.AddComponent<TextMeshProUGUI>();
                        loseText.fontSize = 36;
                        loseText.alignment = TextAlignmentOptions.Center;
                        loseText.color = Color.white;
                    }
                    soPopup.FindProperty("loseInfoText").objectReferenceValue = loseText;
                }
            }
            
            var darkOverlay = popupCanvas.Find("DarkOverlay");
            if (darkOverlay != null)
                soPopup.FindProperty("darkOverlay").objectReferenceValue = darkOverlay.gameObject;
        }
        soPopup.ApplyModifiedProperties();

        Debug.Log("Setup Complete!");
    }
}
