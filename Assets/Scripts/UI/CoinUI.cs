using TMPro;
using UnityEngine;

public class CoinUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI coinText;

    private void Start()
    {
        if (coinText == null)
        {
            coinText = GetComponent<TextMeshProUGUI>();
        }

        if (CoinManager.Instance != null)
        {
            CoinManager.Instance.OnCoinChange += UpdateCoinText;
            UpdateCoinText(CoinManager.Instance.GetCoins());
        }
    }

    private void OnDestroy()
    {
        if (CoinManager.Instance != null)
        {
            CoinManager.Instance.OnCoinChange -= UpdateCoinText;
        }
    }

    private void UpdateCoinText(int amount)
    {
        if (coinText != null)
        {
            coinText.text = amount.ToString();
        }
    }
}
