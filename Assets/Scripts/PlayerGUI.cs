using UnityEngine;

public class PlayerGUI : MonoBehaviour
{
    public GUISkin sciFiSkin; // Assign the Sci-Fi skin in the Inspector

    [Header("Player Components")]
    public PlayerHealth playerHealth;         // Reference to PlayerHealth script
    public BarrelController barrelController; // Reference to BarrelController for reload state

    [Header("Health Bar Settings")]
    public Color healthBarFillColor = Color.green;       // Fill color of the health bar
    public float healthBarHeight = 40f;                  // Height of the health bar
    public float healthBarWidth = 300f;                  // Width of the health bar

    [Header("Reload Bar Settings")]
    public Color reloadBarFillColor = new Color(0f, 0.7f, 1f); // Fill color of the reload bar
    public float reloadBarHeight = 40f;                  // Height of the reload bar
    public float reloadBarWidth = 300f;                  // Width of the reload bar

    private void OnGUI()
    {
        // Apply the Sci-Fi skin
        GUI.skin = sciFiSkin;

        // Draw the health bar on the bottom-left
        DrawHealthBar();

        // Draw the reload bar on the bottom-right
        DrawReloadBar();
    }

    private void DrawHealthBar()
    {
        // Calculate health progress
        float healthFraction = (float)playerHealth.CurrentHealth / playerHealth.maxHealth;

        // Position for the health bar (bottom-left)
        float barX = 40f; // Distance from the left edge
        float barY = Screen.height - healthBarHeight - 60f; // Distance from the bottom edge

        // Define the health bar fill
        Rect healthBarFill = new Rect(barX, barY, healthBarWidth * healthFraction, healthBarHeight);
        GUI.color = GetHealthBarColor(healthFraction); // Dynamic color based on health fraction
        GUI.Box(healthBarFill, GUIContent.none, GUI.skin.button);

        // Reset color
        GUI.color = Color.white;
    }

    private void DrawReloadBar()
    {
        // Calculate reload progress
        float reloadProgress = barrelController.ReloadProgress;

        // Position for the reload bar (bottom-right)
        float barX = Screen.width - reloadBarWidth - 40f; // Distance from the right edge
        float barY = Screen.height - reloadBarHeight - 60f; // Distance from the bottom edge

        // Define the reload bar fill
        Rect reloadBarFill = new Rect(barX, barY, reloadBarWidth * reloadProgress, reloadBarHeight);
        GUI.color = reloadProgress >= 1f ? Color.blue : reloadBarFillColor; // Change color when reload is complete
        GUI.Box(reloadBarFill, GUIContent.none, GUI.skin.button);

        // Reset color
        GUI.color = Color.white;
    }

    private Color GetHealthBarColor(float healthFraction)
    {
        Debug.Log($"Health Fraction: {healthFraction}");

        if (healthFraction >= 0.8f) // Includes 0.8
        {
            return new Color(0f, 1f, 0.2f); // Stronger green
        }
        else if (healthFraction >= 0.6f) // Includes 0.6
        {
            return Color.green; // Default green
        }
        else if (healthFraction >= 0.4f) // Includes 0.4
        {
            return Color.yellow; // Lighter yellow
        }
        else if (healthFraction >= 0.2f) // Includes 0.2
        {
            return new Color(1f, 0.65f, 0f); // Orange
        }
        else
        {
            return Color.red; // Red
        }
    }

}
