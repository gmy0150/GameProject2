using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameStart : MonoBehaviour 
{
    [System.Serializable]
    public class DialogueLine
    {
        public string message;
        public string iconName;
    }

    [System.Serializable]
    public class DialogueWrapper
    {
        public DialogueLine[] array;
    }

    public string jsonFileName = "Tutorial_Start";

    public GameObject dialoguePanel;
    public TextMeshProUGUI dialogueText;
    public Image dialogueIcon;

    public float typingSpeed = 0.05f; // 글자 타이핑 속도 조절 (값이 작을수록 빠름)

    private List<DialogueLine> lines;
    private int currentLineIndex = 0;
    private bool isDialogueActive = false;
    private bool isTyping = false;

    void Start()
    {
        TextAsset jsonText = Resources.Load<TextAsset>("Data/" + jsonFileName);
        if (jsonText != null)
        {
            DialogueWrapper wrapper = JsonUtility.FromJson<DialogueWrapper>("{\"array\":" + jsonText.text + "}");
            lines = new List<DialogueLine>(wrapper.array);
        }
        else
        {
            Debug.LogError("JSON 파일을 찾을 수 없습니다.");
        }

        dialoguePanel.SetActive(false);

        if (lines != null && lines.Count > 0)
        {
            StartCoroutine(StartDialogue());
        }
    }

    void Update()
    {
        if (isDialogueActive && Input.GetKeyDown(KeyCode.Space))
        {
            if (isTyping)
            {
                // 타이핑 중이면 전체 텍스트를 즉시 표시
                StopAllCoroutines();
                dialogueText.text = lines[currentLineIndex].message;
                isTyping = false;
            }
            else
            {
                ShowNextLine();
            }
        }
    }

    IEnumerator StartDialogue()
    {
        isDialogueActive = true;
        currentLineIndex = 0;
        dialoguePanel.SetActive(true);

        yield return new WaitForEndOfFrame();
        StartCoroutine(TypeLine(lines[currentLineIndex]));
    }

    void ShowNextLine()
    {
        currentLineIndex++;

        if (currentLineIndex < lines.Count)
        {
            StartCoroutine(TypeLine(lines[currentLineIndex]));
        }
        else
        {
            EndDialogue();
        }
    }

    IEnumerator TypeLine(DialogueLine line)
    {
        isTyping = true;
        dialogueText.text = "";
        dialogueIcon.sprite = Resources.Load<Sprite>("Icons/" + line.iconName);

        foreach (char letter in line.message.ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }

        isTyping = false;
    }

    void EndDialogue()
    {
        isDialogueActive = false;
        dialoguePanel.SetActive(false);
    }
}
