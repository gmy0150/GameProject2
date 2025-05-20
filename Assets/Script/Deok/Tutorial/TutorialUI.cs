using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TutorialUI : MonoBehaviour
{
    public static TutorialUI Instance;

    [Header("UI 연결")]
    public GameObject panel;
    public TextMeshProUGUI messageText;
    public Image iconImage;

    [Header("플레이어 제어")]
    public Player playerScript;
    public Animator playerAnimator;

    private Queue<TutorialManager.DialogueLine> dialogueQueue = new Queue<TutorialManager.DialogueLine>();
    private Coroutine typingCoroutine;
    private bool isTyping = false;
    private string fullCurrentText = "";

    private void Awake()
    {
        Instance = this;

        if (panel != null)
            panel.SetActive(false);

        if (messageText == null)
            Debug.LogError("❌ messageText가 연결되지 않았습니다.");

        if (iconImage == null)
            Debug.LogError("❌ iconImage가 연결되지 않았습니다.");
    }

    public void ShowTutorialDialogue(List<TutorialManager.DialogueLine> lines)
    {
        if (lines == null || lines.Count == 0)
        {
            Debug.LogWarning("⚠️ ShowTutorialDialogue에 빈 대사가 들어왔습니다.");
            return;
        }

        Time.timeScale = 0f;
        GameManager.Instance.ActPlay(false);
        if (playerScript != null)
            playerScript.enabled = false;

        if (playerAnimator != null)
        {
            playerAnimator.SetFloat("Speed", 0f);
            playerAnimator.SetBool("IsRunning", false);
        }

        dialogueQueue.Clear();
        foreach (var line in lines)
        {
            if (line != null)
                dialogueQueue.Enqueue(line);
        }

        ShowNextLine();
    }

    private void Update()
    {
        if (panel != null && panel.activeSelf && Input.GetKeyDown(KeyCode.Space))
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
            fullCurrentText = line?.message ?? "";

            if (iconImage != null)
            {
                iconImage.sprite = !string.IsNullOrEmpty(line.iconName)
                    ? Resources.Load<Sprite>("Icons/" + line.iconName)
                    : null;

                iconImage.enabled = (iconImage.sprite != null);
            }

            if (panel != null)
                panel.SetActive(true);

            typingCoroutine = StartCoroutine(TypeText(fullCurrentText));
        }
        else
        {
            if (panel != null)
                panel.SetActive(false);

            if (playerScript != null)
                playerScript.enabled = true;

            Time.timeScale = 1f;
        GameManager.Instance.ActPlay(true);

        }
    }

    IEnumerator TypeText(string text)
    {
        isTyping = true;

        if (messageText == null)
        {
            Debug.LogError("❌ messageText가 null입니다.");
            yield break;
        }

        messageText.text = "";

        foreach (char c in text ?? "")
        {
            messageText.text += c;
            yield return new WaitForSecondsRealtime(0.05f);
        }

        isTyping = false;
    }
}
