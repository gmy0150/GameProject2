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
        // �÷��̾� �տ� ����ĳ��Ʈ�� ���� ��ȣ�ۿ� ������ ������Ʈ�� ã��
        Vector3 trans = character.transform.position;
        float newy = 0.2f;
        trans.y = newy;
        if (Physics.Raycast(trans, character.transform.forward, out hit, interactionDistance, interactableLayer))
        {
            interactable = hit.collider.GetComponent<UseageInteract>();
            if (interactable != null  )
            {
                Debug.Log("aaa");
                interactable.Interact(character,keyboardController);  // ��ȣ�ۿ� ����
                isInteracting = true;
            }
        }
    }
    void ResetInteraction()
    {
        if (interactable != null)
        {
            interactable.InteractAgain();//�ٽ� �Է�
            isInteracting = false;
        }
    }
}
