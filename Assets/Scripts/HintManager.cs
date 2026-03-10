using UnityEngine;
using UnityEngine.UI;

public class HintManager : MonoBehaviour
{
    [SerializeField] private GridManager gridManager;
    [SerializeField] private int hintCost = 50;
    [SerializeField] private Button hintButton;

    private void Awake()
    {
        if (hintButton != null)
        {
            hintButton.onClick.RemoveAllListeners();
            hintButton.onClick.AddListener(UseHint);
        }
    }

    public void UseHint()
    {
        if (CoinManager.Instance.GetCoins() >= hintCost)
        {
            if (gridManager.RevealRandomCorrectLetter())
            {
                CoinManager.Instance.SpendCoins(hintCost);
            }
            else
            {
                Debug.Log("Cannot use hint right now (e.g. row full or game ended).");
            }
        }
        else
        {
            Debug.Log("Not enough coins!");
        }
    }
}
