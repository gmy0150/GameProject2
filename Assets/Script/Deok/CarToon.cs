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

    [Header("🎵 카툰 전용 BGM")]
    public AudioClip cartoonBGM;

    private AudioSource bgmAudioSource; 
    private AudioSource sfxAudioSource;  

    [Header("🕹️ 버튼 & 페이드")]
    public Button nextButton;
    public Button skipButton;
    public Image fadePanel;
    public float fadeDuration = 1f;
    public string nextSceneName = "Game";

    private int currentPage = 0;
    
    private float spaceCooldown = 1f;
    private float spaceTimer = 0f;

    void Awake()
    {
        sfxAudioSource = gameObject.AddComponent<AudioSource>();
        sfxAudioSource.loop = false;
        sfxAudioSource.playOnAwake = false;

        bgmAudioSource = gameObject.AddComponent<AudioSource>();
        bgmAudioSource.loop = true;
        bgmAudioSource.playOnAwake = false;

        SetupMixerOutputs();
    }
    
    void SetupMixerOutputs()
    {
        if (VolumeManager.Instance == null || VolumeManager.Instance.audioMixer == null) return;
        
        AudioMixerGroup[] bgmGroups = VolumeManager.Instance.audioMixer.FindMatchingGroups("BGMVolume");
        if (bgmGroups.Length > 0) bgmAudioSource.outputAudioMixerGroup = bgmGroups[0];

        AudioMixerGroup[] sfxGroups = VolumeManager.Instance.audioMixer.FindMatchingGroups("SFXVolume");
        if (sfxGroups.Length > 0) sfxAudioSource.outputAudioMixerGroup = sfxGroups[0];
    }


    void Start()
    {
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

    void OnDestroy()
    {
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
                
                Destroy(gameObject); 
            });
        }
        else
        {
            if (!isStart)
                GameManager.Instance.MainGameStart();
            else
                GameManager.Instance.OnAilionDiaglogueEnd();
            
            Destroy(gameObject);
        }
    }
}