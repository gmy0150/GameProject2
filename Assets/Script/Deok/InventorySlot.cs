using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class InventorySlot : MonoBehaviour
{
    public Image icon;
    public RectTransform slotTransform;

    private Tween currentTween;
    private StorageItem storedItem;
    void Start()
    {
        icon = GetComponent<Image>();
        slotTransform = GetComponent<RectTransform>();
    }
    public void SetItem(StorageItem item)
    {
        Debug.Log("?>");
        icon.sprite = item.icon;
        icon.enabled = true;
        storedItem = item;
    }
    void Update()
    {
        if(isActive && storedItem!=null){
            storedItem.UpdateTime(Time.deltaTime);
        }
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
    public void UseFilm()
    {
        if (storedItem != null)
        {
            storedItem.UseItem();
        }
    }
    public StorageItem GetItem(){
        return storedItem;
    }
    bool isActive;
    public void SetSelected(bool selected)
    {
        if (slotTransform == null)
            slotTransform = GetComponent<RectTransform>();
        if(selected)
            isActive = true;
            else
            isActive = false;
        currentTween?.Kill();

        float targetScale = selected ? 1.15f : 1f;
        currentTween = slotTransform.DOScale(targetScale, 0.2f).SetEase(Ease.OutQuad);
    }
}
