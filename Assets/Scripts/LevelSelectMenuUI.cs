using UnityEngine;

public class LevelSelectMenuUI : MonoBehaviour
{
    public GUISkin sciFiSkin; // Assign Sci-Fi Skin in the Inspector
    public string loadingScreenScene = "LoadingScreen"; // Name of the loading screen scene

    private void OnGUI()
    {
        // Apply the Sci-Fi skin
        GUI.skin = sciFiSkin;

        // Define the area for the level menu
        GUILayout.BeginArea(new Rect(Screen.width / 2 - 150, Screen.height / 2 - 250, 300, 500));

        // Display the title "Select Level"
        GUILayout.Space(20);
        GUIStyle titleStyle = new GUIStyle(GUI.skin.label)
        {
            alignment = TextAnchor.MiddleCenter,
            fontSize = 40,
            fontStyle = FontStyle.Bold
        };
        GUILayout.Label("Select Level", titleStyle);

        GUILayout.Space(40);

        // Level selection buttons
        if (GUILayout.Button("Level 1", GUILayout.Height(70)))
        {
            StartLevelWithLoadingScreen("Level1");
        }

        GUILayout.Space(20);

        if (GUILayout.Button("Level 2", GUILayout.Height(70)))
        {
            StartLevelWithLoadingScreen("Level2");
        }

        GUILayout.Space(20);

        if (GUILayout.Button("Level 3", GUILayout.Height(70)))
        {
            StartLevelWithLoadingScreen("Level3");
        }

        GUILayout.Space(40);

        if (GUILayout.Button("Back", GUILayout.Height(50)))
        {
            GoBackToMainMenu();
        }

        GUILayout.EndArea();
    }

    /// <summary>
    /// Starts the level with a loading screen transition.
    /// </summary>
    void StartLevelWithLoadingScreen(string levelName)
    {
        Debug.Log($"Starting {levelName} with loading screen...");
        PlayerPrefs.SetString("NextScene", levelName); // Save the target level in PlayerPrefs
        UnityEngine.SceneManagement.SceneManager.LoadScene(loadingScreenScene); // Load the loading screen
    }

    /// <summary>
    /// Returns to the main menu.
    /// </summary>
    void GoBackToMainMenu()
    {
        Debug.Log("Back to Main Menu Button Clicked!");
        UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
    }
}
