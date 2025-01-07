using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class BossTankController : MonoBehaviour
{
    [Header("References")]
    public Transform player;
    public GameObject leftMissilePrefab;
    public GameObject rightMissilePrefab;
    public GameObject middleMissilePrefab;
    public Transform leftFirePoint;
    public Transform rightFirePoint;
    public Transform middleFirePoint;

    [Header("Movement Settings")]
    public float minDistance = 20f;
    public float maxDistance = 30f;
    public float rotateSpeed = 5f;

    [Header("Firing Settings")]
    public float fireInterval = 4f;
    public float leftRightProjectileSpeed = 15f;
    public float middleProjectileSpeed = 25f;
    public float rotationAlignmentThreshold = 5f; // Degrees within which firing is allowed

    [Header("Audio Settings")]
    public AudioClip leftRightShootSound;
    public AudioClip middleShootSound;
    [Range(0f, 1f)] public float leftRightShootVolume = 0.5f;
    [Range(0f, 1f)] public float middleShootVolume = 0.7f;

    private NavMeshAgent agent;
    private float lastFireTime;
    private int firingStep = 0; // Tracks firing sequence: 0 for left/right, 1 for middle

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        if (!agent)
        {
            Debug.LogError("No NavMeshAgent attached to the boss tank!");
        }
    }

    private void Start()
    {
        VictoryDefeatManager victoryDefeatManager = FindFirstObjectByType<VictoryDefeatManager>();
        if (victoryDefeatManager != null)
        {
            victoryDefeatManager.RegisterEnemy(gameObject);
        }
    }

    private void Update()
    {
        if (!player)
        {
            Debug.LogWarning("No Player assigned to Boss Tank!");
            return;
        }

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        // Stop all actions if the player is out of max range
        if (distanceToPlayer > maxDistance)
        {
            agent.isStopped = true;
            return;
        }

        // Follow player if within range
        if (distanceToPlayer > minDistance)
        {
            agent.isStopped = false;
            agent.SetDestination(player.position);
        }
        else
        {
            agent.isStopped = true;
            RotateToFacePlayer();
        }

        // Handle firing logic
        if (Time.time >= lastFireTime + fireInterval && IsFacingPlayer())
        {
            FireSequence();
            lastFireTime = Time.time;
        }
    }

    private void RotateToFacePlayer()
    {
        Vector3 directionToPlayer = (player.position - transform.position).normalized;
        directionToPlayer.y = 0f; // Keep rotation on the horizontal plane

        if (directionToPlayer.sqrMagnitude > 0.001f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(directionToPlayer);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotateSpeed * Time.deltaTime);
        }
    }

    private bool IsFacingPlayer()
    {
        Vector3 directionToPlayer = (player.position - transform.position).normalized;
        float angleToPlayer = Vector3.Angle(transform.forward, directionToPlayer);
        return angleToPlayer <= rotationAlignmentThreshold;
    }

    private void FireSequence()
    {
        if (firingStep % 2 == 0) // Left and Right Missiles
        {
            FireMissile(leftMissilePrefab, leftFirePoint, leftRightProjectileSpeed, leftRightShootSound, leftRightShootVolume);
            FireMissile(rightMissilePrefab, rightFirePoint, leftRightProjectileSpeed, leftRightShootSound, leftRightShootVolume);
        }
        else // Middle Missile
        {
            FireMissile(middleMissilePrefab, middleFirePoint, middleProjectileSpeed, middleShootSound, middleShootVolume);
        }

        firingStep++;
    }

    private void FireMissile(GameObject missilePrefab, Transform firePoint, float speed, AudioClip shootSound, float volume)
    {
        if (!missilePrefab || !firePoint)
        {
            Debug.LogWarning("Missing missile prefab or fire point. Cannot fire.");
            return;
        }

        GameObject missile = Instantiate(missilePrefab, firePoint.position, firePoint.rotation);
        Quaternion correctedRotation = firePoint.rotation * Quaternion.Euler(100, 0, 0);

        // Apply the corrected rotation to the projectile
        missile.transform.rotation = correctedRotation;
        Rigidbody missileRb = missile.GetComponent<Rigidbody>();

        if (missileRb)
        {
            Vector3 directionToPlayer = (player.position - firePoint.position).normalized;
            missileRb.useGravity = false;
            missileRb.linearVelocity = directionToPlayer * speed;
        }

        Destroy(missile, 5f); // Cleanup missile after 5 seconds

        if (shootSound != null && AudioManager.Instance != null)
        {
            AudioManager.Instance.PlaySFX(shootSound, volume);
        }

        Debug.Log($"Missile fired from {firePoint.name}.");
    }
}
