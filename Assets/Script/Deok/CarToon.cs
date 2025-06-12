using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine.Audio; // 오디오 믹서를 사용하기 위해 추가

public class Cartoon : MonoBehaviour
{
    [Header("📷 페이지 이미지 리스트")]
    public List<Image> pages = new List<Image>();

    [Header("🔊 페이지마다 재생할 사운드 리스트")]
    public List<AudioClip> pageSounds = new List<AudioClip>();

    // ▼▼▼ [수정] 오류가 나지 않도록 이 부분에 변수들을 모두 선언(정의)합니다. ▼▼▼
    [Header("🎵 카툰 전용 BGM")]
    public AudioClip cartoonBGM;

    private AudioSource bgmAudioSource;   // BGM 재생용
    private AudioSource sfxAudioSource;   // 효과음/목소리 재생용

    [Header("🕹️ 버튼 & 페이드")]
    public Button nextButton;
    public Button skipButton;
    public Image fadePanel;
    public float fadeDuration = 1f;
    public string nextSceneName = "Game";

    private int currentPage = 0;
    
    private float spaceCooldown = 1f;
    private float spaceTimer = 0f;

    // ▼▼▼ [수정] Awake 함수를 추가하여 AudioSource들을 설정합니다. ▼▼▼
    void Awake()
    {
        // 효과음(SFX)용 AudioSource를 생성하고 설정합니다.
        sfxAudioSource = gameObject.AddComponent<AudioSource>();
        sfxAudioSource.loop = false;
        sfxAudioSource.playOnAwake = false;

        // BGM용 AudioSource를 '별도로' 생성하고 설정합니다.
        bgmAudioSource = gameObject.AddComponent<AudioSource>();
        bgmAudioSource.loop = true;
        bgmAudioSource.playOnAwake = false;

        // 생성된 AudioSource들을 올바른 오디오 믹서 그룹에 연결합니다.
        SetupMixerOutputs();
    }
    
    // AudioSource들의 출력을 올바른 믹서 그룹으로 보내는 함수
    void SetupMixerOutputs()
    {
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
        // 카툰이 시작되면 메인 BGM을 멈추고, 이 카툰의 BGM을 재생합니다.
        BGMManager.Instance?.TemporarilyStopBGM();
        if (cartoonBGM != null)
        {
            bgmAudioSource.clip = cartoonBGM;
            bgmAudioSource.Play();
        }

        // 기존 코드 시작
        ResetCartoonState();

        if (pages.Count > 0)
        {
            pages[0].gameObject.SetActive(true);
            pages[0].DOFade(1f, fadeDuration);
            PlayPageSound(0);
        }

        nextButton.onClick.AddListener(NextPage);
        if (skipButton != null)
        {
            skipButton.onClick.AddListener(SkipToScene);
        }

        if (fadePanel != null)
        {
            fadePanel.color = new Color(0, 0, 0, 0);
            fadePanel.gameObject.SetActive(false);
        }
    }

    // ▼▼▼ [수정] OnDestroy 함수를 추가하여 카툰이 끝날 때 BGM을 복구합니다. ▼▼▼//
    void OnDestroy()
    {
        // 카툰 오브젝트가 파괴될 때, 씬에 맞는 메인 BGM을 다시 시작하도록 요청합니다.
        BGMManager.Instance?.ResumeSceneBGM();
    }


    void Update()
    {
        spaceTimer += Time.unscaledDeltaTime;
        if (Input.GetKeyDown(KeyCode.Space) && spaceTimer >= spaceCooldown)
        {
            spaceTimer = 0f;
            NextPage();
        }
    }

    private void ResetCartoonState()
    {
        currentPage = 0;

        foreach (var img in pages)
        {
            if (img == null) continue;
            img.gameObject.SetActive(false);
            img.color = new Color(1, 1, 1, 0);
        }
    }

    private void PlayPageSound(int index)
    {
        // 효과음은 이제 sfxAudioSource에서만 재생됩니다.
        sfxAudioSource.Stop();

        if (index >= 0 && index < pageSounds.Count && pageSounds[index] != null)
        {
            sfxAudioSource.clip = pageSounds[index];
            sfxAudioSource.Play();
        }
    }

    private void NextPage()
    {
        currentPage++;

        if (currentPage < pages.Count)
        {
            var nextImage = pages[currentPage];
            nextImage.gameObject.SetActive(true);
            nextImage.color = new Color(1, 1, 1, 0);
            nextImage.DOFade(1f, fadeDuration);

            PlayPageSound(currentPage);
        }
        else
        {
            StartFadeAndLoadScene();
        }
    }

    private void SkipToScene()
    {
        StartFadeAndLoadScene();
    }

    public bool isStart = false;
    private void StartFadeAndLoadScene()
    {
        // 카툰이 끝나기 전에 BGM과 효과음을 멈춥니다.
        bgmAudioSource?.DOFade(0, fadeDuration).SetUpdate(true);
        sfxAudioSource?.Stop();
        
        if (fadePanel != null)
        {
            fadePanel.gameObject.SetActive(true);
            fadePanel.transform.SetAsLastSibling();

            fadePanel.DOFade(1f, fadeDuration).SetUpdate(true).OnComplete(() =>
            {
                if (!isStart)
                    GameManager.Instance.MainGameStart();
                else
                    GameManager.Instance.OnAilionDiaglogueEnd();
                
                // OnDestroy가 호출되도록 gameObject 자체를 파괴합니다.
                Destroy(gameObject); 
            });
        }
        else
        {
            if (!isStart)
                GameManager.Instance.MainGameStart();
            else
                GameManager.Instance.OnAilionDiaglogueEnd();
            
            // OnDestroy가 호출되도록 gameObject 자체를 파괴합니다.
            Destroy(gameObject);
        }
    }
}