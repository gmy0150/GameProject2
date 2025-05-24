using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

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

    [Header("해상도 설정 스크립트")]
    public ScreenSettingsManager screenSettingsManager;

    [Header("메인메뉴 패널")]
    public GameObject mainMenuPanel;

    [Header("볼륨 컨트롤 유닛 연결")]
    public VolumeControlUnit masterVolumeUnit;
    public VolumeControlUnit bgmVolumeUnit;
    public VolumeControlUnit sfxVolumeUnit;
    public VolumeControlUnit uiSfxVolumeUnit;

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

        // ✅ VolumeManager → UI SFX까지 포함한 연결
        VolumeManager.Instance.BindUI(masterVolumeUnit, bgmVolumeUnit, sfxVolumeUnit, uiSfxVolumeUnit);

        // ✅ 설정값 불러오기
        VolumeManager.Instance.LoadAll();
        screenSettingsManager?.LoadScreenSettings();

        Debug.Log("🔄 OptionMenu 초기화 완료: VolumeManager & ScreenSettings 반영됨");
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
            optionButton.interactable = true;
            backButton.interactable = true;
        });
    }

    public void CloseOptionMenu()
    {
        if (isAnimating) return;
        isAnimating = true;

        optionButton.interactable = false;
        backButton.interactable = false;

        CloseAllSubPanels();

        optionPanel.DOAnchorPos(hiddenPosition, 0.2f).SetEase(Ease.InExpo);
        canvasGroup.DOFade(0f, 0.2f).OnComplete(() =>
        {
            optionPanel.gameObject.SetActive(false);
            HandleAfterClose();
            isAnimating = false;
        });
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

        VolumeManager.Instance.ApplyAll();

        soundPanelRect.DOAnchorPos(new Vector2(soundPanelRect.rect.width, 0), 0.4f).SetEase(Ease.InExpo);
        soundCanvasGroup.DOFade(0f, 0.4f).OnComplete(() =>
        {
            soundPanel.SetActive(false);
            isAnimating = false;
        });
    }

    public void CloseSoundPanelToOption()
    {
        VolumeManager.Instance.RevertAll();
        AnimatePanelClose(soundPanel, soundPanelRect, soundCanvasGroup);
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
        AnimatePanelClose(screenPanel, screenPanelRect, screenCanvasGroup);
    }

    public void ConfirmAllSettings()
    {
        if (isAnimating) return;
        isAnimating = true;

        VolumeManager.Instance.ApplyAll();
        screenSettingsManager?.SaveOnly();

        PlayerPrefs.Save();

        CloseAllSubPanels();

        optionPanel.DOAnchorPos(hiddenPosition, 0.2f).SetEase(Ease.InExpo);
        canvasGroup.DOFade(0f, 0.2f).OnComplete(() =>
        {
            optionPanel.gameObject.SetActive(false);
            HandleAfterClose();
            isAnimating = false;
        });
    }

    private void CloseAllSubPanels()
    {
        soundPanel.SetActive(false);
        screenPanel.SetActive(false);
    }

    private void AnimatePanelClose(GameObject panel, RectTransform rect, CanvasGroup group)
    {
        rect.DOAnchorPos(new Vector2(rect.rect.width, 0), 0.4f).SetEase(Ease.InExpo);
        group.DOFade(0f, 0.4f).OnComplete(() =>
        {
            panel.SetActive(false);
        });
    }

    private void HandleAfterClose()
    {
        string currentScene = SceneManager.GetActiveScene().name;

        if (currentScene == "Test_MainMenu")
        {
            if (mainMenuPanel != null)
            {
                mainMenuPanel.SetActive(true);
            }
        }
        else if (currentScene == "Test_Game")
        {
            PauseManager pauseManager = FindObjectOfType<PauseManager>();
            if (pauseManager != null && pauseManager.gameUIRoot != null)
            {
                pauseManager.gameUIRoot.SetActive(true);
            }
        }
    }
}
