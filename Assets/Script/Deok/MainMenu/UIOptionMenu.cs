using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class UIOptionMenu : MonoBehaviour
{
    [Header("옵션 메뉴 패널")]
    public RectTransform optionPanel;
    public CanvasGroup canvasGroup;
    public Button optionButton;
    public Button backButton;

    [Header("기능 버튼")]
    public Button soundButton;
    public Button screenButton;

    [Header("사운드 패널")]
    public GameObject soundPanel;
    public RectTransform soundPanelRect;
    public CanvasGroup soundCanvasGroup;

    [Header("화면 패널")]
    public GameObject screenPanel;
    public RectTransform screenPanelRect;
    public CanvasGroup screenCanvasGroup;

    [Header("볼륨 매니저 연결")]
    public VolumeManager volumeManager;

    [Header("해상도 설정 스크립트")]
    public ScreenSettingsManager screenSettingsManager;

    public GameObject mainMenuPanel;

    private bool isAnimating = false;
    private Vector2 hiddenPosition;
    private Vector2 visiblePosition = Vector2.zero;

    void Start()
    {
        hiddenPosition = new Vector2(optionPanel.rect.width, 0);
        optionPanel.anchoredPosition = hiddenPosition;
        canvasGroup.alpha = 0f;

        soundPanel.SetActive(false);
        screenPanel.SetActive(false);

        soundPanelRect.anchoredPosition = new Vector2(soundPanelRect.rect.width, 0);
        soundCanvasGroup.alpha = 0f;

        screenPanelRect.anchoredPosition = new Vector2(screenPanelRect.rect.width, 0);
        screenCanvasGroup.alpha = 0f;
    }

    public void OpenOptionMenu()
    {
        if (isAnimating) return;
        isAnimating = true;

        optionButton.interactable = false;
        backButton.interactable = false;

        optionPanel.gameObject.SetActive(true);
        optionPanel.DOAnchorPos(visiblePosition, 1f).SetEase(Ease.OutExpo);
        canvasGroup.DOFade(1f, 1f).OnComplete(() =>
        {
            isAnimating = false;
            DOVirtual.DelayedCall(0.01f, () =>
            {
                optionButton.interactable = true;
                backButton.interactable = true;
            });
        });
    }

    public void CloseOptionMenu()
    {
        if (isAnimating) return;
        isAnimating = true;

        optionButton.interactable = false;
        backButton.interactable = false;

        optionPanel.DOAnchorPos(hiddenPosition, 0.1f).SetEase(Ease.InExpo);
        canvasGroup.DOFade(0f, 0.1f).OnComplete(() =>
        {
            optionPanel.gameObject.SetActive(false);
            isAnimating = false;
            optionButton.interactable = true;
            backButton.interactable = true;
        });

        soundPanel.SetActive(false);
        screenPanel.SetActive(false);
    }

    public void OpenSoundPanel()
    {
        soundPanel.SetActive(true);
        screenPanel.SetActive(false);

        soundPanelRect.anchoredPosition = new Vector2(soundPanelRect.rect.width, 0);
        soundCanvasGroup.alpha = 0f;

        soundPanelRect.DOAnchorPos(Vector2.zero, 0.5f).SetEase(Ease.OutExpo);
        soundCanvasGroup.DOFade(1f, 0.5f);
    }

    public void ConfirmSoundPanel()
    {
        if (isAnimating) return;
        isAnimating = true;

        volumeManager.ApplyAll(); // 🎵 사운드 적용

        soundPanelRect.DOAnchorPos(new Vector2(soundPanelRect.rect.width, 0), 0.4f).SetEase(Ease.InExpo);
        soundCanvasGroup.DOFade(0f, 0.4f).OnComplete(() =>
        {
            soundPanel.SetActive(false);
            isAnimating = false;
        });
    }

    public void CloseSoundPanelToOption()
    {
        volumeManager.RevertAll(); // 🎵 변경사항 되돌리기

        soundPanelRect.DOAnchorPos(new Vector2(soundPanelRect.rect.width, 0), 0.4f).SetEase(Ease.InExpo);
        soundCanvasGroup.DOFade(0f, 0.4f).OnComplete(() =>
        {
            soundPanel.SetActive(false);
        });
    }

    public void OpenScreenPanel()
    {
        screenPanel.SetActive(true);
        soundPanel.SetActive(false);

        screenPanelRect.anchoredPosition = new Vector2(screenPanelRect.rect.width, 0);
        screenCanvasGroup.alpha = 0f;

        screenPanelRect.DOAnchorPos(Vector2.zero, 0.5f).SetEase(Ease.OutExpo);
        screenCanvasGroup.DOFade(1f, 0.5f);
    }

    public void CloseScreenPanelToOption()
    {
        screenPanelRect.DOAnchorPos(new Vector2(screenPanelRect.rect.width, 0), 0.4f).SetEase(Ease.InExpo);
        screenCanvasGroup.DOFade(0f, 0.4f).OnComplete(() =>
        {
            screenPanel.SetActive(false);
        });
    }

    public void ConfirmAllSettings()
    {
        if (isAnimating) return;
        isAnimating = true;

        volumeManager.ApplyAll(); // 🎵 전체 사운드 저장
        if (screenSettingsManager != null)
        {
            screenSettingsManager.SaveOnly(); // 🖥️ 해상도 저장
        }

        PlayerPrefs.Save();

        optionPanel.DOAnchorPos(hiddenPosition, 0.2f).SetEase(Ease.InExpo);
        canvasGroup.DOFade(0f, 0.2f).OnComplete(() =>
        {
            optionPanel.gameObject.SetActive(false);
            if (mainMenuPanel != null)
            {
                mainMenuPanel.SetActive(true);
            }
            isAnimating = false;
        });

        soundPanel.SetActive(false);
        screenPanel.SetActive(false);
    }
}
