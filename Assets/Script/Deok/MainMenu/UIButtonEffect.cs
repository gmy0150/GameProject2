using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;

public class UIButtonEffect : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private RectTransform buttonTransform;

    void Start()
    {
        // 현재 오브젝트의 RectTransform을 자동으로 가져옴.
        buttonTransform = GetComponent<RectTransform>();

        // 디버깅 로그 추가 ( 정확한 오류 어디서 발생했는지 가능함. )
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

    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log($"버튼 효과 실행됨! {gameObject.name}");
        buttonTransform.DOScale(1.1f, 0.2f).SetEase(Ease.OutQuad);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Debug.Log($"버튼 효과 실행됨! {gameObject.name}");
        buttonTransform.DOScale(1f, 0.2f).SetEase(Ease.OutQuad);
    }

    public void OnClick()
    {
        Debug.Log($"버튼 효과 실행됨! {gameObject.name}");
        buttonTransform
            .DOScale(0.9f, 0.1f)
            .SetEase(Ease.OutQuad)
            .OnComplete(() => buttonTransform.DOScale(1f, 0.1f));
    }
}
