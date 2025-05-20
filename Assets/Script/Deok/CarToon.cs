using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using DG.Tweening;

public class Cartoon : MonoBehaviour
{
    public List<Image> pages = new List<Image>();
    public Button nextButton;
    public Image fadePanel;
    public float fadeDuration = 1f;
    public string nextSceneName = "Game";

    private int currentPage = 0;

    void Start()
    {
        GameManager.Instance.ActPlay(false);
        ResetCartoonState(); // 상태 초기화

        if (pages.Count > 0)
        {
            pages[0].gameObject.SetActive(true);
            pages[0].DOFade(1f, fadeDuration);
        }

        nextButton.onClick.AddListener(NextPage);
        Debug.Log("작동?");
        Debug.Log(nextButton);
        if (fadePanel != null)
        {
            fadePanel.color = new Color(0, 0, 0, 0);
            fadePanel.gameObject.SetActive(false);
        }
    }
    

    private void ResetCartoonState()
    {
        currentPage = 0;

        foreach (var img in pages)
        {
            if (img == null) continue;

            img.gameObject.SetActive(false);
            img.color = new Color(1, 1, 1, 0); // 투명한 상태로 초기화
        }
    }

    void NextPage()
    {
        currentPage++;

        if (currentPage < pages.Count)
        {
            var nextImage = pages[currentPage];
            nextImage.gameObject.SetActive(true);
            nextImage.color = new Color(1, 1, 1, 0); // 투명하게 시작
            nextImage.DOFade(1f, fadeDuration);     // 부드럽게 등장
        }
        else
        {
            StartFadeAndLoadScene();
        }
    }

    void StartFadeAndLoadScene()
    {
        if (fadePanel != null)
        {
            fadePanel.gameObject.SetActive(true);
            fadePanel.transform.SetAsLastSibling();

            fadePanel.DOFade(1f, fadeDuration).SetUpdate(true).OnComplete(() =>
            {
                GameManager.Instance.MainGameStart();
                // SceneManager.LoadScene(nextSceneName);
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
