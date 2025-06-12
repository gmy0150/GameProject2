using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Box : StorageItem
{
    float TransTimer;
    Mesh BaseMesh;
    Material[] BaseMaterial;
    public override void Interact(Player character, IController controller)
    {
        base.Interact(character, controller);
        HandAnything = character.Hammer;
        Debug.Log("작동");
        SetHandActive(false);
    }
    
    public override void InteractAgain()
    {
        base.InteractAgain();
        CancelTransformation();
    }
    private void Update()
    {
        if (isHide)
        {
            controller.RunningCancel();
            TransTimer += Time.deltaTime;
            if (TransTimer > 10)
            {
                InteractAgain();
            }
            if(Input.GetKeyDown(KeyCode.E))
                InteractAgain();
        }
    }

    void TransBox()
    {
        SkinnedMeshRenderer skined = character.GetComponentInChildren<SkinnedMeshRenderer>();
        BaseMesh = skined.sharedMesh;
        BaseMaterial = skined.materials;
        isHide = true;
        isInteract = false;
        controller.RunningCancel();
        skined.sharedMesh = character.BoxMesh;
        skined.materials = new Material[] { character.BoxMaterial };
        character.GetInterActControll().GetHide();
        character.GetInterActControll().SetInteractable(this);
        Debug.Log("???작동");
    }
    void CancelTransformation()
    {

        isHide = false;
        isInteract = true;
        SkinnedMeshRenderer skined = character.GetComponentInChildren<SkinnedMeshRenderer>();
        skined.sharedMesh = BaseMesh;
        skined.materials = BaseMaterial;
        TransTimer = 0;
        character.GetInterActControll().SetInteractable(null);

    }

    public override void UseItem()
    {
        TransBox();
            InventoryManager.Instance.GetSlot().ClearItem();
    }

    public override void inititem()
    {
        HandAnything = character.Hammer;
    }

    public override void ActiveFalse()
    {
        SetHandActive(false);
        character.cursorUI.SetCursorImage();
        
    }
}
