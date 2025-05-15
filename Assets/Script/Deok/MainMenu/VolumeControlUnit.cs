using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using TMPro;

[System.Serializable]
public class VolumeControlUnit
{
    public string mixerParamName;

    public TextMeshProUGUI valueText;
    public Button minusButton;
    public Button plusButton;

    private int currentVolume = 100;
    private int savedVolume = 100;

    private const int minVolume = 0;
    private const int maxVolume = 100;
    private const int safeMinVolume = 10;  // 최소 보장값 (볼륨 0으로 저장 시 보호)

    public void Initialize()
    {
        UpdateText();

        minusButton.onClick.AddListener(() =>
        {
            currentVolume = Mathf.Max(minVolume, currentVolume - 10);
            UpdateText();
            SetVolumeToMixer(VolumeManager.Instance.audioMixer, currentVolume);
        });

        plusButton.onClick.AddListener(() =>
        {
            currentVolume = Mathf.Min(maxVolume, currentVolume + 10);
            UpdateText();
            SetVolumeToMixer(VolumeManager.Instance.audioMixer, currentVolume);
        });
    }

    private void UpdateText()
    {
        if (valueText != null)
            valueText.text = currentVolume.ToString();
    }

    private void SetVolumeToMixer(AudioMixer mixer, int volumeValue)
    {
        float linearVolume = Mathf.Clamp01(volumeValue / 100f);
        float db = Mathf.Log10(Mathf.Max(linearVolume, 0.0001f)) * 20f;
        mixer.SetFloat(mixerParamName, db);

        Debug.Log($"[DEBUG] {mixerParamName} ➔ {db} dB 적용됨 (volume: {volumeValue})");
    }

    public void Load(AudioMixer mixer)
    {
        currentVolume = PlayerPrefs.GetInt(mixerParamName, 100);

        if (currentVolume < 0 || currentVolume > 100)
        {
            Debug.LogWarning($"[RESET] {mixerParamName} 값이 {currentVolume}이라서 100으로 초기화됨");
            currentVolume = 100;
            PlayerPrefs.SetInt(mixerParamName, currentVolume);
            PlayerPrefs.Save();
        }

        savedVolume = currentVolume;
        UpdateText();
        SetVolumeToMixer(mixer, savedVolume);
    }


    public void Apply(AudioMixer mixer)
    {
        savedVolume = currentVolume;
        PlayerPrefs.SetInt(mixerParamName, savedVolume);
        PlayerPrefs.Save();
        SetVolumeToMixer(mixer, savedVolume);
    }

    public void Revert(AudioMixer mixer)
    {
        currentVolume = savedVolume;
        UpdateText();
        SetVolumeToMixer(mixer, savedVolume);
    }
}
