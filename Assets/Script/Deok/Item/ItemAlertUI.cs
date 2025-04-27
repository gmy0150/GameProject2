using UnityEngine;
using TMPro;

public class ItemAlertUI : MonoBehaviour
{
    public static ItemAlertUI Instance;

    public GameObject messagePanel;               // 🔸 Message UI 오브젝트
    public TextMeshProUGUI messageText;           // 🔸 안쪽 텍스트

    private void Awake()
    {
        Instance = this;

        if (messagePanel != null)
            messagePanel.SetActive(false); // 시작 시 꺼두기
    }

    public void ShowUIText(string msg)
    {
        if (messageText == null || messagePanel == null)
        {
            Debug.LogWarning("❗ messageText 또는 messagePanel이 연결되지 않았습니다!");
            return;
        }

        messageText.text = msg;
        messagePanel.SetActive(true); // 전체 UI 켜기

        CancelInvoke(nameof(HideMessage));
        Invoke(nameof(HideMessage), 1.5f); // 1.5초 후 자동 숨김
    }

    void HideMessage()
    {
        if (messagePanel != null)
            messagePanel.SetActive(false); // 전체 UI 끄기
    }
}
