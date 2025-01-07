using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class VictoryDefeatManager : MonoBehaviour
{
    [Header("Outcome UI")]
    public GameObject victoryBadge;
    public GameObject defeatBadge;

    [Header("Animations")]
    public Animator victoryAnimator;
    public Animator defeatAnimator;
    public string victoryAnimationName = "VictoryAnimation";
    public string defeatAnimationName = "DefeatAnimation";

    [Header("Audio")]
    public AudioClip victoryMusic;
    public AudioClip defeatMusic;

    [Header("Settings")]
    public string levelSelectionScene = "LevelSelect";
    public float transitionDelay = 3f;

    private bool outcomeDisplayed = false;
    private bool transitionAllowed = false;

    [Header("Camera")]
    public Camera mainCamera;
    public Transform victoryCameraPoint;
    public Transform defeatCameraPoint;

    private HashSet<GameObject> activeEnemies = new HashSet<GameObject>();

    private void Start()
    {
        if (victoryBadge != null)
            victoryBadge.SetActive(false);

        if (defeatBadge != null)
            defeatBadge.SetActive(false);
    }

    public void RegisterEnemy(GameObject enemy)
    {
        if (enemy != null && activeEnemies.Add(enemy))
        {
            Debug.Log($"Enemy registered: {enemy.name}. Total enemies: {activeEnemies.Count}");
        }
    }

    public void DeregisterEnemy(GameObject enemy)
    {
        if (enemy != null && activeEnemies.Remove(enemy))
        {
            Debug.Log($"Enemy deregistered: {enemy.name}. Remaining enemies: {activeEnemies.Count}");
            if (activeEnemies.Count == 0)
            {
                Debug.Log("All enemies defeated. Player wins!");
                ShowOutcome(true);
            }
        }
    }

    public void ShowOutcome(bool didWin)
    {
        if (outcomeDisplayed) return;

        outcomeDisplayed = true;

        if (didWin)
        {
            PlayOutcomeMusic(victoryMusic);
            StartCoroutine(DelayedVictoryOutcome(victoryCameraPoint, victoryBadge, victoryAnimator, victoryAnimationName));
        }
        else
        {
            PlayOutcomeMusic(defeatMusic);
            PlayBadgeAnimation(defeatBadge, defeatAnimator, defeatAnimationName);
            MoveCameraToPoint(defeatCameraPoint);
        }

        StartCoroutine(AllowTransitionAfterDelay());
    }

    private IEnumerator DelayedVictoryOutcome(Transform targetPoint, GameObject badge, Animator animator, string animationName)
    {
        yield return new WaitForSeconds(3f); // Wait for 3 seconds
        MoveCameraToPoint(targetPoint);

        // Display the victory badge after the camera switch
        PlayBadgeAnimation(badge, animator, animationName);
    }


    private void PlayBadgeAnimation(GameObject badge, Animator animator, string animationName)
    {
        if (badge != null)
        {
            badge.SetActive(true);
            badge.transform.SetAsLastSibling();

            if (animator != null && !string.IsNullOrEmpty(animationName))
            {
                if (animator.HasState(0, Animator.StringToHash(animationName)))
                {
                    animator.CrossFade(animationName, 0.1f);
                }
                else
                {
                    Debug.LogWarning($"Animation state '{animationName}' not found in the Animator.");
                }
            }
        }
        else
        {
            Debug.LogWarning("Badge GameObject is not assigned!");
        }
    }

    private void PlayOutcomeMusic(AudioClip clip)
    {
        if (clip != null)
        {
            AudioManager.Instance?.PlayMusic(clip, AudioManager.MusicType.Game);
        }
        else
        {
            Debug.LogWarning("Music clip is not assigned!");
        }
    }

    private void MoveCameraToPoint(Transform targetPoint)
    {
        if (mainCamera != null && targetPoint != null)
        {
            mainCamera.transform.SetParent(null);
            mainCamera.transform.position = targetPoint.position;
            mainCamera.transform.rotation = targetPoint.rotation;
            Debug.Log("Camera moved to the specified point.");
        }
    }

    private IEnumerator AllowTransitionAfterDelay()
    {
        yield return new WaitForSeconds(transitionDelay);
        transitionAllowed = true;
    }

    private void Update()
    {
        if (outcomeDisplayed && transitionAllowed && Input.anyKeyDown)
        {
            RedirectToLevelSelection();
        }
    }

    private void RedirectToLevelSelection()
    {
        if (!string.IsNullOrEmpty(levelSelectionScene))
        {
            Debug.Log("Redirecting to Level Selection...");
            SceneManager.LoadScene(levelSelectionScene);
        }
        else
        {
            Debug.LogError("Level selection scene name is not set!");
        }
    }
}
