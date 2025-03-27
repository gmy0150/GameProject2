using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class UIOptionMenu : MonoBehaviour
{
    public RectTransform optionPanel;
    public CanvasGroup canvasGroup;
    public Button optionButton;
    public Button backButton;

    private bool isAnimating = false;

    private Vector2 hiddenPosition;
    private Vector2 visiblePosition = new Vector2(0, 0); 

    void Start()
    {
        hiddenPosition = new Vector2(optionPanel.rect.width, 0);
        optionPanel.anchoredPosition = hiddenPosition;
        canvasGroup.alpha = 0f;
    }

    public void OpenOptionMenu()
    {
        if (isAnimating) return; 
        isAnimating = true;

        // 애니메이션 나오는 중 버튼 입력 방지함.
        optionButton.interactable = false;
        backButton.interactable = false;

        optionPanel.gameObject.SetActive(true);
        optionPanel.DOAnchorPos(visiblePosition, 1f).SetEase(Ease.OutExpo);
        // 0.5f 시간늘리면 천천히 더 가능함.
        canvasGroup.DOFade(1f, 1f).OnComplete(() => 
        {
            isAnimating = false;
            optionButton.interactable = true;
            backButton.interactable = true;
        });
    }

    public void CloseOptionMenu()
    {
        if (isAnimating) return; // 애니메이션 나오는 중 버튼 입력 방지함.
        isAnimating = true;

        optionButton.interactable = false;
        backButton.interactable = false;

        optionPanel.DOAnchorPos(hiddenPosition, 0.5f).SetEase(Ease.InExpo);
        canvasGroup.DOFade(0f, 0.5f).OnComplete(() =>
        {
            optionPanel.gameObject.SetActive(false);
            isAnimating = false;
            optionButton.interactable = true;
            backButton.interactable = true;
        });
    }
}
