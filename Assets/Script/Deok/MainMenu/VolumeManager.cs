using UnityEngine;
using UnityEngine.Audio;

public class VolumeManager : MonoBehaviour
{
    public AudioMixer audioMixer;

    public VolumeControlUnit masterVolume;
    public VolumeControlUnit bgmVolume;
    public VolumeControlUnit sfxVolume;

    void Start()
    {
        masterVolume.Initialize();
        bgmVolume.Initialize();
        sfxVolume.Initialize();
    }

    public void ApplyAll()
    {
        masterVolume.Apply(audioMixer);
        bgmVolume.Apply(audioMixer);
        sfxVolume.Apply(audioMixer);
    }

    public void RevertAll()
    {
        masterVolume.Revert(audioMixer);
        bgmVolume.Revert(audioMixer);
        sfxVolume.Revert(audioMixer);
    }
}
