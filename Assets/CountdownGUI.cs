using System.Collections;
using UnityEngine;

public class CountdownGUI : MonoBehaviour
{
    public GUISkin sciFiSkin; // Assign the same Sci-Fi Skin in the Inspector
    public float countdownTime = 3f; // Countdown duration
    private float currentTime;
    private bool showCountdown = true; // To toggle countdown visibility

    private void Start()
    {
        currentTime = countdownTime;

        // Pause the game
        Time.timeScale = 0f;

        StartCoroutine(StartCountdown());
    }

    private void OnGUI()
    {
        if (!showCountdown) return; // Don't display if the countdown is over

        // Apply the Sci-Fi skin
        GUI.skin = sciFiSkin;

        // Define the area for the countdown text
        GUIStyle countdownStyle = new GUIStyle(GUI.skin.label)
        {
            alignment = TextAnchor.MiddleCenter, // Center the text
            fontSize = 100,                     // Large font size for the countdown
            fontStyle = FontStyle.Bold          // Bold font for emphasis
        };

        // Draw the countdown text
        GUI.Label(new Rect(Screen.width / 2 - 150, Screen.height / 2 - 100, 300, 200), currentTime > 0 ? currentTime.ToString("0") : "GO!", countdownStyle);
    }

    private IEnumerator StartCountdown()
    {
        // Use real-time instead of in-game time because Time.timeScale is 0
        while (currentTime > 0)
        {
            yield return new WaitForSecondsRealtime(1f); // Wait for 1 second in real-time
            currentTime--;
        }

        yield return new WaitForSecondsRealtime(1f); // Display "GO!" for 1 second in real-time
        showCountdown = false; // Stop showing the countdown

        // Resume the game
        Time.timeScale = 1f;

        StartGame();
    }

    private void StartGame()
    {
        Debug.Log("Game Started!"); // Replace this with your game start logic
    }
}
