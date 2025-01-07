using UnityEngine;

public class StrongerProjectile : MonoBehaviour
{
    [Header("Projectile Settings")]
    public float lifetime = 20f;  // Destroy after 20 seconds
    public int damage = 3;        // Damage dealt by the projectile

    [Header("Effects")]
    public GameObject explosionEffect; // Prefab for the explosion effect
    [Range(1f, 10f)] public float explosionDuration = 4f; // Slider for explosion effect duration

    [Header("Audio Clips")]
    public AudioClip hitTankSound; // Sound when hitting a tank (enemy or player)
    public AudioClip hitObjectSound; // Sound when hitting any other object
    public AudioClip hitPlayerSound; // Sound when hitting the player tank

    private void Start()
    {
        // Destroy the projectile after its lifetime
        Destroy(gameObject, lifetime);
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Check for PlayerHealth component (player tank)
        PlayerHealth playerHealth = collision.gameObject.GetComponent<PlayerHealth>();
        if (playerHealth != null)
        {
            // Deal damage to the player tank
            playerHealth.TakeDamage(damage);

            if (hitPlayerSound != null && AudioManager.Instance != null)
            {
                AudioManager.Instance.PlaySFX(hitPlayerSound);
            }
        }
        // Check for EnemyHealth component (enemy tank)
        else if (collision.gameObject.GetComponent<EnemyHealth>() is EnemyHealth enemyHealth)
        {
            // Deal damage to the enemy tank
            enemyHealth.TakeDamage(damage);

            if (hitTankSound != null && AudioManager.Instance != null)
            {
                AudioManager.Instance.PlaySFX(hitTankSound);
            }
        }
        else
        {
            if (hitObjectSound != null && AudioManager.Instance != null)
            {
                AudioManager.Instance.PlaySFX(hitObjectSound);
            }
        }

        // Trigger explosion effect
        TriggerExplosion(collision.contacts[0].point);

        // Destroy the projectile upon collision
        Destroy(gameObject);
    }

    private void TriggerExplosion(Vector3 position)
    {
        if (explosionEffect != null)
        {
            // Instantiate the explosion effect at the collision point
            GameObject explosion = Instantiate(explosionEffect, position, Quaternion.identity);

            // Destroy the explosion effect after the set duration
            Destroy(explosion, explosionDuration);

            Debug.Log($"Explosion triggered with duration: {explosionDuration} seconds.");
        }
    }
}
