using UnityEngine;

public class MainMenuController : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;
    [SerializeField] private GameObject menuPanel;
    [SerializeField] private GameObject gamePanel;

    public void OnWordLengthSelected(int length)
    {
        menuPanel.SetActive(false);
        gamePanel.SetActive(true);
        gameManager.StartGame(length);
    }

    public void ShowMenu()
    {
        menuPanel.SetActive(true);
        gamePanel.SetActive(false);
    }
}
