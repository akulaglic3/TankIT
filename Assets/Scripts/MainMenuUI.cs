using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuUI : MonoBehaviour
{
    public GUISkin sciFiSkin; // Assign Sci-Fi Skin in the Inspector

    private void Start()
    {
        // Ensure main menu music is playing
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayMainMenuMusic();
        }
    }

    private void OnGUI()
    {
        // Apply the Sci-Fi skin
        GUI.skin = sciFiSkin;

        // Define the area for the main menu
        GUILayout.BeginArea(new Rect(Screen.width / 2 - 150, Screen.height / 2 - 250, 300, 600));

        // Display the game title "TankIT"
        GUILayout.Space(20); // Space above the title
        GUIStyle titleStyle = new GUIStyle(GUI.skin.label)
        {
            alignment = TextAnchor.MiddleCenter, // Center the title text
            fontSize = 40, // Larger font size for the title
            fontStyle = FontStyle.Bold // Bold font for emphasis
        };
        GUILayout.Label("TankIT", titleStyle); // Display the title

        GUILayout.Space(40); // Add space between the title and buttons

        // Main Menu buttons
        if (GUILayout.Button("Play", GUILayout.Height(70))) // Larger button height
        {
            PlayGame();
        }

        GUILayout.Space(20); // Add padding between buttons

        if (GUILayout.Button("Settings", GUILayout.Height(70))) // Larger button height
        {
            OpenSettings();
        }

        GUILayout.Space(20); // Add padding between buttons

        if (GUILayout.Button("Controls", GUILayout.Height(70))) // Larger button height
        {
            OpenControls();
        }

        GUILayout.Space(20); // Add padding between buttons

        if (GUILayout.Button("Graphics Settings", GUILayout.Height(70)))
        {
            SceneManager.LoadScene("GraphicsMenu");
        }

        GUILayout.Space(20); // Add padding between buttons

        if (GUILayout.Button("Exit", GUILayout.Height(70))) // Larger button height
        {
            ExitGame();
        }

        GUILayout.EndArea();
    }

    void OpenControls()
    {
        Debug.Log("Controls Button Clicked!");
        UnityEngine.SceneManagement.SceneManager.LoadScene("ControlsMenu"); // Ensure "ControlsMenu" scene is in Build Settings
    }


    void PlayGame()
    {
        Debug.Log("Play Game Button Clicked!");
        UnityEngine.SceneManagement.SceneManager.LoadScene("LevelSelect"); // Ensure "LevelSelect" scene is in Build Settings
    }

    void OpenSettings()
    {
        Debug.Log("Settings Button Clicked!");
        UnityEngine.SceneManagement.SceneManager.LoadScene("SettingsMenu"); // Ensure "SettingsMenu" scene is in Build Settings
    }

    void ExitGame()
    {
        Debug.Log("Exit Button Clicked!");
        Application.Quit(); // Only works in a built application, not in the Unity Editor
    }
}
