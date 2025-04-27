using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ItemMessage
{
    public string itemName;
    public string message;
}

[System.Serializable]
public class ItemMessageList
{
    public List<ItemMessage> messages;
}

public class MessageManager : MonoBehaviour
{
    public static MessageManager Instance;

    private Dictionary<string, string> messageDict;

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

        messageDict = new Dictionary<string, string>();
        foreach (var msg in list.messages)
        {
            messageDict[msg.itemName] = msg.message;
        }
    }

    public string GetMessage(string itemName)
    {
        if (messageDict != null && messageDict.TryGetValue(itemName, out string message))
        {
            return message;
        }
        return null;
    }
}
