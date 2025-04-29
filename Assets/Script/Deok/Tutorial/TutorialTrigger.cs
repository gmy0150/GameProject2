using System.Collections.Generic;
using UnityEngine;

public class TutorialTrigger : MonoBehaviour
{
    public string jsonFileName = "Tutorial_Continue"; // 사용할 JSON 파일명
    public int dialogueIndex = 0; // 몇 번째 대사를 보여줄지 (0부터 시작)
    private bool hasTriggered = false;

    private void OnTriggerEnter(Collider other)
    {
        if (hasTriggered) return;
        if (!other.CompareTag("Player")) return;

        hasTriggered = true;

        TextAsset jsonText = Resources.Load<TextAsset>("Data/" + jsonFileName);
        if (jsonText != null)
        {
            var wrapper = JsonUtility.FromJson<DialogueWrapper>("{\"array\":" + jsonText.text + "}");
            var allLines = new List<TutorialManager.DialogueLine>(wrapper.array);

            if (dialogueIndex >= 0 && dialogueIndex < allLines.Count)
            {
                // ✅ 특정 인덱스만 골라서 리스트로 만들어 전달
                var selectedLineList = new List<TutorialManager.DialogueLine> { allLines[dialogueIndex] };
                TutorialUI.Instance.ShowTutorialDialogue(selectedLineList);
            }
            else
            {
                Debug.LogError($"❌ 대사 인덱스 {dialogueIndex}가 범위를 벗어났습니다.");
            }
        }
        else
        {
            Debug.LogError("❌ JSON 파일을 찾을 수 없습니다: Resources/Data/" + jsonFileName);
        }
    }

    [System.Serializable]
    public class DialogueWrapper
    {
        public TutorialManager.DialogueLine[] array;
    }
}
