using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class UIButtonSoundBinder : MonoBehaviour
{
    public enum SoundType { Click1, Click2 }
    public SoundType soundType = SoundType.Click1;

    private void Awake()
    {
        var btn = GetComponent<Button>();

        if (UISoundPlayer.Instance == null)
        {
            Debug.LogWarning("[UIButtonSoundBinder] UISoundPlayer 인스턴스를 찾을 수 없습니다.");
            return;
        }

        // 기존 리스너 제거 (중복 방지)
        btn.onClick.RemoveListener(UISoundPlayer.Instance.PlayClickSound);
        btn.onClick.RemoveListener(UISoundPlayer.Instance.PlayClickSound2);

        // 사운드 타입에 따라 연결
        switch (soundType)
        {
            case SoundType.Click1:
                btn.onClick.AddListener(UISoundPlayer.Instance.PlayClickSound);
                break;
            case SoundType.Click2:
                btn.onClick.AddListener(UISoundPlayer.Instance.PlayClickSound2);
                break;
        }
    }
}
