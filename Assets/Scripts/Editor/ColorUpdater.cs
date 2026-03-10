using UnityEngine;
using UnityEngine.UI;

public class ColorUpdater : MonoBehaviour
{
    public static void Execute()
    {
        Camera cam = Camera.main;
        if (cam != null)
        {
            cam.backgroundColor = new Color(0.07f, 0.07f, 0.075f);
            cam.clearFlags = CameraClearFlags.SolidColor;
        }

        GameObject gamePanel = GameObject.Find("Canvas/GamePanel");
        if (gamePanel != null)
        {
            Image img = gamePanel.GetComponent<Image>();
            if (img == null) img = gamePanel.AddComponent<Image>();
            img.color = new Color(0.07f, 0.07f, 0.075f);
        }

        GameObject mainMenu = GameObject.Find("Canvas/MainMenuPanel");
        if (mainMenu != null)
        {
            Image img = mainMenu.GetComponent<Image>();
            if (img != null)
            {
                img.color = new Color(0.07f, 0.07f, 0.075f);
            }
        }
        
        Debug.Log("Colors Updated");
    }
}
