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

    public void UpdateText()
    {
        if (valueText != null)
            valueText.text = currentVolume.ToString();
    }

    public void Apply(AudioMixer mixer)
    {
        savedVolume = currentVolume;
        float db = Mathf.Lerp(-80f, 0f, savedVolume / 100f);
        mixer.SetFloat(mixerParamName, db);
    }

    public void Revert(AudioMixer mixer)
    {
        currentVolume = savedVolume;
        UpdateText();
        float db = Mathf.Lerp(-80f, 0f, savedVolume / 100f);
        mixer.SetFloat(mixerParamName, db);
    }
}
