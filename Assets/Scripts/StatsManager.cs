using UnityEngine;
using System;

[Serializable]
public class GameStats
{
    public int totalGames;
    public int totalWins;
    public int currentStreak;
    public int bestStreak;
    public int[] guessDistribution = new int[6];

    public float GetWinPercentage()
    {
        if (totalGames == 0) return 0f;
        return (float)totalWins / totalGames * 100f;
    }
}

public class StatsManager : MonoBehaviour
{
    public static StatsManager Instance { get; private set; }

    private GameStats currentStats;
    private const string STATS_KEY = "WordleGameStats";

    public GameStats CurrentStats => currentStats;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            LoadStats();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void LoadStats()
    {
        if (PlayerPrefs.HasKey(STATS_KEY))
        {
            string json = PlayerPrefs.GetString(STATS_KEY);
            currentStats = JsonUtility.FromJson<GameStats>(json);
        }
        else
        {
            currentStats = new GameStats();
        }
    }

    private void SaveStats()
    {
        string json = JsonUtility.ToJson(currentStats);
        PlayerPrefs.SetString(STATS_KEY, json);
        PlayerPrefs.Save();
    }

    public void RecordWin(int attemptNumber)
    {
        currentStats.totalGames++;
        currentStats.totalWins++;
        currentStats.currentStreak++;

        if (currentStats.currentStreak > currentStats.bestStreak)
        {
            currentStats.bestStreak = currentStats.currentStreak;
        }

        int index = Mathf.Clamp(attemptNumber - 1, 0, currentStats.guessDistribution.Length - 1);
        currentStats.guessDistribution[index]++;

        SaveStats();
    }

    public void RecordLoss()
    {
        currentStats.totalGames++;
        currentStats.currentStreak = 0;
        SaveStats();
    }
}
