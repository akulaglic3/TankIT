using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [Header("Health Settings")]
    public int maxHealth = 5; // Total health of the player's tank
    private int currentHealth;

    public int CurrentHealth => currentHealth; // Expose current health

    [Header("Effects")]
    public GameObject destroyedTankPrefab; // Prefab for the destroyed tank model
    public GameObject explosionPrefab;    // Explosion effect prefab
    public AudioClip explosionSound;      // Explosion sound effect

    [Header("Camera")]
    public Camera playerCamera; // Reference to the camera following the tank
    public Transform cameraDetachPoint; // Position to move the camera to after detachment

    [Header("Outcome Manager")]
    public VictoryDefeatManager victoryDefeatManager; // Reference to manage outcomes

    private bool isGameOver = false; // Prevent multiple triggers of the game over logic

    void Start()
    {
        currentHealth = maxHealth; // Initialize current health
       
    }

    public void TakeDamage(int damage)
    {
        if (isGameOver) return; // Skip further damage processing if the game is over

        currentHealth -= damage; // Reduce health by the damage amount
        Debug.Log($"Player tank took damage! Current health: {currentHealth}");

        // Check if the tank is destroyed
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        if (isGameOver) return; // Ensure this logic runs only once
        isGameOver = true;

        Debug.Log("Player tank destroyed! Game over.");

        // Play explosion sound
        // Play explosion sound using AudioManager
        if (explosionSound != null && AudioManager.Instance != null)
        {
            AudioManager.Instance.PlaySFX(explosionSound);
        }


        // Trigger an explosion effect
        if (explosionPrefab != null)
        {
            GameObject explosion = Instantiate(explosionPrefab, transform.position, transform.rotation);
            Destroy(explosion, 2f); // Destroy the explosion effect after 2 seconds
        }

        // Detach and transition the camera
        if (playerCamera != null && cameraDetachPoint != null)
        {
            playerCamera.transform.SetParent(null);
            playerCamera.transform.position = cameraDetachPoint.position;
            playerCamera.transform.rotation = cameraDetachPoint.rotation;
            Debug.Log("Camera instantly moved to the detach point.");
        }

        // Trigger the defeat badge
        if (victoryDefeatManager != null)
        {
            victoryDefeatManager.ShowOutcome(false); // Trigger defeat animation
        }
        else
        {
            Debug.LogWarning("VictoryDefeatManager not assigned!");
        }

        // Spawn the destroyed tank model
        if (destroyedTankPrefab != null)
        {
            Instantiate(destroyedTankPrefab, transform.position, transform.rotation);
        }

        // Disable the player's tank
        gameObject.SetActive(false);
    }
}
