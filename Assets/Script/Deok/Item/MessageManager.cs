using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ItemMessage
{
    public string itemName;
    public string message;
    public string iconName; // 아이콘 파일명 (Resources/Icon/ 에 있어야 함)
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

    public string GetMessage(string itemName)
    {
        if (messageDict != null && messageDict.TryGetValue(itemName, out ItemMessage messageData))
        {
            return messageData.message;
        }
        return null;
    }

    public Sprite GetMessageIcon(string itemName)
    {
        if (messageDict != null && messageDict.TryGetValue(itemName, out ItemMessage messageData))
        {
            if (!string.IsNullOrEmpty(messageData.iconName))
            {
                return Resources.Load<Sprite>("Icon/" + messageData.iconName);
            }
        }
        return null;
    }

    // ✅ 아이콘과 메시지를 한번에 가져오고 싶을 때
    public ItemMessage GetMessageData(string itemName)
    {
        if (messageDict != null && messageDict.TryGetValue(itemName, out ItemMessage messageData))
        {
            return messageData;
        }
        return null;
    }
}
