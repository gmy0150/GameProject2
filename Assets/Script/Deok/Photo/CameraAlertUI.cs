using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

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

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            messagePanel?.SetActive(false);
            audioSource = gameObject.AddComponent<AudioSource>();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void ShowPhotoDialogue(List<PhotoTriggerManager.PhotoLine> lines, Action onComplete = null)
    {
        Time.timeScale = 0f;

        if (playerScript != null)
            playerScript.enabled = false;

        dialogueQueue.Clear();
        foreach (var line in lines)
            dialogueQueue.Enqueue(line);

        this.onDialogueComplete = onComplete;

        messagePanel.SetActive(true);
        ShowNextLine();
    }

    void Update()
    {
        if (messagePanel.activeSelf && Input.GetKeyDown(KeyCode.Space))
        {
            if (isTyping)
            {
                if (typingCoroutine != null)
                    StopCoroutine(typingCoroutine);
                messageText.text = fullCurrentText;
                isTyping = false;
            }
            else
            {
                ShowNextLine();
            }
        }
    }

    void ShowNextLine()
    {
        if (dialogueQueue.Count > 0)
        {
            var line = dialogueQueue.Dequeue();
            fullCurrentText = line.message;

            if (!string.IsNullOrEmpty(line.voiceName))
            {
                AudioClip clip = Resources.Load<AudioClip>("Voices/" + line.voiceName);
                if (clip != null)
                {
                    if (audioSource.isPlaying)
                        audioSource.Stop();
                    audioSource.clip = clip;
                    audioSource.Play();
                }
            }

            iconImage.sprite = !string.IsNullOrEmpty(line.iconName)
                ? Resources.Load<Sprite>("Icons/" + line.iconName)
                : null;
            iconImage.enabled = (iconImage.sprite != null);

            if (typingCoroutine != null)
                StopCoroutine(typingCoroutine);
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

        if (playerScript != null)
            playerScript.enabled = true;

        Time.timeScale = 1f;

        onDialogueComplete?.Invoke();
    }
}