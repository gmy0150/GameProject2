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

    // ✅ Spacebar 연속 입력 방지용
    private float spaceCooldown = 1f;
    private float spaceTimer = 0f;

    void Start()
    {
        SetupAudioSource();
        ResetPages();

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
            nextPageButton?.onClick.Invoke();
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
            cartoonAudio.ignoreListenerPause = true;
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
