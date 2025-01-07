using UnityEngine;

public class BossTankHealth : MonoBehaviour
{
    public int maxHealth = 7; // Total health of the boss tank
    private int currentHealth;

    [Header("Death Effects")]
    public GameObject destroyedTankPrefab; // Prefab for the destroyed tank model
    public GameObject explosionPrefab; // Primary explosion effect prefab
    public GameObject secondaryExplosionPrefab; // Secondary explosion effect prefab

    [Header("Smoke Effects")]
    public GameObject smokePrefab; // Smoke effect prefab
    public Transform smokeSpawnPoint; // Optional spawn point for smoke effects
    private GameObject activeSmokeEffect; // Reference to the active smoke effect

    public float smokeDuration = 2.0f; // Duration of the smoke effect
    public float smokeScale = 0.2f; // Scale factor for the smoke effect

    [Header("Audio Settings")]
    public AudioClip explosionSound; // Audio clip for primary explosion
    public AudioClip secondaryExplosionSound; // Audio clip for secondary explosion
    public float explosionVolume = 1.0f; // Volume for the explosion sound
    public float secondaryExplosionVolume = 0.8f; // Volume for the secondary explosion sound

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

        // Play primary explosion sound
        if (explosionSound != null && AudioManager.Instance != null)
        {
            AudioManager.Instance.PlaySFX(explosionSound, explosionVolume);
        }

        // Trigger primary explosion effect
        if (explosionPrefab != null)
        {
            GameObject explosion = Instantiate(explosionPrefab, transform.position, transform.rotation);
        }

        // Trigger secondary explosion effect
        if (secondaryExplosionPrefab != null)
        {
            Vector3 secondaryPosition = transform.position + new Vector3(2f, 0, 2f); // Offset position
            GameObject secondaryExplosion = Instantiate(secondaryExplosionPrefab, secondaryPosition, Quaternion.identity);

            // Play secondary explosion sound
            if (secondaryExplosionSound != null && AudioManager.Instance != null)
            {
                AudioManager.Instance.PlaySFX(secondaryExplosionSound, secondaryExplosionVolume);
            }
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

        // Destroy the current boss tank
        Destroy(gameObject);
    }
}
