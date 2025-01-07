using UnityEngine;
using UnityEngine.SceneManagement;

public class GraphicsMenuUI : MonoBehaviour
{
    public GUISkin sciFiSkin; // Assign Sci-Fi Skin in the Inspector

    private int currentQualityLevel; // Store the current quality level
    private string[] qualityLevels; // Store the available quality levels

    private int resolutionIndex; // Track the selected resolution index
    private Resolution[] availableResolutions; // List of available resolutions

    private bool isFullScreen = true; // Track fullscreen status
    private bool isVSyncEnabled = true; // Track VSync status

    private void Start()
    {
        // Get the current quality level
        currentQualityLevel = QualitySettings.GetQualityLevel();

        // Retrieve all quality levels
        qualityLevels = QualitySettings.names;

        // Retrieve available resolutions
        availableResolutions = Screen.resolutions;

        // Find the current resolution index
        Resolution currentResolution = Screen.currentResolution;
        resolutionIndex = System.Array.FindIndex(availableResolutions, res =>
            res.width == currentResolution.width && res.height == currentResolution.height);

        // Get the current fullscreen and VSync states
        isFullScreen = Screen.fullScreen;
        isVSyncEnabled = QualitySettings.vSyncCount > 0;
    }

    private void OnGUI()
    {
        // Apply the Sci-Fi skin
        GUI.skin = sciFiSkin;

        // Define the area for the graphics menu
        GUILayout.BeginArea(new Rect(Screen.width / 2 - 150, Screen.height / 2 - 350, 300, 700));

        // Display the title "Graphics Settings"
        GUILayout.Space(20);
        GUIStyle titleStyle = new GUIStyle(GUI.skin.label)
        {
            alignment = TextAnchor.MiddleCenter,
            fontSize = 40,
            fontStyle = FontStyle.Bold
        };
        GUILayout.Label("Graphics Settings", titleStyle);

        GUILayout.Space(40);

        // Display quality level options
        GUILayout.Label("Graphics Quality", GUI.skin.label);
        for (int i = 0; i < qualityLevels.Length; i++)
        {
            GUIStyle buttonStyle = new GUIStyle(GUI.skin.button)
            {
                fontSize = 18
            };

            // Highlight the current quality level
            if (i == currentQualityLevel)
            {
                buttonStyle.normal.textColor = Color.green;
            }

            if (GUILayout.Button(qualityLevels[i], buttonStyle, GUILayout.Height(50)))
            {
                SetQualityLevel(i);
            }

            GUILayout.Space(10);
        }

        GUILayout.Space(20);

        // Resolution Dropdown
        GUILayout.Label("Resolution", GUI.skin.label);
        if (GUILayout.Button($"{availableResolutions[resolutionIndex].width} x {availableResolutions[resolutionIndex].height}", GUILayout.Height(50)))
        {
            resolutionIndex = (resolutionIndex + 1) % availableResolutions.Length;
            SetResolution(resolutionIndex);
        }

        GUILayout.Space(20);

        // Fullscreen Toggle
        if (GUILayout.Button($"Fullscreen: {(isFullScreen ? "On" : "Off")}", GUILayout.Height(50)))
        {
            ToggleFullScreen();
        }

        GUILayout.Space(20);

        // VSync Toggle
        if (GUILayout.Button($"VSync: {(isVSyncEnabled ? "Enabled" : "Disabled")}", GUILayout.Height(50)))
        {
            ToggleVSync();
        }

        GUILayout.Space(20);

        // Back Button
        if (GUILayout.Button("Back", GUILayout.Height(50)))
        {
            GoBackToMainMenu();
        }

        GUILayout.EndArea();
    }

    private void SetQualityLevel(int index)
    {
        QualitySettings.SetQualityLevel(index);
        currentQualityLevel = index;
        Debug.Log($"Graphics quality set to: {qualityLevels[index]}");
    }

    private void SetResolution(int index)
    {
        Resolution resolution = availableResolutions[index];
        Screen.SetResolution(resolution.width, resolution.height, isFullScreen);
        resolutionIndex = index;
        Debug.Log($"Resolution set to: {resolution.width} x {resolution.height}");
    }

    private void ToggleFullScreen()
    {
        isFullScreen = !isFullScreen;
        Screen.fullScreen = isFullScreen;
        Debug.Log($"Fullscreen mode: {(isFullScreen ? "Enabled" : "Disabled")}");
    }

    private void ToggleVSync()
    {
        isVSyncEnabled = !isVSyncEnabled;
        QualitySettings.vSyncCount = isVSyncEnabled ? 1 : 0;
        Debug.Log($"VSync: {(isVSyncEnabled ? "Enabled" : "Disabled")}");
    }

    private void GoBackToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
