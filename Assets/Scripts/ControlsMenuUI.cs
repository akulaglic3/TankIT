using UnityEngine;
using UnityEngine.SceneManagement;

public class ControlsMenuUI : MonoBehaviour
{
    public GUISkin sciFiSkin; // Assign Sci-Fi Skin in the Inspector

    private void OnGUI()
    {
        // Apply the Sci-Fi skin
        GUI.skin = sciFiSkin;

        // Define the area for the controls menu
        GUILayout.BeginArea(new Rect(Screen.width / 2 - 150, Screen.height / 2 - 250, 300, 500));

        // Display the title "Controls"
        GUILayout.Space(20);
        GUIStyle titleStyle = new GUIStyle(GUI.skin.label)
        {
            alignment = TextAnchor.MiddleCenter,
            fontSize = 40,
            fontStyle = FontStyle.Bold
        };
        GUILayout.Label("Controls", titleStyle);

        GUILayout.Space(40);

        // Display control instructions
        GUIStyle textStyle = new GUIStyle(GUI.skin.label)
        {
            fontSize = 20,
            alignment = TextAnchor.MiddleCenter
        };

        GUILayout.Label("W - Move Forward", textStyle);
        GUILayout.Label("S - Move Backward", textStyle);
        GUILayout.Label("A - Rotate Left", textStyle);
        GUILayout.Label("D - Rotate Right", textStyle);
        GUILayout.Label("Left Mouse Button - Shoot", textStyle);

        GUILayout.Space(40);

        // Back Button
        if (GUILayout.Button("Back", GUILayout.Height(50)))
        {
            GoBackToMainMenu();
        }

        GUILayout.EndArea();
    }

    private void GoBackToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
