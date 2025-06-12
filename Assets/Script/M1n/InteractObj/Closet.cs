using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Closet : UseageInteract
{
    GameObject door1, door2;
    Quaternion door1Rotation, door2Rotation;
    void Start()
    {
        base.Start();
        door1 = transform.GetChild(1).gameObject;
        door2 = transform.GetChild(2).gameObject;
        door1Rotation = door1.transform.rotation;
        door2Rotation = door2.transform.rotation;
    }
    public override void Interact(Player character, IController controller)
    {
        base.Interact(character, controller);
        if (!isHide)
        {
            Hide();
        }
        InventoryManager.Instance.ExitSlot();
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
        character.SetAudio("Closet");
        if (isHide)
        {
            Rigidbody rigidbody = character.GetComponent<Rigidbody>();
            rigidbody.velocity = Vector3.zero;
            controller.RunningCancel();
            Render(false);
            character.ControllerDisable();
            // door1.transform.rotation = Quaternion.Euler(door1.transform.rotation.x, door1.transform.rotation.x - 90, door1.transform.rotation.z);
            // door2.transform.rotation = Quaternion.Euler(door1.transform.rotation.x, door1.transform.rotation.x + 90, door1.transform.rotation.z);
            // Debug.Log(door1.transform.localRotation.x);
        }
        else
        {
            Render(true);
            character.ControllerEnable();

            // door1.transform.rotation = door1Rotation;
            // door2.transform.rotation = door2Rotation;
        }
        
    }
    protected void Render(bool x)
    {
        InventoryManager.Instance.GetActiveItem()?.SetHandActive(x);
        controller.SetNoise(x);
        character.GetComponentInChildren<SkinnedMeshRenderer>().enabled = x;
        character.GetComponentInChildren<Collider>().enabled = x;
        character.GetComponent<Rigidbody>().useGravity = x;
    }

}
