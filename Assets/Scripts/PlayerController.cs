using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    private Rigidbody rb;
    private float movementInput;
    private float rotationInput;

    public float moveSpeed = 5f; // Speed of forward/backward movement
    public float turnSpeed = 100f; // Speed of rotation (degrees per second)

    [Header("Audio Clips")]
    public AudioClip driveSound; // Sound played when driving
    public AudioClip rotateSound; // Sound played when rotating

    [Range(0f, 1f)] public float driveVolume = 0.5f; // Volume of driving sound
    [Range(0f, 1f)] public float rotateVolume = 0.5f; // Volume of rotating sound

    private AudioSource driveAudioSource;
    private AudioSource rotateAudioSource;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        // Add and configure AudioSources
        driveAudioSource = gameObject.AddComponent<AudioSource>();
        driveAudioSource.clip = driveSound;
        driveAudioSource.loop = true; // Loop the sound for continuous playback
        driveAudioSource.volume = driveVolume;

        rotateAudioSource = gameObject.AddComponent<AudioSource>();
        rotateAudioSource.clip = rotateSound;
        rotateAudioSource.loop = true; // Loop the sound for continuous playback
        rotateAudioSource.volume = rotateVolume;
    }

    void FixedUpdate()
    {
        // Move the tank forward/backward
        Vector3 moveDirection = transform.forward * movementInput * moveSpeed * Time.fixedDeltaTime;
        rb.MovePosition(rb.position + moveDirection);

        // Rotate the tank
        float turnAmount = rotationInput * turnSpeed * Time.fixedDeltaTime;
        Quaternion turnRotation = Quaternion.Euler(0f, turnAmount, 0f);
        rb.MoveRotation(rb.rotation * turnRotation);

        // Handle sound effects
        HandleSoundEffects();
    }

    void OnMove(InputValue movementValue)
    {
        Vector2 movementVector = movementValue.Get<Vector2>();
        movementInput = movementVector.y; // Forward/Backward (W/S or Up/Down)
        rotationInput = movementVector.x; // Left/Right rotation (A/D or Left/Right)
    }

    private void HandleSoundEffects()
    {
        if (Mathf.Abs(rotationInput) > 0f) // If rotating
        {
            if (!rotateAudioSource.isPlaying)
            {
                rotateAudioSource.Play();
            }

            if (driveAudioSource.isPlaying)
            {
                driveAudioSource.Stop();
            }
        }
        else if (Mathf.Abs(movementInput) > 0f) // If driving but not rotating
        {
            if (!driveAudioSource.isPlaying)
            {
                driveAudioSource.Play();
            }

            if (rotateAudioSource.isPlaying)
            {
                rotateAudioSource.Stop();
            }
        }
        else // If idle
        {
            if (rotateAudioSource.isPlaying)
            {
                rotateAudioSource.Stop();
            }

            if (driveAudioSource.isPlaying)
            {
                driveAudioSource.Stop();
            }
        }
    }
}
