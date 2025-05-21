using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Closet : UseageInteract
{
    GameObject door1, door2;
    void Start()
    {
        door1 = transform.GetChild(1).gameObject;
        door2 = transform.GetChild(2).gameObject;
        
    }
    public override void Interact(Player character, IController controller)
    {
        base.Interact(character, controller);
        InventoryManager.Instance.ExitSlot();
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
            Render(false);
            character.ControllerDisable();
            door1.transform.localRotation = Quaternion.Euler(0, 90, 0);
            door2.transform.localRotation = Quaternion.Euler(0, -90, 0);
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
