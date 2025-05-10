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
    public class DialogueWrapper
    {
        public DialogueLine[] array;
    }

    public string jsonFileName = "Tutorial_Start";

    public void Start()
    {
        TextAsset jsonText = Resources.Load<TextAsset>("Data/" + jsonFileName);
        if (jsonText != null)
        {
            DialogueWrapper wrapper = JsonUtility.FromJson<DialogueWrapper>("{\"array\":" + jsonText.text + "}");
            List<DialogueLine> lines = new List<DialogueLine>(wrapper.array);

            // ✅ 내부 타입 그대로 넘기는 전용 함수 사용
            TutorialUI.Instance.ShowTutorialDialogue(lines);
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
