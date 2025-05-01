using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PhotoUI : MonoBehaviour
{
    public static PhotoUI Instance;

    public GameObject panel;
    public TextMeshProUGUI dialogueText;
    public Image iconImage;
    public float typingSpeed = 0.05f;

    private List<DialogueLine> currentLines;
    private int currentIndex = 0;
    private bool isTyping = false;
    private bool isDialogueActive = false;

    void Awake()
    {
        Instance = this;
    }

    void Update()
    {
        if (isDialogueActive && Input.GetMouseButtonDown(0))
        {
            if (isTyping)
            {
                StopAllCoroutines();
                dialogueText.text = currentLines[currentIndex].message;
                isTyping = false;
            }
            else
            {
                currentIndex++;
                if (currentIndex < currentLines.Count)
                {
                    StartCoroutine(TypeLine(currentLines[currentIndex]));
                }
                else
                {
                    EndDialogue();
                }
            }
        }
    }

    public void StartDialogue(List<DialogueLine> lines)
    {
        currentLines = lines;
        currentIndex = 0;
        isDialogueActive = true;
        panel.SetActive(true);
        Time.timeScale = 0f;

        StartCoroutine(TypeLine(currentLines[currentIndex]));
    }

    IEnumerator TypeLine(DialogueLine line)
    {
        isTyping = true;
        dialogueText.text = "";

        // 🎨 아이콘 이미지 불러오기
        SetIcon(line.iconName);

        foreach (char c in line.message.ToCharArray())
        {
            dialogueText.text += c;
            yield return new WaitForSecondsRealtime(typingSpeed);
        }

        isTyping = false;
    }

    void SetIcon(string iconName)
    {
        if (string.IsNullOrEmpty(iconName))
        {
            iconImage.enabled = false;
            return;
        }

        Sprite icon = Resources.Load<Sprite>($"Icons/{iconName}");
        if (icon != null)
        {
            iconImage.sprite = icon;
            iconImage.enabled = true;
        }
        else
        {
            iconImage.enabled = false;
        }
    }

    void EndDialogue()
    {
        isDialogueActive = false;
        panel.SetActive(false);
        Time.timeScale = 1f;
    }
}
