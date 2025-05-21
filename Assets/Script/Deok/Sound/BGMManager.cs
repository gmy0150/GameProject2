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
        // 씬 시작 시 BGM 결정
        OnSceneLoaded(SceneManager.GetActiveScene(), LoadSceneMode.Single);
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("씬 로드됨: " + scene.name);

        // 🎯 BGM 지정된 씬
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
            // 🎯 그 외 씬에서는 부드럽게 BGM 정지
            Debug.Log("[BGMManager] 이 씬에서는 BGM 없음. 페이드 아웃 중지.");
            FadeToBGM(null);
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
            float startVolume = bgmAudioSource.volume;

            for (float t = 0; t < fadeDuration; t += Time.deltaTime)
            {
                bgmAudioSource.volume = Mathf.Lerp(startVolume, 0f, t / fadeDuration);
                yield return null;
            }

            bgmAudioSource.volume = 0f;
            bgmAudioSource.Stop();
        }

        // 🎯 newClip이 null이면 BGM 중단 후 종료
        if (newClip == null)
        {
            bgmAudioSource.clip = null;
            fadeCoroutine = null;
            yield break;
        }

        // 🎯 새 클립 재생
        bgmAudioSource.clip = newClip;
        bgmAudioSource.Play();

        for (float t = 0; t < fadeDuration; t += Time.deltaTime)
        {
            bgmAudioSource.volume = Mathf.Lerp(0f, 1f, t / fadeDuration);
            yield return null;
        }

        bgmAudioSource.volume = 1f;
        fadeCoroutine = null;
    }
}
