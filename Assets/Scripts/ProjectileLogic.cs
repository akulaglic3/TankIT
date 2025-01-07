using UnityEngine;

public class ProjectileLogic : MonoBehaviour
{
    [Header("Projectile Settings")]
    public float lifetime = 20f;  // Destroy after 20 seconds
    public int damage = 1;        // Damage dealt by the projectile

    [Header("Effects")]
    public GameObject explosionEffect; // Prefab for the explosion effect

    [Header("Audio Clips")]
    public AudioClip hitTankSound;     // Sound when hitting a tank (enemy or player)
    public AudioClip hitBossSound;     // Sound when hitting the boss tank
    public AudioClip hitObjectSound;   // Sound when hitting any other object
    public AudioClip hitPlayerSound;   // Sound when hitting the player tank

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
        // Check for BossHealth component (boss tank)
        else if (collision.gameObject.GetComponent<BossTankHealth>() is BossTankHealth bossHealth)
        {
            // Deal damage to the boss tank
            bossHealth.TakeDamage(damage);

            if (hitBossSound != null && AudioManager.Instance != null)
            {
                AudioManager.Instance.PlaySFX(hitBossSound);
            }
        }
        else
        {
            // Play sound for hitting other objects
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

            // Optional: Destroy the explosion effect after its duration to avoid clutter
            ParticleSystem particleSystem = explosion.GetComponent<ParticleSystem>();
            if (particleSystem != null)
            {
                Destroy(explosion, particleSystem.main.duration);
            }
            else
            {
                Destroy(explosion, 2f); // Default fallback duration
            }
        }
    }
}
