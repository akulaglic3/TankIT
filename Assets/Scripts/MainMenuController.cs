using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    // Called by the "Play" or "Level Select" button
    public void OnLevelSelectPressed()
    {
        // Option A: Go directly to Level1
        // SceneManager.LoadScene("Level1");

        // Option B: Go to a separate Level Select scene
        SceneManager.LoadScene("LevelSelect");
    }

    // Called by the "Settings" button
    public void OnSettingsPressed()
    {
        // Show Settings UI (could be a new scene or a popup)
        // For now, let's assume it's a separate scene:
        SceneManager.LoadScene("SettingsMenu");
    }

    // Called by the "Exit" button
    public void OnExitPressed()
    {
        // Quits the application
        Application.Quit();
        // (Does nothing in Editor, but works in a built application)
    }
}
