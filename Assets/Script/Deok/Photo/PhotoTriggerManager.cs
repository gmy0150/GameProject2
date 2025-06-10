using System.Collections.Generic;
using UnityEngine;
using System;

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
        public string voiceName;
    }

    [System.Serializable]
    public class PhotoDialogueGroup
    {
        public string itemName;
        public List<PhotoLine> lines;
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

    public void ShowDialogueFromObjectName(string objectName, Action onCompleteCallback = null)
    {
        objectName = objectName.Replace("_", " ");

        foreach (var group in allGroups)
        {
            if (group.itemName == objectName)
            {
                if (objectName == "Picture complete")
                {
                    CameraAlertUI.Instance.ShowPhotoDialogue(group.lines, onCompleteCallback);
                    return;
                }

                bool wasFirstQuestCompleted = false;

                if (QuestManager.Instance != null && QuestManager.Instance.IsSecretObjectByName(objectName))
                {
                    wasFirstQuestCompleted = QuestManager.Instance.FoundSecret();
                }

                if (QuestManager.Instance != null && QuestManager.Instance.IsFinalPhotoTarget(objectName))
                {
                    QuestManager.Instance.CompleteFinalPhotoMission();
                }

                if (wasFirstQuestCompleted)
                {
                    CameraAlertUI.Instance.ShowPhotoDialogue(group.lines, () => {
                        QuestManager.Instance.TriggerFirstQuestCompletion();
                    });
                }
                else
                {
                    CameraAlertUI.Instance.ShowPhotoDialogue(group.lines, onCompleteCallback);
                }
                return;
            }
        }
        
        Debug.LogWarning("❌ [PTM] 일치하는 대사가 없습니다: " + objectName);
        onCompleteCallback?.Invoke();
    }
}