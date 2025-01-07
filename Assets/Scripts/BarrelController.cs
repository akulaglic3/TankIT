using UnityEngine;

public class BarrelController : MonoBehaviour
{
    [Header("Projectile Settings")]
    public GameObject projectilePrefab; // Assign the projectile prefab
    public Transform firePoint; // Assign the firing point
    public float projectileSpeed = 20f; // Speed of the projectile
    public float reloadTime = 2f; // Time between shots
    [Range(0f, 1f)] public float shootVolume = 0.5f; // Default volume is 50%


    [Header("Audio Settings")]
    public AudioClip shootSound; // Sound effect for shooting

    private float lastFireTime; // Track the last shot fired

    public float ReloadProgress => Mathf.Clamp01((Time.time - lastFireTime) / reloadTime);


    void Update()
    {
        if (Input.GetButtonDown("Fire1") && Time.time > lastFireTime + reloadTime)
        {
            Shoot();
            lastFireTime = Time.time; // Update the last fire time
        }
    }

    void Shoot()
    {
        // Instantiate the projectile
        GameObject projectile = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);

        // Assign the tag to the projectile
        projectile.tag = "PlayerProjectile";

        // Calculate the corrected rotation
        Quaternion correctedRotation = firePoint.rotation * Quaternion.Euler(90, 0, 0);

        // Apply the corrected rotation to the projectile
        projectile.transform.rotation = correctedRotation;

        // Set velocity on the Rigidbody
        Rigidbody rb = projectile.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.useGravity = false; // Ensure gravity is disabled
            rb.linearVelocity = firePoint.forward * projectileSpeed; // Set speed dynamically
        }
        if (shootSound != null && AudioManager.Instance != null)
        {
            AudioManager.Instance.PlaySFX(shootSound, shootVolume);
        }


        Debug.Log("Projectile fired!");
    }
}
