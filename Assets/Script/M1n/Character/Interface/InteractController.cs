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
    Outlinable newOutlinable = null; // 이번 프레임에 새로 아웃라인을 추가할 오브젝트
    Debug.Log(1);
    foreach (Collider col in colliders)
    {
    Debug.Log(2);
        IInterActerable candidate = col.GetComponent<IInterActerable>();
        if (candidate == null)
            continue;

        // 플레이어와의 거리 계산
    Debug.Log(3);
        float distance = Vector3.Distance(character.transform.position, col.transform.position);
        
        if (distance < interactionDistance)
        {
    Debug.Log(4);
            // Outlinable 컴포넌트가 없으면 추가
            newOutlinable = col.GetComponent<Outlinable>();
            if (newOutlinable == null)
            {
                newOutlinable = col.gameObject.AddComponent<Outlinable>();
                var meshRenderer = col.GetComponent<MeshRenderer>();
                if (meshRenderer)
                {
                    newOutlinable.AddRenderer(meshRenderer);
                }
                else
                {
                    // MeshRenderer가 없으면 처리하지 않음
                    continue;
                }
            }

            // Outlinable의 Enabled가 false일 때만 true로 설정
            if (!newOutlinable.OutlineParameters.Enabled)
            {
                newOutlinable.OutlineParameters.Enabled = true;
            }
        }
    }

    // 이전에 아웃라인이 있던 오브젝트가 존재하고, 새로 아웃라인을 적용할 오브젝트가 다르면
    // 기존 오브젝트에서 아웃라인을 꺼야 한다.
    if (currentOutlinable != null && newOutlinable != currentOutlinable)
    {
        currentOutlinable.OutlineParameters.Enabled = false; // 이전 아웃라인 꺼주기
    }

    // 새로 아웃라인을 적용할 오브젝트를 현재 아웃라인으로 설정
    currentOutlinable = newOutlinable; // 새로운 Outlinable을 현재 오브젝트로 업데이트
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
            Debug.Log(1);

            if (angle < 60f && distance < minDistance)
            {
                Debug.Log(2);
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
