using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyTankController : MonoBehaviour
{
    [Header("References")]
    public Transform player;
    public GameObject projectilePrefab;
    public Transform firePoint;

    [Header("Movement Settings")]
    public float minDistance = 10f;
    public float maxDistance = 20f;
    public float rotateSpeed = 10f; // Speed of rotation when facing the player

    [Header("Firing Settings")]
    public float fireInterval = 3f;
    public float projectileSpeed = 10f;

    [Header("Bullet Dodge")]
    public float bulletDodgeRadius = 10f; // Increase from 5f for better detection
    public float bulletDodgeDuration = 0.5f;
    public float bulletDodgeCooldown = 3f;
    public string playerProjectileTag = "PlayerProjectile";

    private NavMeshAgent agent;
    private float lastFireTime;
    private float dodgeTimer = 2f;
    private float dodgeCooldownTimer = 2f;
    private bool isDodging = false;

    [Header("Audio Settings")]
    public AudioClip shootSound;
    [Range(0f, 1f)] public float shootVolume = 0.5f; // Volume for firing sound

    [Header("Idle Settings")]
    public float maxIdleDistance = 30f; // Distance beyond which the enemy tank will do nothing



    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        if (!agent)
        {
            Debug.LogError("No NavMeshAgent attached to the enemy tank!");
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
            Debug.LogWarning("No Player assigned to Enemy Tank!");
            return;
        }

        // Check if the player is beyond the idle distance
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        if (distanceToPlayer > maxIdleDistance)
        {
            // Stop all actions and wait
            agent.isStopped = true; // Ensure the NavMeshAgent is not moving
            return; // Skip the rest of the logic
        }

        // Update timers
        if (dodgeTimer > 0f) dodgeTimer -= Time.deltaTime;
        if (dodgeCooldownTimer > 0f) dodgeCooldownTimer -= Time.deltaTime;

        // Check for incoming bullets and dodge
        if (dodgeTimer <= 0f && dodgeCooldownTimer <= 0f && DetectIncomingBullet())
        {
            Debug.Log("Incoming bullet detected. Dodging.");
            StartDodge();
        }

        if (isDodging)
        {
            DodgeMovement();
            return; // Skip normal logic during dodge
        }

        // Adjust position based on distance to the player
        MoveLogic();

        // Fire if ready
        if (Time.time >= lastFireTime + fireInterval)
        {
            FireProjectile();
            lastFireTime = Time.time;
        }
    }


    private void MoveLogic()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer > maxDistance)
        {
            agent.isStopped = false;
            agent.SetDestination(player.position);
        }
        else if (distanceToPlayer < minDistance)
        {
            Debug.Log("Player too close, moving away.");
            Vector3 directionAwayFromPlayer = transform.position - player.position;
            Vector3 newDestination = transform.position + directionAwayFromPlayer.normalized * minDistance;
            agent.isStopped = false;
            agent.SetDestination(newDestination);
        }
        else
        {
            agent.isStopped = true;

            // Rotate to face the player
            RotateToFacePlayer();
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

    private void DodgeMovement()
    {
        float dodgeDirection = Random.value > 0.5f ? 1f : -1f;
        transform.Rotate(Vector3.up, dodgeDirection * 200f * Time.deltaTime);
        transform.Translate(Vector3.forward * agent.speed * Time.deltaTime);

        dodgeTimer -= Time.deltaTime;
        if (dodgeTimer <= 0f)
        {
            isDodging = false; // End dodge
            agent.isStopped = false; // Resume normal NavMeshAgent movement
        }
    }


    private void StartDodge()
    {
        dodgeTimer = bulletDodgeDuration;
        dodgeCooldownTimer = bulletDodgeCooldown;
        isDodging = true;
        Debug.Log("Started dodging.");
    }

    private void FireProjectile()
    {
        if (!firePoint || !projectilePrefab)
        {
            Debug.LogWarning("Missing firePoint or projectilePrefab. Cannot fire.");
            return;
        }

        // Fire the projectile
        GameObject projectile = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
        Quaternion correctedRotation = firePoint.rotation * Quaternion.Euler(90, 0, 0);
        projectile.transform.rotation = correctedRotation;

        // Set velocity and disable gravity
        Rigidbody projectileRb = projectile.GetComponent<Rigidbody>();
        if (projectileRb)
        {
            projectileRb.useGravity = false;
            projectileRb.linearVelocity = firePoint.forward * projectileSpeed;
        }

        Destroy(projectile, 5f); // Cleanup projectile after 5 seconds

        // Play firing sound
        if (shootSound != null && AudioManager.Instance != null)
        {
            AudioManager.Instance.PlaySFX(shootSound, shootVolume);
        }

        Debug.Log("Fired projectile in tank's forward direction.");
    }


    private bool DetectIncomingBullet()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, bulletDodgeRadius);
        foreach (Collider c in hits)
        {
            if (c.CompareTag(playerProjectileTag))
            {
                Debug.Log("Bullet detected within dodge radius.");
                return true;
            }
        }
        return false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, bulletDodgeRadius);
    }
}
