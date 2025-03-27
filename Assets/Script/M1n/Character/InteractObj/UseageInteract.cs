using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UseageInteract : MonoBehaviour, IInterActerable
{
    protected bool isHide = false;
    protected Player character;
    protected IController controller;

    public virtual void Interact(Player character, IController controller)
    {
        this.character = character;
        this.controller = controller;
    }

    public virtual void InteractAgain()
    {
        throw new System.NotImplementedException();
    }

    



}
