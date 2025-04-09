using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Box : StorageItem
{
    float TransTimer;

    public override void Interact(Player character, IController controller)
    {
        base.Interact(character, controller);
        character.GetInterActControll().ResetInteraction();
    }
    
    public override void InteractAgain()
    {
        CancelTransformation();
    }
    private void Update()
    {
        if (isHide)
        {
            TransTimer += Time.deltaTime;
            if (TransTimer > 10)
            {
            }
        }
    }
    void TransBox()
    {
        isHide = true;
        controller.Crouch();


        SkinnedMeshRenderer skined = character.GetComponentInChildren<SkinnedMeshRenderer>();
        skined.sharedMesh = character.BoxMesh;
        
    }
    void CancelTransformation()
    {

        isHide = false;
        controller.CrouchCancel();
        SkinnedMeshRenderer skined = character.GetComponentInChildren<SkinnedMeshRenderer>();
        skined.sharedMesh = character.BaseMesh;
        TransTimer = 0;
    }
}
