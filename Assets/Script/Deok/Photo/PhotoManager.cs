using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PhotoDialogueData
{
    public string targetName;
    public List<DialogueLine> lines; // ✅ 수정됨
}

[System.Serializable]
public class PhotoDialogueWrapper
{
    public List<PhotoDialogueData> data;
}

public class PhotoManager : MonoBehaviour
{
    public static PhotoManager Instance;

    public TextAsset jsonFile;
    private Dictionary<string, List<DialogueLine>> dialogueDict;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        var wrapped = JsonUtility.FromJson<PhotoDialogueWrapper>("{\"data\":" + jsonFile.text + "}");

        dialogueDict = new Dictionary<string, List<DialogueLine>>();
        foreach (var entry in wrapped.data)
        {
            dialogueDict[entry.targetName] = entry.lines;
        }
    }

    public void ShowDialogue(string targetName)
    {
        if (dialogueDict.TryGetValue(targetName, out var lines))
        {
            PhotoUI.Instance.StartDialogue(lines);
        }
        else
        {
            PhotoUI.Instance.StartDialogue(new List<DialogueLine> {
                new DialogueLine { message = "해당 대상에 대한 정보가 없습니다.", iconName = "confused" }
            });
        }
    }
}
