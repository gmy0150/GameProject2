using UnityEngine;
using UnityEngine.SceneManagement; 
using UnityEngine.Audio; 

public class Story_MainMenu : MonoBehaviour
{
    [Header("🎵 스토리 전용 BGM")]
    public AudioClip storyBGM;
    private AudioSource bgmAudioSource; 

    [Header("페이드 효과 연결")]
    public FadeScript fadeScript;

    [Header("이동할 씬 설정")]
    public string targetSceneName = "MainMenu";

    void Awake()
    {
        bgmAudioSource = gameObject.AddComponent<AudioSource>();
        bgmAudioSource.loop = true; 
        bgmAudioSource.playOnAwake = false;

        if (VolumeManager.Instance != null && VolumeManager.Instance.audioMixer != null)
        {
            AudioMixerGroup[] bgmGroups = VolumeManager.Instance.audioMixer.FindMatchingGroups("BGMVolume");
            if (bgmGroups.Length > 0)
            {
                bgmAudioSource.outputAudioMixerGroup = bgmGroups[0];
            }
        }
    }
    void Start()
    {
        BGMManager.Instance?.FadeToBGM(null);

        if (storyBGM != null)
        {
            bgmAudioSource.clip = storyBGM;
            bgmAudioSource.Play();
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (bgmAudioSource.isPlaying && fadeScript != null)
            {
                float fadeOutDuration = fadeScript.fadeDuration; 
                StartCoroutine(FadeOutBGM(fadeOutDuration));
            }

            if (fadeScript != null)
            {
                fadeScript.FadeToScene(targetSceneName);
            }
            else
            {
                Debug.LogError("Story_MainMenu에 FadeScript가 연결되지 않았습니다! 인스펙터에서 연결해주세요.");
                SceneManager.LoadScene(targetSceneName);
            }
        }
    }

    System.Collections.IEnumerator FadeOutBGM(float duration)
    {
        float startVolume = bgmAudioSource.volume;
        for (float t = 0; t < duration; t += Time.deltaTime)
        {
            bgmAudioSource.volume = Mathf.Lerp(startVolume, 0f, t / duration);
            yield return null;
        }
        bgmAudioSource.volume = 0f;
        bgmAudioSource.Stop();
    }
}