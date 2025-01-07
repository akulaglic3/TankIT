using UnityEngine;
using UnityEngine.UI;

public class ReloadIndicator : MonoBehaviour
{
    public BarrelController turretController; // Drag your turret script here
    public Slider reloadSlider; // Assign the Slider component in the Inspector

    void Update()
    {
        if (turretController != null && reloadSlider != null)
        {
            // Update the slider value based on the reload progress
            reloadSlider.value = turretController.ReloadProgress;
        }
    }
}
