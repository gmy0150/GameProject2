using System.Collections.Generic;
using UnityEngine;

public class PhotoTriggerManager : MonoBehaviour
{
    public static PhotoTriggerManager Instance;

    public string jsonFileName = "Photo_messages";
    private List<PhotoDialogueGroup> allGroups;

    [System.Serializable]
    public class PhotoLine
    {
        public string message;
        public string iconName;
    }

    [System.Serializable]
    public class PhotoDialogueGroup
    {
        public string itemName;        // ex: "UFO 1"
        public List<PhotoLine> lines;  // multiple lines
    }

    [System.Serializable]
    public class DialogueWrapper
    {
        public PhotoDialogueGroup[] array;
    }

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            LoadDialogueData();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void LoadDialogueData()
    {
        TextAsset jsonText = Resources.Load<TextAsset>("Data/" + jsonFileName);
        if (jsonText != null)
        {
            var wrapper = JsonUtility.FromJson<DialogueWrapper>("{\"array\":" + jsonText.text + "}");
            allGroups = new List<PhotoDialogueGroup>(wrapper.array);
        }
        else
        {
            Debug.LogError("❌ JSON file not found: Resources/Data/" + jsonFileName);
        }
    }

    public void ShowDialogueFromObjectName(string objectName)
    {
        objectName = objectName.Replace("_", " ");  // 혹시 _ -> 공백 매핑

        Debug.Log("🔍 [PTM] 찾는 오브젝트 이름: " + objectName); // ✅ 로그 ②

        foreach (var group in allGroups)
        {
            Debug.Log("📝 [PTM] 현재 JSON 항목: " + group.itemName); // ✅ 로그 ③

            if (group.itemName == objectName)
            {
                Debug.Log("✅ [PTM] 일치하는 대사 찾음!"); // ✅ 로그 ④
                CameraAlertUI.Instance.ShowPhotoDialogue(group.lines);
                return;
            }
        }

        Debug.LogWarning("❌ [PTM] 일치하는 대사가 없습니다: " + objectName); // ✅ 로그 ⑤
    }

}