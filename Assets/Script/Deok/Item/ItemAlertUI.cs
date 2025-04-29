using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ItemAlertUI : MonoBehaviour
{
    public static ItemAlertUI Instance;

    public GameObject messagePanel;               // 🔸 전체 패널 오브젝트
    public TextMeshProUGUI messageText;           // 🔸 텍스트 메시지
    public Image iconImage;                       // 🔸 아이템/표정 이미지

    private void Awake()
    {
        Instance = this;

        if (messagePanel != null)
            messagePanel.SetActive(false); // 시작 시 꺼두기
    }

    public void ShowUIText(string msg, Sprite icon = null)
    {
        if (messageText == null || messagePanel == null)
        {
            Debug.LogWarning("❗ messageText 또는 messagePanel이 연결되지 않았습니다!");
            return;
        }

        messageText.text = msg;

        if (iconImage != null && icon != null)
        {
            iconImage.sprite = icon;
            iconImage.enabled = true;
        }

        messagePanel.SetActive(true);

        CancelInvoke(nameof(HideMessage));
        Invoke(nameof(HideMessage), 1.5f);
    }

    void HideMessage()
    {
        if (messagePanel != null)
            messagePanel.SetActive(false);
    }
}
