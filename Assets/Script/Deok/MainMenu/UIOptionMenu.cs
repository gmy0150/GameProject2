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

    [Header("사운드 설정")]
    public Slider bgmSlider;
    public Slider sfxSlider;

    [Header("해상도 설정 스크립트")]
    public ScreenSettingsManager screenSettingsManager;

    private bool isAnimating = false;
    private Vector2 hiddenPosition;
    private Vector2 visiblePosition = Vector2.zero;

    void Start()
    {
        // 옵션 메뉴 초기 위치
        hiddenPosition = new Vector2(optionPanel.rect.width, 0);
        optionPanel.anchoredPosition = hiddenPosition;
        canvasGroup.alpha = 0f;

        // 패널 비활성화
        soundPanel.SetActive(false);
        screenPanel.SetActive(false);

        // 사운드 패널 초기 위치 & 투명도
        soundPanelRect.anchoredPosition = new Vector2(soundPanelRect.rect.width, 0);
        soundCanvasGroup.alpha = 0f;

        // 화면 패널 초기 위치 & 투명도
        screenPanelRect.anchoredPosition = new Vector2(screenPanelRect.rect.width, 0);
        screenCanvasGroup.alpha = 0f;

        // 🎵 저장된 사운드 값 불러오기
        bgmSlider.value = PlayerPrefs.GetFloat("BGMVolume", 1f);
        sfxSlider.value = PlayerPrefs.GetFloat("SFXVolume", 1f);
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
        Debug.Log("✅ OpenSoundPanel 함수 실행됨");
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

        PlayerPrefs.SetFloat("BGMVolume", bgmSlider.value);
        PlayerPrefs.SetFloat("SFXVolume", sfxSlider.value);
        PlayerPrefs.Save();
        Debug.Log($"🎵 사운드 저장됨: BGM={bgmSlider.value}, SFX={sfxSlider.value}");

        soundPanelRect.DOAnchorPos(new Vector2(soundPanelRect.rect.width, 0), 0.4f).SetEase(Ease.InExpo);
        soundCanvasGroup.DOFade(0f, 0.4f).OnComplete(() =>
        {
            soundPanel.SetActive(false);
            isAnimating = false;
        });
    }

    public void CloseSoundPanelToOption()
    {
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

    public GameObject mainMenuPanel; // ✅ 메인 메뉴 오브젝트 연결용 변수 추가

public void ConfirmAllSettings()
{
    if (isAnimating) return;
    isAnimating = true;

    // 🎵 사운드 저장 ( 추후에 BGM / SFX추가해서 넣으면 됨 )
    //PlayerPrefs.SetFloat("BGMVolume", bgmSlider.value);
    //PlayerPrefs.SetFloat("SFXVolume", sfxSlider.value);

    // 🖥️ 해상도 저장
    if (screenSettingsManager != null)
    {
        screenSettingsManager.SaveOnly();
    }

    PlayerPrefs.Save();
    Debug.Log("✅ 전체 설정 저장 완료");

    // 옵션 메뉴 닫기 + 메인 메뉴 UI 다시 보여주기
    optionPanel.DOAnchorPos(hiddenPosition, 0.2f).SetEase(Ease.InExpo);
    canvasGroup.DOFade(0f, 0.2f).OnComplete(() =>
    {
        optionPanel.gameObject.SetActive(false);
        if (mainMenuPanel != null)
        {
            mainMenuPanel.SetActive(true); // ✅ 메인 메뉴 다시 보이기
        }
        isAnimating = false;
    });

    // 하위 패널 닫기
    soundPanel.SetActive(false);
    screenPanel.SetActive(false);
}

}
