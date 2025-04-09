using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class KeyboardController : IController
{
    
    Player controllerableCharacter = null;
    Camera mainCam;
    Rigidbody rigid;
    bool isCrouch;
    private float rotationVelocity = 10f; // 회전 속도 보관
    public float RotationSmoothTime = 0.1f; // 부드러운 회전 시간
    float rotation = 0;
    public bool isHide;
    float runSpeed, walkSpeed, crouchSpeed, applySpeed;
    float runNoise, walkNoise, applyNoise;
    bool GenNoise;
    LayerMask Picture;
    public bool GetHide()//찾는거 있음
    {
        return isHide;
    }
    public void OnPosessed(Player controllerableCharacter)
    {
        this.controllerableCharacter = controllerableCharacter;
        mainCam = Camera.main;
        rigid = controllerableCharacter.GetComponent<Rigidbody>();

        runSpeed = controllerableCharacter.RunSpeed;
        walkSpeed = controllerableCharacter.MoveSpeed;
        crouchSpeed = controllerableCharacter.CrouchSpeed;

        walkNoise = Player.WalkNoise;
        runNoise = Player.RunNoise;

        applySpeed = walkSpeed;
        applyNoise = walkNoise;

        GenNoise = true;

        Picture = controllerableCharacter.Picture;

    }
    public float GetSpeed()
    {
        return applySpeed;
    }

    public void Tick(float deltaTime)
    {
        if (!isHide)
        {
            TryRun();
            TryCrouch();
        }
        Transform tr = controllerableCharacter.transform;
        Vector3 direction = Vector3.zero;

        Vector3 forward = new Vector3(1, 0, 1).normalized;
        Vector3 back = -forward;
        Vector3 right = new Vector3(1, 0, -1).normalized;
        Vector3 left = -right;
        if (controllerableCharacter.GetInterActControll().RotateInteract())
        {
            TransRotation();
        }
        if (Input.GetMouseButton(1))
        {
            TransRotation();
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
            
            rigid.velocity = new Vector3( direction.x * applySpeed,rigid.velocity.y , direction.z * applySpeed);
            float targetRotation = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
            float currentRotation = tr.eulerAngles.y;
            float angleDiff = Mathf.DeltaAngle(currentRotation, targetRotation);
            rotation = Mathf.SmoothDampAngle(currentRotation, targetRotation, ref rotationVelocity, RotationSmoothTime);
            if (!Input.GetMouseButton(1) && !controllerableCharacter.GetInterActControll().RotateInteract())
            {
                tr.rotation = Quaternion.Euler(0.0f, rotation, 0.0f);
            }
            
            //tr.rotation = Quaternion.Euler(0.0f, rotation, 0.0f);
            if (GetNoise())
            {
                controllerableCharacter.MakeNoise(controllerableCharacter.gameObject, GetSpeed(), 10);
            }
        }
        else
        {
            rigid.velocity = new Vector3(0,rigid.velocity.y,0);
        }
    }
    void TransRotation()
    {
        Transform tr = controllerableCharacter.transform;
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
    void TryRun()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            Running();
        }
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            RunningCancel();
        }
    }
    void Running()
    {
        if (isCrouch)
            Crouch();

        applySpeed = runSpeed;
        GenNoise = true;
        applyNoise = runNoise;
    }


    void RunningCancel()
    {
        applySpeed = walkSpeed;
        applyNoise = walkNoise;
        GenNoise = true;
    }
    void TryCrouch()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            Crouch();
        }
        if (Input.GetKeyUp(KeyCode.LeftControl))
        {
            CrouchCancel();
        }
    }
    public void CrouchCancel()
    {
        isCrouch = false;
        GenNoise = true;
        applySpeed = walkSpeed;
        applyNoise = walkNoise;
    }
    public void Crouch()
    {
        isCrouch = true;
        applySpeed = crouchSpeed;
        GenNoise = false;
        applyNoise = 0;
    }
    public float ApplyNoise()
    {
        return applyNoise;
    }
    public bool GetNoise()
    {
        return GenNoise;
    }
    public void SetHide(bool x)
    {
        isHide = x;
    }
    public void SetNoise(bool x)
    {
        GenNoise = x;

    }
}
