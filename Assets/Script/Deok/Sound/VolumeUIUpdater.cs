using UnityEngine;
using TMPro;

public class VolumeUIUpdater : MonoBehaviour
{
    public TextMeshProUGUI masterVolumeText;
    public TextMeshProUGUI bgmVolumeText;
    public TextMeshProUGUI sfxVolumeText;
    public TextMeshProUGUI uiSfxVolumeText;

    private void Start()
    {
        UpdateVolumeTexts();
    }

    private void UpdateVolumeTexts()
    {
        int master = PlayerPrefs.GetInt("MasterVolume", 100);
        int bgm = PlayerPrefs.GetInt("BGMVolume", 100);
        int sfx = PlayerPrefs.GetInt("SFXVolume", 100);
        int uiSfx = PlayerPrefs.GetInt("UI_SFXVolume", 100);

        if (masterVolumeText != null) masterVolumeText.text = master.ToString();
        if (bgmVolumeText != null) bgmVolumeText.text = bgm.ToString();
        if (sfxVolumeText != null) sfxVolumeText.text = sfx.ToString();
        if (uiSfxVolumeText != null) uiSfxVolumeText.text = uiSfx.ToString();
    }
}
