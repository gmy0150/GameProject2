using UnityEngine;
using DG.Tweening;

public class OptionPanelController : MonoBehaviour
{
    [Header("옵션창 페이드용 캔버스 그룹")]
    public CanvasGroup optionCanvasGroup;

    [Header("페이드 시간")]
    public float fadeDuration = 0.5f;

    private bool isOptionsVisible = false;

    private void Start()
    {
        // 옵션창을 꺼둔 상태로 초기화 (안보이게)
        optionCanvasGroup.alpha = 0f;
        optionCanvasGroup.gameObject.SetActive(false);
    }

    public void ToggleOptions()
    {
        if (isOptionsVisible)
            HideOptions();
        else
            ShowOptions();
    }

    public void ShowOptions()
    {
        optionCanvasGroup.gameObject.SetActive(true);
        optionCanvasGroup.DOFade(1f, fadeDuration).SetUpdate(true);
        isOptionsVisible = true;
    }

    public void HideOptions()
    {
        optionCanvasGroup.DOFade(0f, fadeDuration).SetUpdate(true)
            .OnComplete(() =>
            {
                optionCanvasGroup.gameObject.SetActive(false);
            });
        isOptionsVisible = false;
    }
}
