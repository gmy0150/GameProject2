using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class BGMManager : MonoBehaviour
{
    public static BGMManager Instance { get; private set; }
    
    public AudioSource bgmAudioSource;

    [Header("BGM Clips")]
    public AudioClip mainMenuBGM;
    public AudioClip inGameBGM;

    [Header("Fade Settings")]
    public float fadeDuration = 1.5f;

    private Coroutine fadeCoroutine;
    private bool isTemporarilyStopped = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            if (bgmAudioSource == null)
                bgmAudioSource = GetComponent<AudioSource>();
            DontDestroyOnLoad(gameObject);
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        OnSceneLoaded(SceneManager.GetActiveScene(), LoadSceneMode.Single);
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (isTemporarilyStopped) return;

        if (scene.name == "MainMenu" || scene.name == "Test_MainMenu")
        {
            FadeToBGM(mainMenuBGM);
        }
        else if (scene.name == "Game" || scene.name == "Test_Game")
        {
            FadeToBGM(inGameBGM);
        }
        else
        {
            FadeToBGM(null);
        }
    }

    public void TemporarilyStopBGM()
    {
        if (isTemporarilyStopped) return;
        isTemporarilyStopped = true;
        FadeToBGM(null);
    }

    public void ResumeSceneBGM()
    {
        if (!isTemporarilyStopped) return;
        isTemporarilyStopped = false;
        OnSceneLoaded(SceneManager.GetActiveScene(), LoadSceneMode.Single);
    }

    public void FadeToBGM(AudioClip newClip)
    {
        if (fadeCoroutine != null)
        {
            StopCoroutine(fadeCoroutine);
        }
        fadeCoroutine = StartCoroutine(FadeBGM(newClip));
    }

    private IEnumerator FadeBGM(AudioClip newClip)
    {
        if (bgmAudioSource.isPlaying)
        {
            float startVolume = bgmAudioSource.volume;
            for (float t = 0; t < fadeDuration; t += Time.unscaledDeltaTime)
            {
                bgmAudioSource.volume = Mathf.Lerp(startVolume, 0f, t / fadeDuration);
                yield return null;
            }
            bgmAudioSource.volume = 0f;
            bgmAudioSource.Stop();
        }

        if (newClip == null)
        {
            bgmAudioSource.clip = null;
            fadeCoroutine = null;
            yield break;
        }

        bgmAudioSource.clip = newClip;
        bgmAudioSource.Play();
        float targetVolume = 1f;
        for (float t = 0; t < fadeDuration; t += Time.unscaledDeltaTime)
        {
            bgmAudioSource.volume = Mathf.Lerp(0f, targetVolume, t / fadeDuration);
            yield return null;
        }
        bgmAudioSource.volume = targetVolume;
        fadeCoroutine = null;
    }
}