using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class VolumeSlider : MonoBehaviour
{
    public TextMeshProUGUI valueText;     // 숫자 출력
    public Button minusButton;            // <
    public Button plusButton;             // >

    private int volume = 100;             // 초기값
    private const int minVolume = 0;
    private const int maxVolume = 100;

    void Start()
    {
        UpdateVolumeText();

        minusButton.onClick.AddListener(() =>
        {
            volume = Mathf.Max(minVolume, volume - 10);
            UpdateVolumeText();
        });

        plusButton.onClick.AddListener(() =>
        {
            volume = Mathf.Min(maxVolume, volume + 10);
            UpdateVolumeText();
        });
    }

    void UpdateVolumeText()
    {
        valueText.text = volume.ToString();
        // 나중에 실제 오디오 볼륨 조절도 여기에 추가 가능
    }
}
