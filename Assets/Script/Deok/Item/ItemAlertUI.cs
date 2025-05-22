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
    private string fullCurrentText = "";

    // ✅ 직접 Inspector에서 연결할 Player와 Animator
    [Header("Player Control References")]
    public Player playerScript;
    public Animator playerAnimator;

    private void Awake()
    {
        Instance = this;
        messagePanel?.SetActive(false);
    }

    public void ShowDialogue(List<DialogueLine> lines)
    {
        GameManager.Instance.ActPlay(false);
        Time.timeScale = 0f;


        if (playerScript != null)
            playerScript.enabled = false;

        if (playerAnimator != null)
        {
            playerAnimator.SetFloat("Speed", 0f);
            playerAnimator.SetBool("IsRunning", false);
        }

        dialogueQueue.Clear();
        foreach (var line in lines)
            dialogueQueue.Enqueue(line);

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
            yield return new WaitForSecondsRealtime(0.07f);
        }

        isTyping = false;
    }

    void HideMessage()
    {
        messagePanel?.SetActive(false);

        if (playerScript != null)
            playerScript.enabled = true;

        Time.timeScale = 1f;
        GameManager.Instance.ActPlay(true);

    }
}
