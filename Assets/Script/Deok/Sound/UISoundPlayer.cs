using UnityEngine;
using UnityEngine.Audio;

public class UISoundPlayer : MonoBehaviour
{
    public AudioSource uiAudioSource;
    public AudioClip clickSound;
    public AudioClip clickSound_2; // ✅ 추가: 다른 버튼용 사운드

    [Header("AudioMixer Parameter Names")]
    public string masterVolumeParam = "MasterVolume";
    public string uiSfxVolumeParam = "UI_SFXVolume";

    private void Reset()
    {
        if (uiAudioSource == null)
            uiAudioSource = GetComponent<AudioSource>();
    }

    public void PlayClickSound()
    {
        PlayClickSoundInternal(clickSound);
    }

    public void PlayClickSound2() // ✅ 버튼에 연결할 함수
    {
        PlayClickSoundInternal(clickSound_2);
    }

    private void PlayClickSoundInternal(AudioClip clip)
    {
        if (uiAudioSource == null || clip == null || VolumeManager.Instance == null)
            return;

        float masterDb, uiDb;
        VolumeManager.Instance.audioMixer.GetFloat(masterVolumeParam, out masterDb);
        VolumeManager.Instance.audioMixer.GetFloat(uiSfxVolumeParam, out uiDb);

        float masterVolume = Mathf.Pow(10f, masterDb / 20f);
        float uiVolume = Mathf.Pow(10f, uiDb / 20f);

        uiAudioSource.volume = masterVolume * uiVolume;
        uiAudioSource.PlayOneShot(clip);
    }
}
