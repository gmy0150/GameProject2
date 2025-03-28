using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Box : UseageInteract
{
    float TransTimer;

    public override void Interact(Player character, IController controller)
    {
        base.Interact(character, controller);
        TransBox();
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
                CancelTransformation();

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
