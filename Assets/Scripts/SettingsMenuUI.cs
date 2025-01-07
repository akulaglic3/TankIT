using UnityEngine;
using UnityEngine.SceneManagement;

public class SettingsMenuUI : MonoBehaviour
{
    public GUISkin sciFiSkin; // Assign Sci-Fi Skin in the Inspector

    // Volume settings
    private float mainMenuMusicVolume;
    private float gameSFXVolume;
    private float gameMusicVolume;

    private void Start()
    {
        // Load saved volume levels or set default values
        mainMenuMusicVolume = PlayerPrefs.GetFloat("MainMenuMusicVolume", 0.5f);
        gameSFXVolume = PlayerPrefs.GetFloat("GameSFXVolume", 0.5f);
        gameMusicVolume = PlayerPrefs.GetFloat("GameMusicVolume", 0.5f);

        // Apply the initial volume settings
        AudioManager.Instance.SetMainMenuMusicVolume(mainMenuMusicVolume); // Apply main menu music volume
        AudioManager.Instance.SetSFXVolume(gameSFXVolume);                // Apply SFX volume
        AudioManager.Instance.SetGameMusicVolume(gameMusicVolume);        // Apply game music volume
    }

    private void OnGUI()
    {
        // Apply the Sci-Fi skin
        GUI.skin = sciFiSkin;

        // Define the area for the settings menu
        GUILayout.BeginArea(new Rect(Screen.width / 2 - 150, Screen.height / 2 - 250, 300, 500));

        // Display the title "Settings"
        GUILayout.Space(20);
        GUIStyle titleStyle = new GUIStyle(GUI.skin.label)
        {
            alignment = TextAnchor.MiddleCenter,
            fontSize = 40,
            fontStyle = FontStyle.Bold
        };
        GUILayout.Label("Settings", titleStyle);

        GUILayout.Space(40);

        // Main Menu Music Volume
        GUILayout.Label("Main Menu Music Volume", GUI.skin.label);
        float newMainMenuMusicVolume = GUILayout.HorizontalSlider(mainMenuMusicVolume, 0f, 1f);
        if (newMainMenuMusicVolume != mainMenuMusicVolume)
        {
            mainMenuMusicVolume = newMainMenuMusicVolume;
            AudioManager.Instance.SetMainMenuMusicVolume(mainMenuMusicVolume); // Apply changes immediately
        }
        GUILayout.Space(20);

        // Game SFX Volume
        GUILayout.Label("Game SFX Volume", GUI.skin.label);
        float newGameSFXVolume = GUILayout.HorizontalSlider(gameSFXVolume, 0f, 1f);
        if (newGameSFXVolume != gameSFXVolume)
        {
            gameSFXVolume = newGameSFXVolume;
            AudioManager.Instance.SetSFXVolume(gameSFXVolume); // Apply changes immediately
        }
        GUILayout.Space(20);

        // Game Music Volume
        GUILayout.Label("Game Ambient Music Volume", GUI.skin.label);
        float newGameMusicVolume = GUILayout.HorizontalSlider(gameMusicVolume, 0f, 1f);
        if (newGameMusicVolume != gameMusicVolume)
        {
            gameMusicVolume = newGameMusicVolume;
            AudioManager.Instance.SetGameMusicVolume(gameMusicVolume); // Apply changes immediately
        }
        GUILayout.Space(40);

        // Back Button
        if (GUILayout.Button("Back", GUILayout.Height(50)))
        {
            SaveSettings();
            GoBackToMainMenu();
        }

        GUILayout.EndArea();
    }

    private void SaveSettings()
    {
        // Save the volume settings
        PlayerPrefs.SetFloat("MainMenuMusicVolume", mainMenuMusicVolume);
        PlayerPrefs.SetFloat("GameSFXVolume", gameSFXVolume);
        PlayerPrefs.SetFloat("GameMusicVolume", gameMusicVolume);
        PlayerPrefs.Save();
        Debug.Log("Settings Saved!");
    }

    private void GoBackToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
