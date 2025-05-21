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
    public Button skipButton;           // ✅ Skip 버튼 추가
    public Image fadePanel;
    public float fadeDuration = 1f;
    public string nextSceneName = "Game";

    private int currentPage = 0;
    private AudioSource audioSource;

    void Start()
    {
        InitAudioSource();
        ResetCartoonState();

        // 첫 페이지 표시 & 사운드 재생
        if (pages.Count > 0)
        {
            pages[0].gameObject.SetActive(true);
            pages[0].DOFade(1f, fadeDuration);
            PlayPageSound(0);
        }

        nextButton.onClick.AddListener(NextPage);

        if (skipButton != null)
        {
            skipButton.onClick.AddListener(SkipToScene); // ✅ 스킵 버튼 연결
        }

        if (fadePanel != null)
        {
            fadePanel.color = new Color(0, 0, 0, 0);
            fadePanel.gameObject.SetActive(false);
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
            img.color = new Color(1, 1, 1, 0); // 투명 초기화
        }
    }

    private void PlayPageSound(int index)
    {
        if (audioSource.isPlaying)
        {
            audioSource.Stop(); // 이전 사운드 정지
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
        StartFadeAndLoadScene(); // ✅ Skip도 Fade 후 씬 전환
    }

    private void StartFadeAndLoadScene()
    {
        if (fadePanel != null)
        {
            fadePanel.gameObject.SetActive(true);
            fadePanel.transform.SetAsLastSibling();

            fadePanel.DOFade(1f, fadeDuration).SetUpdate(true).OnComplete(() =>
            {
                SceneManager.LoadScene(nextSceneName);
            });
        }
        else
        {
            SceneManager.LoadScene(nextSceneName);
        }
    }
}
