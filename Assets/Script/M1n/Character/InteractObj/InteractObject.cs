using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class InteractObject :  MonoBehaviour, IInterActerable
{
    protected bool isHide = false;
    protected Player character;
    protected IController controller;
    public bool shoot = false;
    public bool hasCoin = false;
    // bool CanInteract = false;

    public bool GetHide()
    {
        return this.isHide;
    }

    public virtual void Interact(Player character, IController controller)
    {
        this.character = character;
        this.controller = controller;
    }

    public abstract void InteractAgain();

    public abstract bool CanInteract();
    public abstract bool IsOneTimeInteraction();

    public abstract void UpdateTime(float time);

    public virtual bool RotateInteract()
    {
        return true;
    }
}
