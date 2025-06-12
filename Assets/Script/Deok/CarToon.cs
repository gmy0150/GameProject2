using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using DG.Tweening;

public class Cartoon : MonoBehaviour
{
    [Header("📷 페이지 이미지 리스트")]
    public List<Image> pages = new List<Image>();

    [Header("🔊 페이지마다 재생할 사운드 리스트")]
    public List<AudioClip> pageSounds = new List<AudioClip>();

    [Header("🕹️ 버튼 & 페이드")]
    public Button nextButton;
    public Button skipButton;
    public Image fadePanel;
    public float fadeDuration = 1f;
    public string nextSceneName = "Game";

    private int currentPage = 0;
    private AudioSource audioSource;

    // ✅ Space 입력 쿨타임
    private float spaceCooldown = 1f;
    private float spaceTimer = 0f;

    void Start()
    {
        InitAudioSource();
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

    void Update()
    {
        spaceTimer += Time.unscaledDeltaTime;
        if (Input.GetKeyDown(KeyCode.Space) && spaceTimer >= spaceCooldown)
        {
            spaceTimer = 0f;
            nextButton?.onClick.Invoke(); // ✅ Next 버튼처럼 작동
        }
    }

    private void InitAudioSource()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
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
        if (audioSource.isPlaying)
        {
            audioSource.Stop();
        }

        if (index >= 0 && index < pageSounds.Count && pageSounds[index] != null)
        {
            audioSource.clip = pageSounds[index];
            audioSource.Play();
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

                transform.gameObject.SetActive(false);
                Destroy(this);
            });
        }
        else
        {
            transform.gameObject.SetActive(false);
            Destroy(this);
        }
    }
}
