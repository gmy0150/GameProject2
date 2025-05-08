using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;

[RequireComponent(typeof(RectTransform))]
public class UIButtonEffect : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    private RectTransform buttonTransform;

    private void Awake()
    {
        buttonTransform = GetComponent<RectTransform>();
    }

    private void OnEnable()
    {
        if (buttonTransform == null)
            buttonTransform = GetComponent<RectTransform>();

        buttonTransform.DOKill(); // 이전 트윈 정리
        buttonTransform.localScale = Vector3.one; // 스케일 초기화
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!gameObject.activeInHierarchy || buttonTransform == null) return;

        Debug.Log($"[Enter] {gameObject.name}");
        buttonTransform.DOKill();
        buttonTransform.DOScale(1.1f, 0.2f).SetEase(Ease.OutQuad);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (!gameObject.activeInHierarchy || buttonTransform == null) return;

        Debug.Log($"[Exit] {gameObject.name}");
        buttonTransform.DOKill();
        buttonTransform.DOScale(1f, 0.2f).SetEase(Ease.OutQuad);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!gameObject.activeInHierarchy || buttonTransform == null) return;

        Debug.Log($"[Click] {gameObject.name}");

        buttonTransform.DOKill();
        buttonTransform
            .DOScale(0.9f, 0.1f)
            .SetEase(Ease.OutQuad)
            .OnComplete(() =>
            {
                buttonTransform.DOScale(1f, 0.1f).SetEase(Ease.OutQuad);
            });
    }
}
