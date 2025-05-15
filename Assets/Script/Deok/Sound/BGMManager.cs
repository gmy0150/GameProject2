using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class BGMManager : MonoBehaviour
{
    public AudioSource bgmAudioSource;

    [Header("BGM Clips")]
    public AudioClip mainMenuBGM;
    public AudioClip inGameBGM;

    [Header("Fade Settings")]
    public float fadeDuration = 1.5f;

    private Coroutine fadeCoroutine;

    private void Awake()
    {
        if (bgmAudioSource == null)
            bgmAudioSource = GetComponent<AudioSource>();

        DontDestroyOnLoad(gameObject);
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void Start()
    {
        // 씬 시작 시 강제 호출
        OnSceneLoaded(SceneManager.GetActiveScene(), LoadSceneMode.Single);
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("씬 로드됨: " + scene.name);

        if (scene.name == "MainMenu" || scene.name == "Test_MainMenu")
        {
            FadeToBGM(mainMenuBGM);
        }
        else if (scene.name == "GameScene" || scene.name == "Test_Game")
        {
            FadeToBGM(inGameBGM);
        }
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
            // 페이드 아웃
            float startVolume = bgmAudioSource.volume;

            for (float t = 0; t < fadeDuration; t += Time.deltaTime)
            {
                bgmAudioSource.volume = Mathf.Lerp(startVolume, 0f, t / fadeDuration);
                yield return null;
            }

            bgmAudioSource.volume = 0f;
            bgmAudioSource.Stop();
        }

        // 새 클립으로 교체 후 페이드 인
        bgmAudioSource.clip = newClip;

        if (newClip != null)
        {
            bgmAudioSource.Play();

            for (float t = 0; t < fadeDuration; t += Time.deltaTime)
            {
                bgmAudioSource.volume = Mathf.Lerp(0f, 1f, t / fadeDuration);
                yield return null;
            }

            bgmAudioSource.volume = 1f;
        }

        fadeCoroutine = null;
    }
}
