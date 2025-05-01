using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CameraAlertUI : MonoBehaviour
{
    public static CameraAlertUI Instance;

    [Header("📷 Camera Dialogue UI Elements")]
    public GameObject messagePanel;
    public TextMeshProUGUI messageText;
    public Image iconImage;

    private Queue<PhotoTriggerManager.PhotoLine> dialogueQueue = new Queue<PhotoTriggerManager.PhotoLine>();
    private Coroutine typingCoroutine;
    private bool isTyping = false;
    private string fullCurrentText = "";

    private void Awake()
    {
        Instance = this;
        messagePanel?.SetActive(false);
    }

    public void ShowPhotoDialogue(List<PhotoTriggerManager.PhotoLine> lines)
    {
        dialogueQueue.Clear();
        foreach (var line in lines)
        {
            dialogueQueue.Enqueue(line);
        }

        messagePanel.SetActive(true);
        ShowNextLine();
    }


    void Update()
    {
        if (messagePanel.activeSelf && Input.GetKeyDown(KeyCode.Space))
        {
            if (isTyping)
            {
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

            iconImage.sprite = !string.IsNullOrEmpty(line.iconName)
                ? Resources.Load<Sprite>("Icons/" + line.iconName)
                : null;

            iconImage.enabled = (iconImage.sprite != null);
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
    }
}