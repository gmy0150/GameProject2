using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public abstract class StorageItem : InteractObject
{
    public Sprite icon;
    public string itemname;
    [SerializeField]
    protected bool isInteract = true;
    protected bool isActive;
    public override void Interact(Player character, IController controller)
    {
        base.Interact(character, controller);
        isInteract = false;
        InventoryManager.Instance.AddItemToInventory(this);
        Debug.Log("작동해줘");
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
        Debug.Log("1");
    }
    public override bool RotateInteract() => isActive;

    public override bool IsOneTimeInteraction()
    {
        return true;
    }
}
