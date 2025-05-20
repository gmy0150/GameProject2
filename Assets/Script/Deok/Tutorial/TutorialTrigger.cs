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

        Debug.Log("✅ 플레이어가 트리거에 진입했습니다.");

        hasTriggered = true;

        TextAsset jsonText = Resources.Load<TextAsset>("Data/" + jsonFileName);
        if (jsonText != null)
        {
            var wrapper = JsonUtility.FromJson<DialogueWrapper>("{\"array\":" + jsonText.text + "}");
            var allGroups = new List<DialogueGroup>(wrapper.array);

            if (dialogueIndex >= 0 && dialogueIndex < allGroups.Count)
            {
                var selectedLines = allGroups[dialogueIndex].lines;
                TutorialUI.Instance.ShowTutorialDialogue(selectedLines);
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
        public DialogueGroup[] array;
    }

    [System.Serializable]
    public class DialogueGroup
    {
        public List<TutorialManager.DialogueLine> lines;
    }
}
