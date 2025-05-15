using UnityEngine;
using UnityEngine.Audio;

public class VolumeManager : MonoBehaviour
{
    public static VolumeManager Instance;

    public AudioMixer audioMixer;

    public VolumeControlUnit masterVolume;
    public VolumeControlUnit bgmVolume;
    public VolumeControlUnit sfxVolume;
    public VolumeControlUnit uiSfxVolume;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        LoadAll();
    }

    public void BindUI(VolumeControlUnit master, VolumeControlUnit bgm, VolumeControlUnit sfx, VolumeControlUnit uiSfx)
    {
        masterVolume = master;
        bgmVolume = bgm;
        sfxVolume = sfx;
        uiSfxVolume = uiSfx;

        masterVolume.Initialize();
        bgmVolume.Initialize();
        sfxVolume.Initialize();
        uiSfxVolume.Initialize();

        LoadAll();
    }

    public void LoadAll()
    {
        Debug.Log("[VolumeManager] LoadAll 호출됨");
        masterVolume.Load(audioMixer);
        bgmVolume.Load(audioMixer);
        sfxVolume.Load(audioMixer);
        uiSfxVolume.Load(audioMixer);
    }

    public void ApplyAll()
    {
        Debug.Log("[VolumeManager] ApplyAll 호출됨");
        masterVolume.Apply(audioMixer);
        bgmVolume.Apply(audioMixer);
        sfxVolume.Apply(audioMixer);
        uiSfxVolume.Apply(audioMixer);
    }

    public void RevertAll()
    {
        Debug.Log("[VolumeManager] RevertAll 호출됨");
        masterVolume.Revert(audioMixer);
        bgmVolume.Revert(audioMixer);
        sfxVolume.Revert(audioMixer);
        uiSfxVolume.Revert(audioMixer);
    }
}
