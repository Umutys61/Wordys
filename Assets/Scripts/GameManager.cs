using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GridManager gridManager;
    [SerializeField] private WordManager wordManager;
    [SerializeField] private TMP_Text levelText;
    [SerializeField] private PopupController popupController;
    [SerializeField] private MainMenuController mainMenuController;

    [SerializeField] private int maxLevel = 5000;

    private Dictionary<int, int> levels = new Dictionary<int, int>();
    private Dictionary<int, int> gamesPlayed = new Dictionary<int, int>();
    private int currentWordLength = 5;

    public int Level => levels.ContainsKey(currentWordLength) ? levels[currentWordLength] : 1;

    private void Awake()
    {
        LoadData();
    }

    private void Start()
    {
        popupController.Init(this);
        mainMenuController.ShowMenu();
    }

    private void LoadData()
    {
        int[] lengths = { 4, 5, 6, 7 };
        foreach (int len in lengths)
        {
            levels[len] = PlayerPrefs.GetInt($"Level_{len}", 1);
            gamesPlayed[len] = PlayerPrefs.GetInt($"Games_{len}", 0);
        }
    }

    private void SaveData()
    {
        foreach (var kvp in levels)
        {
            PlayerPrefs.SetInt($"Level_{kvp.Key}", kvp.Value);
        }
        foreach (var kvp in gamesPlayed)
        {
            PlayerPrefs.SetInt($"Games_{kvp.Key}", kvp.Value);
        }
        PlayerPrefs.Save();
    }

    public (int level, int played) GetStats(int length)
    {
        int l = levels.ContainsKey(length) ? levels[length] : 1;
        int p = gamesPlayed.ContainsKey(length) ? gamesPlayed[length] : 0;
        return (l, p);
    }

    public void StartGame(int length)
    {
        currentWordLength = length;
        
        if (!levels.ContainsKey(length))
            levels[length] = 1;

        Debug.Log($"Starting Game: Length={length}, Level={levels[length]}");

        wordManager.LoadWords(length);
        gridManager.SetupGrid(length);
        StartFirstGame();
    }

    public void ReturnToMenu()
    {
        mainMenuController.ShowMenu();
    }

    private void StartFirstGame()
    {
        wordManager.SelectRandomWord();
        UpdateLevelUI(); 
    }


   
    public void OnWin()
    {
        if (!gamesPlayed.ContainsKey(currentWordLength)) gamesPlayed[currentWordLength] = 0;
        gamesPlayed[currentWordLength]++;

        int currentLevel = levels[currentWordLength];
        if (currentLevel < maxLevel)
        {
            levels[currentWordLength]++;
        }
        
        SaveData();

        if (StatsManager.Instance != null)
        {
   
            int attempts = gridManager.CurrentRow + 1;
            StatsManager.Instance.RecordWin(attempts);
        }

        Debug.Log($"LEVEL UP ({currentWordLength} letters) → " + levels[currentWordLength]);

        wordManager.SelectRandomWord();
        gridManager.ResetGrid();
        UpdateLevelUI();
        popupController.ShowWin(levels[currentWordLength]);
    }

    public void OnLose()
    {
        if (!gamesPlayed.ContainsKey(currentWordLength)) gamesPlayed[currentWordLength] = 0;
        gamesPlayed[currentWordLength]++;
        SaveData();

        if (StatsManager.Instance != null)
        {
            StatsManager.Instance.RecordLoss();
        }

        Debug.Log("KAYBETTİN → AYNI KELİMEYLE DEVAM");
        popupController.ShowLose(wordManager.TargetWord);  
        gridManager.ResetGrid(); 
         
    }

    private void UpdateLevelUI()
    {
        if (levelText != null)
            levelText.text = $"Seviye: {Level}";
    }
public void ContinueAfterPopup(bool wasWin)
{
    if (wasWin)
    {
        wordManager.SelectRandomWord();
    }

    gridManager.ResetGrid();
}


}
