using System.Collections;
using System.Collections.Generic;
using EPOOutline;
using UnityEngine;

[RequireComponent(typeof(Outlinable))]
public abstract class InteractObject :  MonoBehaviour, IInterActerable
{
    protected bool isHide = false;
    protected Player character;
    protected IController controller;
    public bool shoot = false;
    public bool hasCoin = false;
    // bool CanInteract = false;

    void Start()
    {
        Outlinable outlinable = GetComponent<Outlinable>();
        // outlinable.RenderStyle = RenderStyle.FrontBack;
        outlinable.OutlineParameters.Enabled = false;
        // outlinable.BackParameters.Enabled = false;
    }
    public virtual bool GetHide()
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

    public virtual void Init()
    {
    }
}
