using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ScreenSettingsManager : MonoBehaviour
{
    [Header("UI 요소")]
    public TMP_Text resolutionText;
    public TMP_Text fullscreenText;

    private Resolution[] resolutions = new Resolution[]
    {
        new Resolution { width = 1280, height = 720 },
        new Resolution { width = 1920, height = 1080 },
        new Resolution { width = 2560, height = 1440 },
        new Resolution { width = 3840, height = 2160 } // 4K 추가
    };

    private int currentResolutionIndex;
    private bool isFullscreen = false;

    void Start()
    {
        // ✅ 저장 여부 확인
        if (PlayerPrefs.HasKey("ScreenResIndex") && PlayerPrefs.HasKey("Fullscreen"))
        {
            currentResolutionIndex = PlayerPrefs.GetInt("ScreenResIndex");
            isFullscreen = PlayerPrefs.GetInt("Fullscreen") == 1;
        }
        else
        {
            // ✅ 저장된 값이 없다면, 현재 시스템 해상도에 가장 가까운 해상도 선택
            Resolution current = Screen.currentResolution;
            currentResolutionIndex = GetClosestResolutionIndex(current.width, current.height);
            isFullscreen = true; // 기본값 전체화면
        }

        // ✅ 실제 적용
        ApplyResolution();

        // ✅ UI 업데이트
        UpdateResolutionText();
        fullscreenText.text = isFullscreen ? "On" : "Off";
    }

    public void ChangeResolutionLeft()
    {
        currentResolutionIndex = (currentResolutionIndex - 1 + resolutions.Length) % resolutions.Length;
        UpdateResolutionText();
    }

    public void ChangeResolutionRight()
    {
        currentResolutionIndex = (currentResolutionIndex + 1) % resolutions.Length;
        UpdateResolutionText();
    }

    public void ToggleFullscreen()
    {
        isFullscreen = !isFullscreen;
        fullscreenText.text = isFullscreen ? "On" : "Off";
    }

    public UIOptionMenu uiOptionMenu;
    public void ApplyAndSaveSettings()
    {
        ApplyResolution();

        PlayerPrefs.SetInt("ScreenResIndex", currentResolutionIndex);
        PlayerPrefs.SetInt("Fullscreen", isFullscreen ? 1 : 0);
        PlayerPrefs.Save();

        Debug.Log($"💾 설정 저장: {resolutions[currentResolutionIndex].width}x{resolutions[currentResolutionIndex].height}, 전체화면: {isFullscreen}");
        
        uiOptionMenu.CloseScreenPanelToOption();
    }

    private void ApplyResolution()
    {
        Resolution res = resolutions[currentResolutionIndex];
        Screen.SetResolution(res.width, res.height,
            isFullscreen ? FullScreenMode.FullScreenWindow : FullScreenMode.Windowed);
    }

    private void UpdateResolutionText()
    {
        Resolution res = resolutions[currentResolutionIndex];
        resolutionText.text = $"{res.width} × {res.height}";
    }

    // ✅ 현재 해상도에 가장 가까운 인덱스를 계산하는 함수
    private int GetClosestResolutionIndex(int width, int height)
    {
        int closestIndex = 0;
        int minDiff = int.MaxValue;

        for (int i = 0; i < resolutions.Length; i++)
        {
            int diff = Mathf.Abs(resolutions[i].width - width) + Mathf.Abs(resolutions[i].height - height);
            if (diff < minDiff)
            {
                minDiff = diff;
                closestIndex = i;
            }
        }

        return closestIndex;
    }

    public void SaveOnly()
    {
    PlayerPrefs.SetInt("ScreenResIndex", currentResolutionIndex);
    PlayerPrefs.SetInt("Fullscreen", isFullscreen ? 1 : 0);
    }
}
