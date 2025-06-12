using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Box : StorageItem
{
    float TransTimer;
    Mesh BaseMesh;
    Material BaseMaterial;
    public override void Interact(Player character, IController controller)
    {
        base.Interact(character, controller);
        // character.GetInterActControll().ResetInteraction();
        Debug.Log("작동");
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
        BaseMaterial = skined.material;
        isHide = true;
        isInteract = false;
        controller.RunningCancel();
        skined.sharedMesh = character.BoxMesh;
        skined.material = character.BoxMesh.GetComponent<MeshRenderer>().material;
        
    }
    void CancelTransformation()
    {

        isHide = false;
        isInteract = true;
        SkinnedMeshRenderer skined = character.GetComponentInChildren<SkinnedMeshRenderer>();
        skined.sharedMesh = baseMesh;
        skined.material = baseMaterial;
        TransTimer = 0;
    }

    public override void UseItem()
    {
        TransBox();
            InventoryManager.Instance.GetSlot().ClearItem();
    }

    public override void inititem()
    {
        
    }

        public override void ActiveFalse()
    {
        SetHandActive(false);
        character.cursorUI.SetCursorImage();
    }
}
