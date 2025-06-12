using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using UnityEngine.Audio; // 오디오 믹서를 사용하기 위해 추가

public class CameraAlertUI : MonoBehaviour
{
    public static CameraAlertUI Instance;

    [Header("📸 Camera Dialogue UI Elements")]
    public GameObject messagePanel;
    public TextMeshProUGUI messageText;
    public Image iconImage;

    [Header("Player Control References")]
    public Player playerScript;
    public Animator playerAnimator;

    private Queue<PhotoTriggerManager.PhotoLine> dialogueQueue = new Queue<PhotoTriggerManager.PhotoLine>();
    private Coroutine typingCoroutine;
    private bool isTyping = false;
    private string fullCurrentText = "";
    private AudioSource audioSource;
    private Action onDialogueComplete;

    private Dictionary<string, AudioClip> preloadedClips = new Dictionary<string, AudioClip>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            messagePanel?.SetActive(false);
            audioSource = gameObject.AddComponent<AudioSource>();

            SetupMixerOutput();
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    void SetupMixerOutput()
    {
        if (VolumeManager.Instance == null || VolumeManager.Instance.audioMixer == null) return;
        
        AudioMixerGroup[] sfxGroups = VolumeManager.Instance.audioMixer.FindMatchingGroups("SFXVolume");
        if (sfxGroups.Length > 0)
        {
            audioSource.outputAudioMixerGroup = sfxGroups[0];
        }
        else
        {
            Debug.LogWarning("AudioMixer에서 'SFXVolume' 그룹을 찾을 수 없습니다.");
        }
    }


    public void ShowPhotoDialogue(List<PhotoTriggerManager.PhotoLine> lines, Action onComplete = null)
    {
        Time.timeScale = 0f;

        if (playerScript != null)
            playerScript.enabled = false;

        PreloadAudioClips(lines);

        dialogueQueue.Clear();
        foreach (var line in lines)
            dialogueQueue.Enqueue(line);

        this.onDialogueComplete = onComplete;

        messagePanel.SetActive(true);
        ShowNextLine();
    }

    void PreloadAudioClips(List<PhotoTriggerManager.PhotoLine> lines)
    {
        preloadedClips.Clear();
        foreach (var line in lines)
        {
            if (!string.IsNullOrEmpty(line.voiceName) && !preloadedClips.ContainsKey(line.voiceName))
            {
                AudioClip clip = Resources.Load<AudioClip>("Voices/" + line.voiceName);
                if (clip != null)
                {
                    preloadedClips.Add(line.voiceName, clip);
                }
            }
        }
    }

    void Update()
    {
        if (messagePanel.activeSelf && Input.GetKeyDown(KeyCode.Space))
        {
            if (isTyping)
            {
                if (typingCoroutine != null) StopCoroutine(typingCoroutine);
                isTyping = false;
                messageText.text = fullCurrentText;
            }
            else
            {
                ShowNextLine();
            }
        }
    }

    void ShowNextLine()
    {
        audioSource.Stop();
        audioSource.clip = null;

        if (dialogueQueue.Count > 0)
        {
            var line = dialogueQueue.Dequeue();
            fullCurrentText = line.message;

            if (!string.IsNullOrEmpty(line.voiceName) && preloadedClips.ContainsKey(line.voiceName))
            {
                audioSource.clip = preloadedClips[line.voiceName];
                audioSource.Play();
            }

            iconImage.sprite = !string.IsNullOrEmpty(line.iconName) ? Resources.Load<Sprite>("Icons/" + line.iconName) : null;
            iconImage.enabled = (iconImage.sprite != null);

            if (typingCoroutine != null) StopCoroutine(typingCoroutine);
            typingCoroutine = StartCoroutine(TypeText(line.message));
        }
        else
        {
            HidePanel();
        }
    }

    IEnumerator TypeText(string text)
    {
        isTyping = true;
        messageText.text = "";
        foreach (char c in text)
        {
            messageText.text += c;
            yield return new WaitForSecondsRealtime(0.05f);
        }
        isTyping = false;
    }

    void HidePanel()
    {
        messagePanel?.SetActive(false);
        if (playerScript != null) playerScript.enabled = true;
        Time.timeScale = 1f;

        audioSource.Stop();
        audioSource.clip = null;
        
        onDialogueComplete?.Invoke();
    }
}