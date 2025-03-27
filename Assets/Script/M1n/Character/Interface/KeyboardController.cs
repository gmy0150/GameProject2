using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class KeyboardController : IController
{
    
    Character controllerableCharacter = null;
    Camera mainCam;
    Rigidbody rigid;
    Vector3 prev = Vector3.zero;
    private Vector3 previousDirection = Vector3.zero; // 이전 방향 저장
    private float rotationVelocity = 10f; // 회전 속도 보관
    public float RotationSmoothTime = 0.1f; // 부드러운 회전 시간
    float rotation = 0;
    public void OnPosessed(Character controllerableCharacter)
    {
        this.controllerableCharacter = controllerableCharacter;
        mainCam = Camera.main;
        rigid = controllerableCharacter.GetComponent<Rigidbody>();
        Debug.Log(rigid.name);
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
        if (Input.GetMouseButtonDown(0))
        {

            //controllerableCharacter.Action();

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
            direction.Normalize();
        if (bMoveKeyDown)
        {

            Debug.Log(controllerableCharacter.ReturnSpeed());
            rigid.velocity = new Vector3( direction.x * controllerableCharacter.ReturnSpeed(),rigid.velocity.y , direction.z * controllerableCharacter.ReturnSpeed());
            //tr.localPosition += direction * deltaTime * controllerableCharacter.ReturnSpeed();
            float targetRotation = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
            float currentRotation = tr.eulerAngles.y;
            float angleDiff = Mathf.DeltaAngle(currentRotation, targetRotation);
            rotation = Mathf.SmoothDampAngle(currentRotation, targetRotation, ref rotationVelocity, RotationSmoothTime);
            if (!Input.GetMouseButton(1) )
            {
                tr.rotation = Quaternion.Euler(0.0f, rotation, 0.0f);
            }
            //tr.rotation = Quaternion.Euler(0.0f, rotation, 0.0f);
            previousDirection = direction;
            if (controllerableCharacter.GetNoise())
            {
                controllerableCharacter.MakeNoise(controllerableCharacter.gameObject, controllerableCharacter.ReturnNoise(), 10);
            }
        }
        else
        {
            rigid.velocity = new Vector3(0,rigid.velocity.y,0);
        }
    }

}
