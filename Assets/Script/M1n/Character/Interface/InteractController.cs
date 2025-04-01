using Pathfinding.Examples;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractController 
{
    public float interactionDistance = 5.0f; 
    public LayerMask interactableLayer;
    Player character;
    IController keyboardController;
    bool isInteracting = false;

    public void OnPosessed(Player controllerableCharacter)
    {
        character = controllerableCharacter;
        keyboardController = character.GetKey();
        interactableLayer = character.Layer;
    }
    public void TIck(float deltaTime)
    {
        if (!isInteracting && Input.GetKeyDown(KeyCode.E))
        {
            TryInteract();
        }
        else if (isInteracting && Input.GetKeyDown(KeyCode.E))
        {
            ResetInteraction();
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
        interactable = null;

        float mindistance = interactionDistance;
        foreach (Collider col in colliders)
        {
            Vector3 dirToTarget = (col.transform.position - character.transform.position).normalized;
            float angle = Vector3.Angle(character.transform.forward, dirToTarget);
            float distance = Vector3.Distance(character.transform.position, col.transform.position);


            if (angle < 60f && distance < mindistance)
            {
                UseageInteract candiate = col.GetComponent<UseageInteract>();
                if (candiate != null)
                {
                    interactable = candiate;
                    mindistance = distance;
                }
                
            }
        }
        if (interactable != null && !GetCoin())
        {
            interactable.Interact(character, keyboardController);
            isInteracting = true;
        }
    }
    public bool GetHide()
    {
        if(interactable != null)
        {
            return interactable.GetHide();
        }
        else
        {
            return false;
        }
    }
    public bool GetCoin()
    {
        if (interactable != null)
        {
            return interactable.isCoin();
        }
        else
        {
            return false;
        }
    }
    public void ResetInteraction()
    {
        if (interactable != null)
        {
            interactable.InteractAgain();//다시 입력
            isInteracting = false;
        }
    }

}
