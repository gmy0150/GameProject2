using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class InventorySlot : MonoBehaviour
{
    public Image icon;
    public RectTransform slotTransform;

    private Tween currentTween;
    private InterItem storedItem;

    public void SetItem(Sprite itemIcon, InterItem item)
    {
        if (icon == null)
        {
            Debug.LogError("InventorySlot: icon 연결안됨.");
            return;
        }

        icon.sprite = itemIcon;
        icon.enabled = true;
        storedItem = item;
    }

    public void ClearItem()
    {
        if (icon != null)
        {
            icon.sprite = null;
            icon.enabled = false;
        }

        storedItem = null;
        SetSelected(false);
    }

    public bool HasItem()
    {
        return storedItem != null;
    }

    public void UseItem()
    {
        if (storedItem != null)
        {
            storedItem.UseItem();
            ClearItem();
        }
    }

    public void SetSelected(bool selected)
    {
        if (slotTransform == null)
            slotTransform = GetComponent<RectTransform>();

        currentTween?.Kill();

        float targetScale = selected ? 1.15f : 1f;
        currentTween = slotTransform.DOScale(targetScale, 0.2f).SetEase(Ease.OutQuad);
    }
}
