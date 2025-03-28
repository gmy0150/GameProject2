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
        Debug.Log(controllerableCharacter);
        keyboardController = character.GetKey();
        interactableLayer = character.Layer;
        Debug.Log("aa");
        Debug.Log(interactableLayer);
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
    void TryInteract()
    {
        RaycastHit hit;
        // 플레이어 앞에 레이캐스트를 쏴서 상호작용 가능한 오브젝트를 찾음
        Vector3 trans = character.transform.position;
        float newy = 0.2f;
        trans.y = newy;
        if (Physics.Raycast(trans, character.transform.forward, out hit, interactionDistance, interactableLayer))
        {
            interactable = hit.collider.GetComponent<UseageInteract>();
            if (interactable != null  )
            {
                Debug.Log("aaa");
                interactable.Interact(character,keyboardController);  // 상호작용 실행
                isInteracting = true;
            }
        }
    }
    void ResetInteraction()
    {
        if (interactable != null)
        {
            interactable.InteractAgain();//다시 입력
            isInteracting = false;
        }
    }
}
