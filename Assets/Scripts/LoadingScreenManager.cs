using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class LoadingScreenManager : MonoBehaviour
{
    public GUISkin sciFiSkin;          // Assign the sci-fi GUI skin in the Inspector
    public string defaultNextScene = "MainMenu"; // Fallback scene if no scene is set in PlayerPrefs

    private float progress = 0f;      // Progress value for the loading bar
    private string loadingText = "Loading..."; // Initial loading text

    private void Start()
    {
        // Retrieve the next scene from PlayerPrefs or fallback to default
        string nextScene = PlayerPrefs.GetString("NextScene", defaultNextScene);

        // Switch to the loading screen music using AudioManager
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayMusic(AudioManager.Instance.loadingScreenMusic, AudioManager.MusicType.MainMenu);
        }

        // Start the async scene loading process
        StartCoroutine(LoadSceneAsync(nextScene));
    }

    private IEnumerator LoadSceneAsync(string sceneName)
    {
        // Start loading the scene asynchronously
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);

        // Prevent the scene from activating immediately
        operation.allowSceneActivation = false;

        while (!operation.isDone)
        {
            // Update the progress (operation.progress goes from 0 to 0.9)
            progress = Mathf.Clamp01(operation.progress / 0.9f);

            // Activate the scene when loading is complete
            if (operation.progress >= 0.9f)
            {
                loadingText = "Press any key to continue";
                if (Input.anyKeyDown)
                {
                    // Transition to the next scene
                    operation.allowSceneActivation = true;

                    // Switch to the appropriate music when loading is complete
                    if (AudioManager.Instance != null)
                    {
                        // If loading the main menu, play main menu music; otherwise, play game music
                        if (sceneName == "MainMenu")
                        {
                            AudioManager.Instance.PlayMainMenuMusic();
                        }
                        else
                        {
                            AudioManager.Instance.PlayGameMusic();
                        }
                    }
                }
            }

            yield return null;
        }
    }

    private void OnGUI()
    {
        // Apply the Sci-Fi skin
        GUI.skin = sciFiSkin;

        // Define the area for the loading screen UI
        GUILayout.BeginArea(new Rect(Screen.width / 2 - 200, Screen.height / 2 - 100, 400, 200));

        // Display the title "Loading"
        GUILayout.Space(20);
        GUIStyle titleStyle = new GUIStyle(GUI.skin.label)
        {
            alignment = TextAnchor.MiddleCenter,
            fontSize = 30,
            fontStyle = FontStyle.Bold
        };
        GUILayout.Label("Loading", titleStyle);

        GUILayout.Space(20);

        // Render the loading progress bar
        GUIStyle progressBarBackground = GUI.skin.box;
        GUIStyle progressBarFill = GUI.skin.button;

        // Draw the progress bar background
        GUILayout.BeginHorizontal();
        GUILayout.Box("", progressBarBackground, GUILayout.Width(300), GUILayout.Height(30));
        GUILayout.EndHorizontal();

        // Draw the progress bar fill (adjust width based on progress)
        GUILayout.BeginHorizontal();
        GUILayout.Box("", progressBarFill, GUILayout.Width(progress * 300), GUILayout.Height(30));
        GUILayout.EndHorizontal();

        GUILayout.Space(10);

        // Display the loading text or progress percentage
        GUIStyle textStyle = new GUIStyle(GUI.skin.label)
        {
            alignment = TextAnchor.MiddleCenter,
            fontSize = 20
        };
        GUILayout.Label(loadingText, textStyle);

        GUILayout.EndArea();
    }
}
