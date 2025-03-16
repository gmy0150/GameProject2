using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class KeyboardController : IController
{
    
    Character controllerableCharacter = null;
    Camera mainCam;
    Rigidbody rigid;
    public void OnPosessed(Character controllerableCharacter)
    {
        this.controllerableCharacter = controllerableCharacter;
        mainCam = Camera.main;
        rigid = controllerableCharacter.GetComponent<Rigidbody>();
    }

    public void Tick(float deltaTime)
    {
        Transform tr = controllerableCharacter.transform;
        Vector3 direction = Vector3.zero;

        Vector3 forward = new Vector3(1, 0, 1).normalized;
        Vector3 back = -forward;
        Vector3 right = new Vector3(1, 0, -1).normalized;
        Vector3 left = -right;

        if (Input.GetMouseButton(1))
        {

            Ray ray = mainCam.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                Vector3 lookDir = hit.point - tr.position;
                lookDir.y = 0;

                if (lookDir.magnitude > 0.1f)
                {
                    Quaternion targetRotation = Quaternion.LookRotation(lookDir);
                    tr.rotation = Quaternion.Lerp(tr.rotation, targetRotation, Time.deltaTime * 10f);
                }
            }

        }
        bool bMoveKeyDown = false;
        if (Input.GetKey(KeyCode.W))
        {
            direction += forward;
            bMoveKeyDown = true;
        }
        if (Input.GetKey(KeyCode.S))
        {
            direction += back;
            bMoveKeyDown = true;
        }
        if (Input.GetKey(KeyCode.A))
        {
            direction += left;
            bMoveKeyDown = true;
        }
        if (Input.GetKey(KeyCode.D))
        {
            direction += right;
            bMoveKeyDown = true;
        }
        if (bMoveKeyDown)
        {
            direction.Normalize();
            rigid.velocity = direction * controllerableCharacter.ReturnSpeed();
            //tr.localPosition += direction * deltaTime * controllerableCharacter.ReturnSpeed();
        }
        else
        {
            rigid.velocity = new Vector3(0,rigid.velocity.y,0);
        }
    }
}
