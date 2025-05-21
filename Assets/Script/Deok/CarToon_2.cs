using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using DG.Tweening;

public class Cartoon_2 : MonoBehaviour
{
    [Header("📷 페이지 이미지 리스트")]
    public List<Image> cartoonPages = new List<Image>();

    [Header("🔊 페이지마다 재생할 사운드 리스트")]
    public List<AudioClip> cartoonSounds = new List<AudioClip>();

    [Header("🕹️ 버튼 & 페이드")]
    public Button nextPageButton;
    public Button skipCartoonButton;
    public Image fadeOverlay;
    public float fadeTime = 1f;
    public string targetSceneName = "Game";

    private int cartoonPageIndex = 0;
    private AudioSource cartoonAudio;

    void Start()
    {
        SetupAudioSource();
        ResetPages();

        // 첫 페이지 보여주기
        if (cartoonPages.Count > 0)
        {
            cartoonPages[0].gameObject.SetActive(true);
            cartoonPages[0].DOFade(1f, fadeTime);
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

    private void SetupAudioSource()
    {
        cartoonAudio = GetComponent<AudioSource>();
        if (cartoonAudio == null)
        {
            cartoonAudio = gameObject.AddComponent<AudioSource>();
        }
    }

    private void ResetPages()
    {
        cartoonPageIndex = 0;

        foreach (var img in cartoonPages)
        {
            if (img == null) continue;
            img.gameObject.SetActive(false);
            img.color = new Color(1, 1, 1, 0);
        }
    }

    private void PlaySoundForPage(int index)
    {
        if (cartoonAudio.isPlaying) cartoonAudio.Stop();

        if (index >= 0 && index < cartoonSounds.Count && cartoonSounds[index] != null)
        {
            cartoonAudio.clip = cartoonSounds[index];
            cartoonAudio.Play();
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
            nextImg.DOFade(1f, fadeTime);

            PlaySoundForPage(cartoonPageIndex);
        }
        else
        {
            LoadTargetSceneWithFade();
        }
    }

    private void LoadTargetSceneWithFade()
    {
        if (fadeOverlay != null)
        {
            fadeOverlay.gameObject.SetActive(true);
            fadeOverlay.transform.SetAsLastSibling();

            fadeOverlay.DOFade(1f, fadeTime).SetUpdate(true).OnComplete(() =>
            {
                SceneManager.LoadScene(targetSceneName);
            });
        }
        else
        {
            SceneManager.LoadScene(targetSceneName);
        }
    }
}
