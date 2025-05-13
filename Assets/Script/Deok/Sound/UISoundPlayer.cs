using UnityEngine;
using UnityEngine.Audio;

public class UISoundPlayer : MonoBehaviour
{
    public AudioSource uiAudioSource; // UI 사운드 재생 전용 AudioSource
    public AudioClip clickSound;      // 클릭 사운드 클립

    [Header("AudioMixer Parameter Names")]
    public string masterVolumeParam = "MasterVolume";
    public string uiSfxVolumeParam = "UI_SFXVolume";

    private void Reset()
    {
        // 자동 연결 (에디터에서 붙여줄 때)
        if (uiAudioSource == null)
            uiAudioSource = GetComponent<AudioSource>();
    }

    public void PlayClickSound()
    {
        if (uiAudioSource == null || clickSound == null || VolumeManager.Instance == null)
            return;

        float masterDb, uiDb;

        // AudioMixer에서 현재 볼륨 값(dB) 가져오기
        VolumeManager.Instance.audioMixer.GetFloat(masterVolumeParam, out masterDb);
        VolumeManager.Instance.audioMixer.GetFloat(uiSfxVolumeParam, out uiDb);

        // dB ➔ 선형 볼륨 값으로 변환
        float masterVolume = Mathf.Pow(10f, masterDb / 20f);
        float uiVolume = Mathf.Pow(10f, uiDb / 20f);

        // 실제 최종 볼륨 (Master * UI SFX)
        uiAudioSource.volume = masterVolume * uiVolume;

        // 효과음 재생
        uiAudioSource.PlayOneShot(clickSound);
    }
}
