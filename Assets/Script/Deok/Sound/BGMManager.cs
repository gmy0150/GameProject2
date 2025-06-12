using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class BGMManager : MonoBehaviour
{
    // ▼▼▼ [수정] 이 부분이 바로 싱글톤(Singleton) 코드입니다. ▼▼▼
    public static BGMManager Instance { get; private set; }
    
    public AudioSource bgmAudioSource;

    [Header("BGM Clips")]
    public AudioClip mainMenuBGM;
    public AudioClip inGameBGM;

    [Header("Fade Settings")]
    public float fadeDuration = 1.5f;

    private Coroutine fadeCoroutine;
    private bool isTemporarilyStopped = false; // 카툰 등에 의해 BGM이 일시 정지되었는지 확인

    private void Awake()
    {
        // ▼▼▼ [수정] 싱글톤 인스턴스를 설정하는 코드입니다. ▼▼▼
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
            // 이미 BGMManager가 존재하면 새로 생긴 것은 파괴합니다.
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
        // 씬이 로드될 때, BGM이 일시정지 상태가 아니라면 씬에 맞는 BGM을 재생합니다.
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

    // 카툰이 시작될 때 이 함수를 호출하여 메인 BGM을 멈춥니다.
    public void TemporarilyStopBGM()
    {
        if (isTemporarilyStopped) return;
        isTemporarilyStopped = true;
        FadeToBGM(null);
    }

    // 카툰이 끝날 때 이 함수를 호출하여 씬에 맞는 BGM을 다시 시작합니다.
    public void ResumeSceneBGM()
    {
        if (!isTemporarilyStopped) return;
        isTemporarilyStopped = false;
        // 현재 씬을 다시 확인하여 적절한 BGM을 재생합니다.
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
            for (float t = 0; t < fadeDuration; t += Time.deltaTime)
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
        for (float t = 0; t < fadeDuration; t += Time.deltaTime)
        {
            bgmAudioSource.volume = Mathf.Lerp(0f, targetVolume, t / fadeDuration);
            yield return null;
        }
        bgmAudioSource.volume = targetVolume;
        fadeCoroutine = null;
    }
}