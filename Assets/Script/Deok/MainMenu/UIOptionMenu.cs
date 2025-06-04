using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

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

        VolumeManager.Instance.ApplyAll(); // ⚙️ 설정 적용

        StartCoroutine(CloseSoundPanelAfterDelay()); // ⏳ 잠깐 기다렸다가 닫기
    }

    private IEnumerator CloseSoundPanelAfterDelay()
    {
        yield return new WaitForSeconds(0.1f); // 👈 이 시간 동안 ApplyAll로 인한 내부 처리가 안정화됨

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
        StartCoroutine(CloseSoundPanelAfterDelay());
    }

        public void CloseScreenPanelDelayed()
    {
        // 해상도 변경 직후 잠시 대기 → DOTween이 안정적으로 적용되도록 유도
        StartCoroutine(CloseScreenPanelAfterDelay());
    }

    private IEnumerator CloseScreenPanelAfterDelay()
    {
        yield return new WaitForSeconds(0.1f); // 짧은 대기
        AnimatePanelClose(screenPanel, screenPanelRect, screenCanvasGroup);
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

    private bool canPressEsc = true;
    private IEnumerator StartEscCooldown()
    {
        canPressEsc = false;
        yield return new WaitForSeconds(1f);
        canPressEsc = true;
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && canPressEsc)
        {
            if (screenPanel.activeSelf)
            {
                CloseScreenPanelToOption();
                StartCoroutine(StartEscCooldown());
            }
            else if (soundPanel.activeSelf)
            {
                CloseSoundPanelToOption();
                StartCoroutine(StartEscCooldown());
            }
            else if (optionPanel.gameObject.activeSelf && canvasGroup.alpha > 0.5f)
            {
                backButton.onClick.Invoke(); // Back 버튼 기능 수행
                StartCoroutine(StartEscCooldown());
            }
        }
    }

}
