using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine.Audio; // 오디오 믹서를 사용하기 위해 추가

public class Cartoon_2 : MonoBehaviour
{
    [Header("📷 페이지 이미지 리스트")]
    public List<Image> cartoonPages = new List<Image>();

    [Header("🔊 페이지마다 재생할 사운드 리스트")]
    public List<AudioClip> cartoonSounds = new List<AudioClip>();

    [Header("🎵 카툰 전용 BGM")]
    public AudioClip cartoonBGM; // 인스펙터에서 BGM을 연결할 변수

    [Header("🕹️ 버튼 & 페이드")]
    public Button nextPageButton;
    public Button skipCartoonButton;
    public Image fadeOverlay;
    public float fadeTime = 1f;
    public string targetSceneName = "Game";

    // BGM과 SFX(효과음/목소리)를 위한 AudioSource를 명확히 분리합니다.
    private AudioSource bgmAudioSource;
    private AudioSource sfxAudioSource;

    private int cartoonPageIndex = 0;
    private float spaceCooldown = 1f;
    private float spaceTimer = 0f;

    void Awake()
    {
        // 1. 효과음(SFX)용 AudioSource를 생성하고 설정합니다.
        sfxAudioSource = gameObject.AddComponent<AudioSource>();
        sfxAudioSource.loop = false; // 효과음은 반복되면 안 됩니다.
        sfxAudioSource.playOnAwake = false;

        // 2. BGM용 AudioSource를 '별도로' 생성하고 설정합니다.
        bgmAudioSource = gameObject.AddComponent<AudioSource>();
        bgmAudioSource.loop = true; // BGM은 반복되어야 합니다.
        bgmAudioSource.playOnAwake = false;

        // 3. 생성된 AudioSource들을 올바른 오디오 믹서 그룹에 연결합니다.
        SetupMixerOutputs();
    }

    // AudioSource들의 출력을 올바른 믹서 그룹으로 보내는 함수
    void SetupMixerOutputs()
    {
        // VolumeManager가 없으면 실행하지 않습니다.
        if (VolumeManager.Instance == null || VolumeManager.Instance.audioMixer == null) return;

        // BGM 오디오 소스를 'BGMVolume' 그룹에 연결
        AudioMixerGroup[] bgmGroups = VolumeManager.Instance.audioMixer.FindMatchingGroups("BGMVolume");
        if (bgmGroups.Length > 0) bgmAudioSource.outputAudioMixerGroup = bgmGroups[0];

        // SFX 오디오 소스를 'SFXVolume' 그룹에 연결
        AudioMixerGroup[] sfxGroups = VolumeManager.Instance.audioMixer.FindMatchingGroups("SFXVolume");
        if (sfxGroups.Length > 0) sfxAudioSource.outputAudioMixerGroup = sfxGroups[0];
    }

    void Start()
    {
        ResetPages();

        // BGM 재생을 시작합니다.
        if (cartoonBGM != null)
        {
            bgmAudioSource.clip = cartoonBGM;
            bgmAudioSource.Play();
        }

        // 첫 페이지를 보여주고 첫 효과음을 재생합니다.
        if (cartoonPages.Count > 0)
        {
            cartoonPages[0].gameObject.SetActive(true);
            cartoonPages[0].DOFade(1f, fadeTime).SetUpdate(true);
            PlaySoundForPage(0);
        }

        nextPageButton?.onClick.AddListener(ShowNextPage);
        skipCartoonButton?.onClick.AddListener(LoadTargetSceneWithFade);

        if (fadeOverlay != null)
        {
            fadeOverlay.color = new Color(0, 0, 0, 0);
            fadeOverlay.gameObject.SetActive(false);
        }
    }

    void Update()
    {
        spaceTimer += Time.unscaledDeltaTime;

        if (Input.GetKeyDown(KeyCode.Space) && spaceTimer >= spaceCooldown)
        {
            spaceTimer = 0f;
            ShowNextPage();
        }
    }

    private void ResetPages()
    {
        cartoonPageIndex = 0;
        foreach (var img in cartoonPages)
        {
            if (img != null)
            {
                img.gameObject.SetActive(false);
                img.color = new Color(1, 1, 1, 0);
            }
        }
    }

    // 효과음/목소리는 이제 SFX 전용 AudioSource에서만 재생됩니다.
    private void PlaySoundForPage(int index)
    {
        sfxAudioSource.Stop();

        if (index >= 0 && index < cartoonSounds.Count && cartoonSounds[index] != null)
        {
            sfxAudioSource.clip = cartoonSounds[index];
            sfxAudioSource.ignoreListenerPause = true;
            sfxAudioSource.Play();
        }
    }

    private void ShowNextPage()
    {
        cartoonPageIndex++;

        if (cartoonPageIndex < cartoonPages.Count)
        {
            var nextImg = cartoonPages[cartoonPageIndex];
            nextImg.gameObject.SetActive(true);
            nextImg.color = new Color(1, 1, 1, 0);
            nextImg.DOFade(1f, fadeTime).SetUpdate(true);
            PlaySoundForPage(cartoonPageIndex);
        }
        else
        {
            LoadTargetSceneWithFade();
        }
    }

    private void LoadTargetSceneWithFade()
    {
        // BGM을 부드럽게 멈춥니다.
        if (bgmAudioSource.isPlaying)
        {
            bgmAudioSource.DOFade(0, fadeTime).SetUpdate(true);
        }
        // 효과음도 멈춥니다.
        sfxAudioSource.Stop();

        if (fadeOverlay != null)
        {
            fadeOverlay.gameObject.SetActive(true);
            fadeOverlay.transform.SetAsLastSibling();
            fadeOverlay.DOFade(1f, fadeTime).SetUpdate(true).OnComplete(() =>
            {
                Time.timeScale = 1f;
                SceneManager.LoadScene(targetSceneName);
            });
        }
        else
        {
            Time.timeScale = 1f;
            SceneManager.LoadScene(targetSceneName);
        }
    }
}