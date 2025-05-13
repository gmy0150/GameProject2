using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using TMPro;


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
        masterVolume.Load(audioMixer);
        bgmVolume.Load(audioMixer);
        sfxVolume.Load(audioMixer);
        uiSfxVolume.Load(audioMixer); 
    }

    public void ApplyAll()
    {
        masterVolume.Apply(audioMixer);
        bgmVolume.Apply(audioMixer);
        sfxVolume.Apply(audioMixer);
        uiSfxVolume.Apply(audioMixer);

        float debugValue;
        audioMixer.GetFloat("UI_SFXVolume", out debugValue);
        Debug.Log($"[DEBUG] UI_SFXVolume 적용값: {debugValue} dB");
    }

    public void RevertAll()
    {
        masterVolume.Revert(audioMixer);
        bgmVolume.Revert(audioMixer);
        sfxVolume.Revert(audioMixer);
        uiSfxVolume.Revert(audioMixer);
    }
}

