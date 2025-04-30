using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Closet : UseageInteract
{
    public override void Interact(Player character, IController controller)
    {
        base.Interact(character, controller);
        InventoryManager.Instance.ExitSlot();
        if (!isHide)
        {
            Debug.Log("작동하자");
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

    public override void UpdateTime(float time)
    {
        return;
    }

    protected override void Hide()
    {
        isHide = !isHide;
        if (isHide)
        {
            Rigidbody rigidbody = character.GetComponent<Rigidbody>();
            rigidbody.velocity = Vector3.zero;
            controller.RunningCancel();
            // controller.Crouch();
            Render(false);
            character.ControllerDisable();
        }
        else
        {
            Render(true);
            character.ControllerEnable();

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
