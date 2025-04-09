using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UseageInteract : InteractObject
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
        throw new System.NotImplementedException();
    }

    public override bool RotateInteract()
    {
        throw new System.NotImplementedException();
    }
}
