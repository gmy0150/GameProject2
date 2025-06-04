using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ScreenSettingsManager : MonoBehaviour
{
    [Header("UI 요소")]
    public TMP_Text resolutionText;
    public TMP_Text fullscreenText;

    [Header("옵션창 참조")]
    public UIOptionMenu uiOptionMenu;

    private Resolution[] resolutions = new Resolution[]
    {
        new Resolution { width = 1280, height = 720 },
        new Resolution { width = 1920, height = 1080 },
        new Resolution { width = 2560, height = 1440 },
        new Resolution { width = 3840, height = 2160 } // 4K 추가
    };

    private int currentResolutionIndex;
    private bool isFullscreen = false;

    // ✅ 되돌리기용 저장된 값
    private int savedResolutionIndex;
    private bool savedFullscreen;

    void Start()
    {
        if (PlayerPrefs.HasKey("ScreenResIndex") && PlayerPrefs.HasKey("Fullscreen"))
        {
            savedResolutionIndex = PlayerPrefs.GetInt("ScreenResIndex");
            savedFullscreen = PlayerPrefs.GetInt("Fullscreen") == 1;
        }
        else
        {
            // 현재 해상도 기준으로 가장 가까운 해상도 선택
            Resolution current = Screen.currentResolution;
            savedResolutionIndex = GetClosestResolutionIndex(current.width, current.height);
            savedFullscreen = true;
        }

        // 초기 설정 적용
        currentResolutionIndex = savedResolutionIndex;
        isFullscreen = savedFullscreen;

        ApplyResolution();
        UpdateResolutionText();
        fullscreenText.text = isFullscreen ? "On" : "Off";

        LoadScreenSettings();
    }

    public void LoadScreenSettings()
    {
        if (PlayerPrefs.HasKey("ScreenResIndex") && PlayerPrefs.HasKey("Fullscreen"))
        {
            savedResolutionIndex = PlayerPrefs.GetInt("ScreenResIndex");
            savedFullscreen = PlayerPrefs.GetInt("Fullscreen") == 1;
        }
        else
        {
            Resolution current = Screen.currentResolution;
            savedResolutionIndex = GetClosestResolutionIndex(current.width, current.height);
            savedFullscreen = true;
        }

        currentResolutionIndex = savedResolutionIndex;
        isFullscreen = savedFullscreen;

        ApplyResolution();
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

    // ✅ OK 버튼: 저장 + 적용 + 닫기
        public void ApplyAll()
    {
        ApplyResolution();

        PlayerPrefs.SetInt("ScreenResIndex", currentResolutionIndex);
        PlayerPrefs.SetInt("Fullscreen", isFullscreen ? 1 : 0);
        PlayerPrefs.Save();

        savedResolutionIndex = currentResolutionIndex;
        savedFullscreen = isFullscreen;

        Debug.Log($"💾 설정 저장: {resolutions[currentResolutionIndex].width}x{resolutions[currentResolutionIndex].height}, 전체화면: {isFullscreen}");

        // ❌ 바로 닫지 말고 UIOptionMenu에게 닫으라고 지시
        uiOptionMenu.CloseScreenPanelDelayed();
    }

    // ✅ 뒤로가기 버튼: 저장하지 않고 이전 설정으로 되돌리기
    public void RevertAll()
    {
        currentResolutionIndex = savedResolutionIndex;
        isFullscreen = savedFullscreen;

        ApplyResolution();
        UpdateResolutionText();
        fullscreenText.text = isFullscreen ? "On" : "Off";

        Debug.Log("↩️ 설정 되돌림 (저장 안됨)");

        uiOptionMenu.CloseScreenPanelDelayed();
    }

    // 외부 저장 전용 (사운드에서 전체 저장 시 호출)
    public void SaveOnly()
    {
        PlayerPrefs.SetInt("ScreenResIndex", currentResolutionIndex);
        PlayerPrefs.SetInt("Fullscreen", isFullscreen ? 1 : 0);
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
}
