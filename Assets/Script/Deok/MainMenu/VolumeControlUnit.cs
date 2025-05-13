using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Audio;

[System.Serializable]
public class VolumeControlUnit
{
    public string mixerParamName; // AudioMixer 파라미터 이름 (예: "MasterVolume")

    public TextMeshProUGUI valueText;
    public Button minusButton;
    public Button plusButton;

    private int currentVolume = 100;
    private int savedVolume = 100;

    private const int minVolume = 0;
    private const int maxVolume = 100;

    public void Initialize()
    {
        UpdateText();

        minusButton.onClick.AddListener(() =>
        {
            currentVolume = Mathf.Max(minVolume, currentVolume - 10);
            UpdateText();
        });

        plusButton.onClick.AddListener(() =>
        {
            currentVolume = Mathf.Min(maxVolume, currentVolume + 10);
            UpdateText();
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

        // 디버그 로그
        Debug.Log($"[DEBUG] {mixerParamName} SetFloat({db} dB) 적용됨 (volume: {volumeValue})");
    }

    public void Load(AudioMixer mixer)
    {
        currentVolume = PlayerPrefs.GetInt(mixerParamName, 100);
        savedVolume = currentVolume;
        UpdateText();
        SetVolumeToMixer(mixer, savedVolume);
    }

    public void Apply(AudioMixer mixer)
    {
        savedVolume = currentVolume;
        PlayerPrefs.SetInt(mixerParamName, savedVolume);
        SetVolumeToMixer(mixer, savedVolume);
    }

    public void Revert(AudioMixer mixer)
    {
        currentVolume = savedVolume;
        UpdateText();
        SetVolumeToMixer(mixer, savedVolume);
    }
}
