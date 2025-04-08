using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInterActerable 
{
    void Interact(Player character,IController controller);
    void InteractAgain();
    public bool isShoot();
    public bool isCoin();
    public bool GetHide();

}
