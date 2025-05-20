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

    public string jsonFileName = "Game_Start";

    void Start()
    {
        TextAsset jsonText = Resources.Load<TextAsset>("Data/" + jsonFileName);
        if (jsonText != null)
        {
            DialogueWrapper wrapper = JsonUtility.FromJson<DialogueWrapper>("{\"array\":" + jsonText.text + "}");
            List<DialogueLine> lines = new List<DialogueLine>(wrapper.array);

            // ✅ 내부 타입 그대로 넘기는 전용 함수 사용
            GameTutorialUI.Instance.ShowGameDialogue(lines);
        }
        else
        {
            Debug.LogError("❌ JSON 파일을 찾을 수 없습니다: " + jsonFileName);
        }
    }

    // 다른 스크립트에서도 쓰고 싶으면 여기에 공유해도 됨
    public static class DialogueDataBridge
    {
        public static List<DialogueLine> CurrentLines;
    }
}

    /*public GameObject dialoguePanel;
    public TextMeshProUGUI dialogueText;
    public Image dialogueIcon;

    private List<DialogueLine> lines;
    private int currentLineIndex = 0;
    private bool isDialogueActive = false;

    void Start()
    {
        // JSON 파일 로드
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

        // 씬이 로드되자마자 자동으로 대화 시작
        if (lines != null && lines.Count > 0)
        {
            StartCoroutine(StartDialogue());
        }
    }

    void Update()
    {
        // 대화 진행중일 때 클릭하면 다음 메시지 표시
        if (isDialogueActive && Input.GetMouseButtonDown(0))
        {
            ShowNextLine();
        }
    }

    IEnumerator StartDialogue()
    {
        isDialogueActive = true;
        currentLineIndex = 0;
        dialoguePanel.SetActive(true);

        yield return new WaitForEndOfFrame();
        ShowDialogueLine(lines[currentLineIndex]);
    }

    void ShowNextLine()
    {
        currentLineIndex++;

        if (currentLineIndex < lines.Count)
        {
            ShowDialogueLine(lines[currentLineIndex]);
        }
        else
        {
            EndDialogue();
        }
    }

    void ShowDialogueLine(DialogueLine line)
    {
        dialogueText.text = line.message;
        dialogueIcon.sprite = Resources.Load<Sprite>("Icons/" + line.iconName);
    }

    void EndDialogue()
    {
        isDialogueActive = false;
        dialoguePanel.SetActive(false);
    }
}
    */