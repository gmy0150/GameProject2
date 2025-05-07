using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameTutorialUI : MonoBehaviour
{
    public static GameTutorialUI Instance;

    [Header("UI 연결")]
    public GameObject panel;
    public TextMeshProUGUI messageText;
    public Image iconImage;

    [Header("플레이어 제어")]
    public Player playerScript;
    public Animator playerAnimator;

    private Queue<GameStart.DialogueLine> dialogueQueue = new Queue<GameStart.DialogueLine>();
    private Coroutine typingCoroutine;
    private bool isTyping = false;
    private string fullCurrentText = "";

    private void Awake()
    {
        Instance = this;
        panel?.SetActive(false);
    }

    public void ShowGameDialogue(List<GameStart.DialogueLine> lines)
    {
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

        ShowNextLine();
    }

    private void Update()
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
        Debug.Log("ShowNextLine 호출됨. 남은 메시지 수: " + dialogueQueue.Count);

        if (dialogueQueue.Count > 0)
        {
            var line = dialogueQueue.Dequeue();
            Debug.Log("다음 메시지: " + line.message);

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
            Debug.Log("대화 종료됨");
            panel.SetActive(false);

            if (playerScript != null)
                playerScript.enabled = true;

            Time.timeScale = 1f;
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