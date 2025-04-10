using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UseageInteract : InteractObject
{
    public override void Interact(Player character, IController controller)
    {
        base.Interact(character, controller);
        if (!isHide)
        {
            Hide();
        }
    }
    public override void InteractAgain()
    {
        if (isHide)
        {
            Hide();
        }

    }
    protected virtual void Hide(){}
    public override bool CanInteract()
    {
        return true;
        //throw new System.NotImplementedException(); 위 코드때문에 잠깐 주석처리 
    }



    public override bool IsOneTimeInteraction()
    {
        return false;
    }

    public override abstract void UpdateTime(float time);
}
