using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TutorialUI : MonoBehaviour
{
    public static TutorialUI Instance;

    public GameObject panel;
    public TextMeshProUGUI messageText;
    public Image iconImage;

    private Queue<TutorialManager.DialogueLine> dialogueQueue = new Queue<TutorialManager.DialogueLine>();
    private Coroutine typingCoroutine;
    private bool isTyping = false;
    private string fullCurrentText = "";

    private void Awake()
    {
        Instance = this;
        panel?.SetActive(false);
    }

    // ✅ 내부 타입에 맞춘 함수
    public void ShowTutorialDialogue(List<TutorialManager.DialogueLine> lines)
    {
        Time.timeScale = 0f; // 게임 정지
        dialogueQueue.Clear();

        foreach (var line in lines)
            dialogueQueue.Enqueue(line);

        ShowNextLine();
    }

    void Update()
    {
        if (panel.activeSelf && Input.GetKeyDown(KeyCode.Space))
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
            panel.SetActive(true);

            typingCoroutine = StartCoroutine(TypeText(fullCurrentText));
        }
        else
        {
            panel.SetActive(false);
            Time.timeScale = 1f; // 대사 끝 → 게임 재개
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
}
