using UnityEngine;
using UnityEngine.EventSystems; // Pointer 이벤트를 위해 추가
using DG.Tweening;

public class UIButtonEffect : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private RectTransform buttonTransform;

    void Start()
    {
        // 현재 오브젝트의 RectTransform을 자동으로 가져오기
        buttonTransform = GetComponent<RectTransform>();

        // 디버깅 로그 추가
        if (buttonTransform == null)
        {
            Debug.LogError($"UIButtonEffect: RectTransform을 찾을 수 없습니다! " +
                           $"오브젝트 이름: {gameObject.name}, " +
                           $"부모 오브젝트: {(transform.parent != null ? transform.parent.name : "없음")} " +
                           $"해당 스크립트가 UI 버튼에 붙어 있는지 확인하세요.");
        }
        else
        {
            Debug.Log($"UIButtonEffect: {gameObject.name}의 RectTransform을 정상적으로 가져옴.");
        }
    }

    // 마우스를 올렸을 때
    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log($"OnPointerEnter 실행됨! {gameObject.name}");
        buttonTransform.DOScale(1.1f, 0.2f).SetEase(Ease.OutQuad);
    }

    // 마우스를 뗐을 때
    public void OnPointerExit(PointerEventData eventData)
    {
        Debug.Log($"OnPointerExit 실행됨! {gameObject.name}");
        buttonTransform.DOScale(1f, 0.2f).SetEase(Ease.OutQuad);
    }

    // 버튼 클릭 효과
    public void OnClick()
    {
        Debug.Log($"버튼 클릭! {gameObject.name}");
        buttonTransform
            .DOScale(0.9f, 0.1f)
            .SetEase(Ease.OutQuad)
            .OnComplete(() => buttonTransform.DOScale(1f, 0.1f));
    }
}
