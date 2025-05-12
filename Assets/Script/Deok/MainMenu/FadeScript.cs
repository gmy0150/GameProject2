using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class FadeScript : MonoBehaviour
{
    [Header("🔲 페이드용 패널 이미지 (검정 이미지)")]
    public Image panelImage;

    [Header("⏱️ 페이드 지속 시간")]
    public float fadeDuration = 1f;

    private void Awake()
    {
        DG.Tweening.DOTween.Init();
    }

    private void Start()
    {
        if (panelImage == null)
        {
            Debug.LogError("FadeScript 오류: panelImage가 할당되지 않았습니다!");
            return;
        }

        // 시작 시 화면이 검게 → 투명하게 페이드 아웃
        panelImage.color = new Color(0, 0, 0, 1);
        panelImage.DOFade(0f, fadeDuration).SetUpdate(true);
    }

    public void FadeToScene(string sceneName)
    {
        Debug.Log("Fade 시작 → 대상 씬: " + sceneName);

        panelImage.gameObject.SetActive(true);
        panelImage.color = new Color(0, 0, 0, 0); // 투명 상태로 시작

        panelImage.DOFade(1f, fadeDuration)
            .SetUpdate(true)
            .OnComplete(() =>
            {
                Debug.Log("Fade 완료 → 씬 전환 시도");
                SceneManager.LoadScene(sceneName);
            });
    }
}
