using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Closet : UseageInteract
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
    protected void Hide()
    {
        isHide = !isHide;
        if (isHide)
        {
            Rigidbody rigidbody = character.GetComponent<Rigidbody>();
            rigidbody.velocity = Vector3.zero;
            controller.Crouch();
            Render(false);
            character.ControllerDisable();
            Debug.Log("¿ÊÀå¿¡ ¼û");
        }
        else
        {
            controller.CrouchCancel();
            Render(true);
            character.ControllerEnable();

            Debug.Log("¿ÊÀå¿¡ ³ª¿È");
        }
    }
    protected void Render(bool x)
    {
        controller.SetNoise(x);
        
        character.GetComponentInChildren<SkinnedMeshRenderer>().enabled = x;
        character.GetComponentInChildren<Collider>().enabled = x;
        character.GetComponent<Rigidbody>().useGravity = x;
    }

}
