using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public abstract class StorageItem : InteractObject
{
    public LayerMask interact;
    protected GameObject HandAnything;

    public Sprite icon;
    public string itemname;
    [SerializeField]
    protected bool isInteract = true;
    protected bool isActive;

    [SerializeField] protected float interactDis;
    protected Mesh Base;
    protected MeshFilter filter;
    public Sprite cursorImage;
    public Sprite cusorInterectImage;
    public override void Interact(Player character, IController controller)
    {
        base.Interact(character, controller);
        isInteract = false;
        InventoryManager.Instance.AddItemToInventory(this);
        filter = GetComponent<MeshFilter>();
        Base = filter.mesh;
        filter.mesh = null;
    }
    public abstract void inititem();
    public override void InteractAgain()
    {
        isInteract = false;
        InventoryManager.Instance.RemoveItem();
        isActive = true;
    }
    public abstract void UseItem();

    public override bool CanInteract()
    {
        return isInteract;
    }

    public override void UpdateTime(float time)
    {
        isActive = false;

    }
    public override bool RotateInteract() => isActive;

    public override bool IsOneTimeInteraction()
    {
        return true;
    }
    public void SetHandActive(bool isActive)
    {
        HandAnything.gameObject.SetActive(isActive);
    }
    public abstract void ActiveFalse();
}
