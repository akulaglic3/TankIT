using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static bool IsPaused = false; // Track pause state
    public GUISkin sciFiSkin; // Assign the Sci-Fi skin in the Inspector

    private void Update()
    {
        // Toggle pause state with the Escape key
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (IsPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    private void OnGUI()
    {
        if (IsPaused)
        {
            // Apply the Sci-Fi skin
            GUI.skin = sciFiSkin;

            // Draw semi-transparent background
            DrawOverlay();

            // Pause Menu Modal with Padding
            GUILayout.BeginArea(new Rect(Screen.width / 2 - 170, Screen.height / 2 - 240, 340, 480), GUI.skin.box); // Increased height for better bottom padding

            GUILayout.Space(30); // Space at the top for padding

            // Title
            GUILayout.Label("Paused", new GUIStyle(GUI.skin.label)
            {
                alignment = TextAnchor.MiddleCenter,
                fontSize = 40,
                fontStyle = FontStyle.Bold
            });

            GUILayout.Space(40); // Additional padding below the title

            // Resume Button
            if (GUILayout.Button("Resume", GUILayout.Height(50)))
            {
                ResumeGame();
            }

            GUILayout.Space(30); // Padding between buttons

            // Main Menu Button
            if (GUILayout.Button("Main Menu", GUILayout.Height(50)))
            {
                GoToMainMenu();
            }

            GUILayout.Space(30); // Padding between buttons

            // Quit Button
            if (GUILayout.Button("Quit Game", GUILayout.Height(50)))
            {
                QuitGame();
            }

            GUILayout.Space(30); // Bottom padding added to ensure no overlap with the border

            GUILayout.EndArea();
        }
    }

    private void DrawOverlay()
    {
        // Create a semi-transparent black background
        Color originalColor = GUI.color;
        GUI.color = new Color(0, 0, 0, 0.5f); // 50% opacity
        GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), Texture2D.whiteTexture);
        GUI.color = originalColor; // Restore original GUI color
    }

    private void PauseGame()
    {
        IsPaused = true;
        Time.timeScale = 0f; // Pause game time
        AudioManager.Instance?.PauseMusic(); // Pause music
    }

    private void ResumeGame()
    {
        IsPaused = false;
        Time.timeScale = 1f; // Resume game time
        AudioManager.Instance?.ResumeMusic(); // Resume music
    }

    private void GoToMainMenu()
    {
        Time.timeScale = 1f; // Unpause game time
        SceneManager.LoadScene("MainMenu");
    }

    private void QuitGame()
    {
        Debug.Log("Quitting game...");
        Application.Quit();
    }
}
