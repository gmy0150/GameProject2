using System.Collections.Generic;
using UnityEngine;

public class TutorialManager 
{
    [System.Serializable]
    public class DialogueLine
    {
        public string message;
        public string iconName;
    }

    [System.Serializable]
    public class DialogueGroup
    {
        public List<DialogueLine> lines;
    }

    [System.Serializable]
    public class DialogueWrapper
    {
        public DialogueGroup[] array;
    }

    public string jsonFileName = "Tutorial_Start";
    public int dialogueIndex = 0;

    public void Start()
    {
        TextAsset jsonText = Resources.Load<TextAsset>("Data/" + jsonFileName);
        if (jsonText != null)
        {
            DialogueWrapper wrapper = JsonUtility.FromJson<DialogueWrapper>("{\"array\":" + jsonText.text + "}");
            List<DialogueGroup> allGroups = new List<DialogueGroup>(wrapper.array);

            if (dialogueIndex >= 0 && dialogueIndex < allGroups.Count)
            {
                var linesToShow = allGroups[dialogueIndex].lines;
                TutorialUI.Instance.ShowTutorialDialogue(linesToShow);
            }
            else
            {
                Debug.LogError($"❌ dialogueIndex {dialogueIndex}가 JSON 배열 범위를 벗어났습니다.");
            }
        }
        else
        {
            Debug.LogError("❌ JSON 파일을 찾을 수 없습니다: Resources/Data/" + jsonFileName);
        }
    }

    // 필요하면 다른 클래스에서 대사를 공유할 수 있도록
    public static class DialogueDataBridge
    {
        public static List<DialogueLine> CurrentLines;
    }
}
