using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public int maxHealth = 2; // Total health of the enemy tank
    private int currentHealth;

    public GameObject destroyedTankPrefab; // Prefab for the destroyed tank model
    public GameObject explosionPrefab; // Explosion effect prefab
    public GameObject smokePrefab; // Smoke effect prefab

    public Transform smokeSpawnPoint; // Optional spawn point for smoke effects
    private GameObject activeSmokeEffect; // Reference to the active smoke effect

    public float smokeDuration = 1.0f; // Duration of the smoke effect
    public float smokeScale = 0.1f; // Scale factor for the smoke effect

    [Header("Audio Settings")]
    public AudioClip explosionSound; // Audio clip for explosion
    public float explosionVolume = 1.0f; // Volume for the explosion sound

    void Start()
    {
        currentHealth = maxHealth; // Initialize current health
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage; // Reduce health by the damage amount
        Debug.Log($"{gameObject.name} took damage! Current health: {currentHealth}");

        // Trigger smoke effect for non-final hits
        if (currentHealth > 0 && smokePrefab != null && activeSmokeEffect == null)
        {
            TriggerSmokeEffect();
        }

        // Destroy the tank if health reaches 0
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void TriggerSmokeEffect()
    {
        // Determine the spawn position for the smoke effect
        Vector3 spawnPosition = smokeSpawnPoint != null ? smokeSpawnPoint.position : transform.position;

        // Instantiate the smoke effect and parent it to the tank
        activeSmokeEffect = Instantiate(smokePrefab, spawnPosition, Quaternion.identity);
        activeSmokeEffect.transform.SetParent(smokeSpawnPoint != null ? smokeSpawnPoint : transform);

        // Scale down the smoke effect
        activeSmokeEffect.transform.localScale *= smokeScale;

        // Destroy the smoke effect after the specified duration
        Destroy(activeSmokeEffect, smokeDuration);

        Debug.Log("Smoke effect triggered with reduced size and duration.");
    }

    private void Die()
    {
        Debug.Log($"{gameObject.name} destroyed!");

        // Play explosion sound
        if (explosionSound != null && AudioManager.Instance != null)
        {
            AudioManager.Instance.PlaySFX(explosionSound, explosionVolume);
        }


        // Trigger an explosion effect
        if (explosionPrefab != null)
        {
            GameObject explosion = Instantiate(explosionPrefab, transform.position, transform.rotation);
            Destroy(explosion, 2f);
        }

        // Destroy the active smoke effect if it exists
        if (activeSmokeEffect != null)
        {
            Destroy(activeSmokeEffect);
        }

        // Notify VictoryDefeatManager
        VictoryDefeatManager victoryDefeatManager = FindFirstObjectByType<VictoryDefeatManager>();
        if (victoryDefeatManager != null)
        {
            victoryDefeatManager.DeregisterEnemy(gameObject);
        }

        // Spawn the destroyed tank model
        if (destroyedTankPrefab != null)
        {
            Instantiate(destroyedTankPrefab, transform.position, transform.rotation);
        }

        // Destroy the current enemy tank
        Destroy(gameObject);
    }

}
