using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DialogueLine
{
    public string message;     // 한 줄 대사
    public string iconName;    // 이 대사에 대응되는 아이콘 이름
}

[System.Serializable]
public class ItemMessage
{
    public string itemName;        // 아이템 이름 (Key)
    public List<DialogueLine> lines; // 대사 목록
}

[System.Serializable]
public class ItemMessageList
{
    public List<ItemMessage> messages;
}

public class MessageManager : MonoBehaviour
{
    public static MessageManager Instance;

    private Dictionary<string, ItemMessage> messageDict;

    void Awake()
    {
        Instance = this;
        LoadMessages();
    }

    void LoadMessages()
    {
        TextAsset jsonFile = Resources.Load<TextAsset>("Data/item_messages");
        if (jsonFile == null)
        {
            Debug.LogError("❌ item_messages.json 파일을 찾을 수 없습니다.");
            return;
        }

        string wrappedJson = "{\"messages\":" + jsonFile.text + "}";
        ItemMessageList list = JsonUtility.FromJson<ItemMessageList>(wrappedJson);

        messageDict = new Dictionary<string, ItemMessage>();
        foreach (var msg in list.messages)
        {
            messageDict[msg.itemName] = msg;
        }
    }

    /// <summary>
    /// 특정 아이템 이름으로 메시지 전체 (대사 목록) 가져오기
    /// </summary>
    public ItemMessage GetMessageData(string itemName)
    {
        if (messageDict != null && messageDict.TryGetValue(itemName, out ItemMessage messageData))
        {
            return messageData;
        }
        return null;
    }
}
