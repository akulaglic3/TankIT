using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    private AudioSource musicSource;
    private AudioSource sfxSource;

    [Header("Audio Clips")]
    public AudioClip mainMenuMusic;
    public AudioClip loadingScreenMusic;
    public AudioClip gameAmbientMusic;

    [Header("Settings")]
    [Range(0f, 1f)] public float defaultVolumeMainMenuMusicVolume = 0.2f;
    [Range(0f, 1f)] public float defaultVolumeGameSFXVolume = 0.3f;
    [Range(0f, 1f)] public float defaultVolumeGameMusicVolume = 0.8f;

    public enum MusicType { MainMenu, Game }
    private MusicType currentMusicType;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            musicSource = gameObject.AddComponent<AudioSource>();
            musicSource.loop = true;
            musicSource.playOnAwake = false;

            sfxSource = gameObject.AddComponent<AudioSource>();
            sfxSource.loop = false;
            sfxSource.playOnAwake = false;

            // Load initial volumes
            SetMainMenuMusicVolume(PlayerPrefs.GetFloat("MainMenuMusicVolume", defaultVolumeMainMenuMusicVolume));
            SetSFXVolume(PlayerPrefs.GetFloat("GameSFXVolume", defaultVolumeGameSFXVolume));
            SetGameMusicVolume(PlayerPrefs.GetFloat("GameMusicVolume", defaultVolumeGameMusicVolume));

            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        string sceneName = scene.name;

        if (IsMainMenuGroup(sceneName))
        {
            PlayMainMenuMusic();
        }
        else if (sceneName == "LoadingScreen")
        {
            PlayMusic(loadingScreenMusic, MusicType.MainMenu);
        }
        else if (sceneName.StartsWith("Level"))
        {
            PlayGameMusic();
        }
    }

    private bool IsMainMenuGroup(string sceneName)
    {
        return sceneName == "MainMenu" || sceneName == "SettingsMenu" || sceneName == "LevelSelect";
    }

    public void PlayMainMenuMusic()
    {
        PlayMusic(mainMenuMusic, MusicType.MainMenu);
    }

    public void PlayGameMusic()
    {
        PlayMusic(gameAmbientMusic, MusicType.Game);
    }

    public void PlayMusic(AudioClip clip, MusicType musicType)
    {
        if (musicSource.clip != clip || currentMusicType != musicType)
        {
            currentMusicType = musicType;
            musicSource.clip = clip;

            // Apply appropriate volume
            musicSource.volume = musicType == MusicType.MainMenu
                ? PlayerPrefs.GetFloat("MainMenuMusicVolume", defaultVolumeMainMenuMusicVolume)
                : PlayerPrefs.GetFloat("GameMusicVolume", defaultVolumeGameMusicVolume);

            musicSource.Play();
        }
    }


    public void SetMainMenuMusicVolume(float volume)
    {
        if (currentMusicType == MusicType.MainMenu)
        {
            musicSource.volume = volume;
        }

        PlayerPrefs.SetFloat("MainMenuMusicVolume", volume);
        PlayerPrefs.Save();
    }

    public void PauseMusic()
    {
        if (musicSource.isPlaying)
        {
            musicSource.Pause();
        }
    }

    public void ResumeMusic()
    {
        if (!musicSource.isPlaying)
        {
            musicSource.UnPause();
        }
    }

    public void SetGameMusicVolume(float volume)
    {
        if (currentMusicType == MusicType.Game)
        {
            musicSource.volume = volume;
        }

        PlayerPrefs.SetFloat("GameMusicVolume", volume);
        PlayerPrefs.Save();
    }

    public void SetSFXVolume(float volume)
    {
        sfxSource.volume = volume;
        PlayerPrefs.SetFloat("GameSFXVolume", volume);
        PlayerPrefs.Save();
    }

    public void PlaySFX(AudioClip clip, float volumeScale = 1f)
    {
        if (clip != null)
        {
            sfxSource.PlayOneShot(clip, volumeScale * sfxSource.volume);
        }
    }

}
