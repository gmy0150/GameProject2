using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInterActerable 
{
    void Interact(Player character,IController controller);
    void InteractAgain();
    public bool CanInteract();
    public bool RotateInteract();
    public bool GetHide();

}
