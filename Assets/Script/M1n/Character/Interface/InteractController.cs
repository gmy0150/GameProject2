using Pathfinding.Examples;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class InteractController 
{
    public float interactionDistance = 5.0f; 
    public LayerMask interactableLayer;
    Player character;
    IController keyboardController;
    bool isInteracting = false;
    private IInterActerable currentInteractable;

    public void OnPosessed(Player controllerableCharacter)
    {
        character = controllerableCharacter;
        keyboardController = character.GetKey();
        interactableLayer = character.Layer;
    }
    public void TIck(float deltaTime)
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (currentInteractable != null && !currentInteractable.IsOneTimeInteraction())
            {
                currentInteractable.InteractAgain();
                currentInteractable = null;
            }
            else
            {
                TryInteract();
            }
        }
    }
    IInterActerable interactable;
    public bool GetInterAct()
    {
        return isInteracting;
    }
    void TryInteract()
    {
        Collider[] colliders = Physics.OverlapSphere(character.transform.position, interactionDistance, interactableLayer);
        IInterActerable bestInteractable = null;

        float minDistance = interactionDistance;
        foreach (Collider col in colliders)
        {
            Vector3 dirToTarget = (col.transform.position - character.transform.position).normalized;
            float angle = Vector3.Angle(character.transform.forward, dirToTarget);
            float distance = Vector3.Distance(character.transform.position, col.transform.position);
 

            if (angle < 60f && distance < minDistance)
            {
                IInterActerable candidate = col.GetComponent<IInterActerable>();
                if (candidate != null && candidate.CanInteract())
                {
                    InventoryManager.Instance.InitSlot();

                    bestInteractable = candidate;
                    minDistance = distance;
                }
            }
        }

        if (bestInteractable != null)
        {
            bestInteractable.Interact(character, keyboardController);
            
            // 일회성이 아닌 상호작용이면 현재 상호작용 중인 오브젝트로 저장
            if (!bestInteractable.IsOneTimeInteraction())
            {
                currentInteractable = bestInteractable;
            }
        }
    }
    public bool IsInteracting()
    {
        return currentInteractable != null && !currentInteractable.IsOneTimeInteraction();
    }
    public bool GetHide()
    {
        if(currentInteractable != null)
        {
            Debug.Log(currentInteractable.GetHide());
            return currentInteractable.GetHide();
        }
        else
        {
            return false;
        }
    }

    public void ResetInteraction()
    {
        if (currentInteractable != null)
        {
            currentInteractable.InteractAgain();//�ٽ� �Է�
            isInteracting = false;
        }
    }
    public bool RotateInteract(){
        var slot = InventoryManager.Instance.GetActiveItem();
        if(slot != null){
            return slot.RotateInteract();
        }
        return true;
    }
    
}
