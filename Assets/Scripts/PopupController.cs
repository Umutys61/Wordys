using UnityEngine;
using TMPro;

public class PopupController : MonoBehaviour
{
    [SerializeField] private GameObject darkOverlay;
    [SerializeField] private GameObject winPopup;
    [SerializeField] private GameObject losePopup;
    [SerializeField] private TMP_Text winInfoText;
    [SerializeField] private TMP_Text loseInfoText;


    private bool lastWasWin;
    private GameManager gameManager;

    public void Init(GameManager manager)
    {
        gameManager = manager;
        HideAll();
    }

    private void HideAll()
    {
        darkOverlay.SetActive(false);
        winPopup.SetActive(false);
        losePopup.SetActive(false);
    }

    public void ShowWin(int level)
    {
        lastWasWin = true;
        darkOverlay.SetActive(true);
        winPopup.SetActive(true);
    }

    public void ShowLose(string word)
    {
        lastWasWin = false;
        darkOverlay.SetActive(true);
        losePopup.SetActive(true);
    }

    public void OnContinue()
    {
        HideAll();
        gameManager.ContinueAfterPopup(lastWasWin);
    }

    public void OnMainMenu()
    {
        HideAll();
        gameManager.ReturnToMenu();
    }
}
