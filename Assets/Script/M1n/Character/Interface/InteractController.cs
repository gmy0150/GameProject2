using EPOOutline;
using Pathfinding.Examples;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class InteractController
{
    public float interactionDistance = 8.0f;
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
        interactionDistance = controllerableCharacter.interactionDistance;
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
        UpdateOutline();
    }
    IInterActerable interactable;
    public bool GetInterAct()
    {
        return isInteracting;
    }
    Outlinable currentOutlinable = null; // 현재 아웃라인을 적용 중인 오브젝트

void UpdateOutline()
{
    Collider[] colliders = Physics.OverlapSphere(character.transform.position, interactionDistance, interactableLayer);
    Outlinable closestOutlinable = null;
    float closestDistance = Mathf.Infinity;

    foreach (Collider col in colliders)
    {
        IInterActerable candidate = col.GetComponent<IInterActerable>();
        if (candidate == null)
            continue;

        float distance = Vector3.Distance(character.transform.position, col.transform.position);

        if (distance < interactionDistance && distance < closestDistance)
        {
            Outlinable outlinable = col.GetComponent<Outlinable>();
            if (outlinable != null)
            {
                closestOutlinable = outlinable;
                closestDistance = distance;
            }
        }
    }

    // 기존 오브젝트와 다르거나, 새로 선택된 게 없거나, 거리 초과 시 이전 오브젝트의 아웃라인을 끈다
    if (currentOutlinable != null && 
        (currentOutlinable != closestOutlinable || closestOutlinable == null || closestDistance > interactionDistance))
    {
        currentOutlinable.OutlineParameters.Enabled = false;
        currentOutlinable = null;
    }

    // 새로 선택된 오브젝트의 아웃라인을 켠다
    if (closestOutlinable != null && !closestOutlinable.OutlineParameters.Enabled)
    {
        closestOutlinable.OutlineParameters.Enabled = true;
        currentOutlinable = closestOutlinable;
    }
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


            if ( distance < minDistance)
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
        if (currentInteractable != null)
        {
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
    public bool RotateInteract()
    {
        var slot = InventoryManager.Instance.GetActiveItem();
        if (slot != null)
        {
            return slot.RotateInteract();
        }
        return true;
    }

}
