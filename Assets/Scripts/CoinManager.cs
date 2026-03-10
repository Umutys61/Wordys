using System;
using UnityEngine;

public class CoinManager : MonoBehaviour
{
    public static CoinManager Instance { get; private set; }

    public event Action<int> OnCoinChange;

    private int _currentCoins;
    private const string CoinKey = "PlayerCoins";

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            LoadCoins();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void LoadCoins()
    {
        _currentCoins = PlayerPrefs.GetInt(CoinKey, 0);
    }

    private void SaveCoins()
    {
        PlayerPrefs.SetInt(CoinKey, _currentCoins);
        PlayerPrefs.Save();
    }

    public int GetCoins()
    {
        return _currentCoins;
    }

    public void AddCoins(int amount)
    {
        if (amount < 0)
        {
            Debug.LogWarning("Cannot add negative coins.");
            return;
        }

        _currentCoins += amount;
        SaveCoins();
        OnCoinChange?.Invoke(_currentCoins);
    }

    public bool SpendCoins(int amount)
    {
        if (amount < 0)
        {
            Debug.LogWarning("Cannot spend negative coins.");
            return false;
        }

        if (_currentCoins >= amount)
        {
            _currentCoins -= amount;
            SaveCoins();
            OnCoinChange?.Invoke(_currentCoins);
            return true;
        }

        return false;
    }
    public void AddTestCoins()
{
    CoinManager.Instance.AddCoins(1000);
}

public void SpendTestCoins()
{
    CoinManager.Instance.SpendCoins(5);
}

}
