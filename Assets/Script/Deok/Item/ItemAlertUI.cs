using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemAlertUI : MonoBehaviour
{
    public static ItemAlertUI Instance;

    public GameObject messagePanel;
    public TextMeshProUGUI messageText;
    public Image iconImage;

    private Queue<DialogueLine> dialogueQueue = new Queue<DialogueLine>();
    private Coroutine typingCoroutine;
    private bool isTyping = false;

    private void Awake()
    {
        Instance = this;
        messagePanel?.SetActive(false);
    }

    public void ShowDialogue(List<DialogueLine> lines)
    {
        dialogueQueue.Clear();
        foreach (var line in lines)
            dialogueQueue.Enqueue(line);

        ShowNextLine();
    }

    void Update()
    {
        if (messagePanel.activeSelf && Input.GetKeyDown(KeyCode.Space))
        {
            if (isTyping)
            {
                // Skip typing and show full line
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

    string fullCurrentText = "";

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

            messagePanel.SetActive(true);
            typingCoroutine = StartCoroutine(TypeText(line.message));
        }
        else
        {
            HideMessage();
        }
    }

    IEnumerator TypeText(string text)
    {
        isTyping = true;
        messageText.text = "";

        foreach (char c in text)
        {
            messageText.text += c;
            yield return new WaitForSeconds(0.08f);
        }

        isTyping = false;
    }

    void HideMessage()
    {
        messagePanel?.SetActive(false);
    }
}
