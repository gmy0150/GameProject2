using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DialogueLine
{
    public string message;
    public string iconName;
}

[System.Serializable]
public class ItemMessage
{
    public string itemName;
    public List<DialogueLine> lines;
}

[System.Serializable]
public class ItemMessageList
{
    public List<ItemMessage> messages;
}

public class MessageManager : MonoBehaviour
{
    public static MessageManager Instance;

    private Dictionary<string, ItemMessage> itemMessageDict;
    private Dictionary<string, ItemMessage> photoMessageDict;

    void Awake()
    {
        Instance = this;
        LoadItemMessages();
        LoadPhotoMessages();
    }

    void LoadItemMessages()
    {
        TextAsset jsonFile = Resources.Load<TextAsset>("Data/item_messages");
        if (jsonFile == null)
        {
            Debug.LogError("❌ item_messages.json 파일을 찾을 수 없습니다.");
            return;
        }

        string wrappedJson = "{\"messages\":" + jsonFile.text + "}";
        ItemMessageList list = JsonUtility.FromJson<ItemMessageList>(wrappedJson);

        itemMessageDict = new Dictionary<string, ItemMessage>();
        foreach (var msg in list.messages)
        {
            itemMessageDict[msg.itemName] = msg;
        }
    }

    void LoadPhotoMessages()
    {
        TextAsset jsonFile = Resources.Load<TextAsset>("Data/photo_messages");
        if (jsonFile == null)
        {
            Debug.LogWarning("📷 photo_messages.json 파일이 없어 사진 메시지는 비활성화됩니다.");
            return;
        }

        string wrappedJson = "{\"messages\":" + jsonFile.text + "}";
        ItemMessageList list = JsonUtility.FromJson<ItemMessageList>(wrappedJson);

        photoMessageDict = new Dictionary<string, ItemMessage>();
        foreach (var msg in list.messages)
        {
            photoMessageDict[msg.itemName] = msg;
        }
    }

    public ItemMessage GetItemMessage(string itemName)
    {
        if (itemMessageDict != null && itemMessageDict.TryGetValue(itemName, out var data))
        {
            return data;
        }
        return null;
    }

    public ItemMessage GetPhotoMessage(string targetName)
    {
        if (photoMessageDict != null && photoMessageDict.TryGetValue(targetName, out var data))
        {
            return data;
        }
        return null;
    }
}
