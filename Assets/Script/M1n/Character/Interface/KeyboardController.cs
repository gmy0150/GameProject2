using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Build.Content;
using UnityEngine;

public class KeyboardController : IController
{
    
    Player controllerableCharacter = null;
    Camera mainCam;
    Rigidbody rigid;
    // bool isCrouch;
    private float rotationVelocity = 10f; // 회전 속도 보관
    public float RotationSmoothTime = 0.1f; // 부드러운 회전 시간
    float rotation = 0;
    public bool isHide;
    float runSpeed, walkSpeed, applySpeed;
    float runNoise, walkNoise, applyNoise;
    bool GenNoise;
    Animator anim;
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

        walkNoise = Player.WalkNoise;
        runNoise = Player.RunNoise;

        applySpeed = walkSpeed;
        applyNoise = walkNoise;

        GenNoise = false;
        anim = controllerableCharacter.GetComponent<Animator>();

    }
    public float GetSpeed()
    {
        return applySpeed;
    }
    bool bClickMouse;
    public void Tick(float deltaTime)
    {
        if (!isHide)
        {
            TryRun();
        }
        Transform tr = controllerableCharacter.transform;
        Vector3 direction = Vector3.zero;

        Vector3 forward = new Vector3(1, 0, 1).normalized;
        Vector3 back = -forward;
        Vector3 right = new Vector3(1, 0, -1).normalized;
        Vector3 left = -right;
        // anim.SetLayerWeight(0,0);

        if (!controllerableCharacter.GetInterActControll().RotateInteract())
        {
        // bClickMouse = true;
            // TransRotation();
        }
        if (Input.GetMouseButton(1))
        {
            // UpdateRotation();
        // bClickMouse = true;
            // TransRotation();
        }
        if(Input.GetKeyDown(KeyCode.F)){
            InventoryManager.Instance.RemoveItem();
        }
        InventoryManager.Instance.HandleSlotSelection();

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
            anim.SetBool(walk,true);
            
            rigid.velocity = new Vector3( direction.x * applySpeed,rigid.velocity.y , direction.z * applySpeed);
            float targetRotation = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
            float currentRotation = tr.eulerAngles.y;
            rotation = Mathf.SmoothDampAngle(currentRotation, targetRotation, ref rotationVelocity, RotationSmoothTime);
                tr.rotation = Quaternion.Euler(0.0f, rotation, 0.0f);
            if (!Input.GetMouseButton(1)&& controllerableCharacter.GetInterActControll().RotateInteract() )
            {
                Debug.Log(">");
            }
            
            if (GetNoise())
            {
                controllerableCharacter.MakeNoise(controllerableCharacter.gameObject, GetSpeed(), 10);
            }
        }
        else
        {
            rigid.velocity = new Vector3(0,rigid.velocity.y,0);
            anim.SetBool(walk,false);
        }
        if(bClickMouse && Input.GetMouseButtonUp(1))
            bClickMouse = false;
    }
    public void LateTick(float deltaTime){
        if(bClickMouse){
            // UpdateRotation();
        }else{
            anim.SetLayerWeight(1,0);
        }
    }
    //testRotate
// void UpdateRotation()
// {
//     Transform tr = controllerableCharacter.transform;

//     Ray ray = mainCam.ScreenPointToRay(Input.mousePosition);
//     if (Physics.Raycast(ray, out RaycastHit hit))
//     {
//         Vector3 lookDir = hit.point - tr.position;
//         lookDir.y = 0;

//         if (lookDir.magnitude > 0.1f)
//         {
//             // 현재 바라보는 방향과 목표 방향의 차이를 구한다
//             float angle = Vector3.SignedAngle(tr.forward, lookDir, Vector3.up);

//             // Angle을 정규화해서 (-1 ~ 1) 사이 값으로 만들기
//             float normalizedTurn = Mathf.Clamp(angle / 90f, -1f, 1f);

//             // 애니메이션 Turn 값 설정
//             anim.SetFloat("Turn", normalizedTurn);

//             // 회전값을 부드럽게 전환
//             Quaternion targetRotation = Quaternion.LookRotation(lookDir);

//             // 회전 속도를 조정: 5f로 회전 속도를 낮춰 부드럽게 회전
//             tr.rotation = Quaternion.Lerp(tr.rotation, targetRotation, Time.deltaTime * 5f);  // 5f를 적당히 조정해보세요.
//         }
//         else
//         {
//             // 마우스가 일정 범위 안에 있을 때 Turn 값을 부드럽게 0으로 보냄
//             float currentTurn = anim.GetFloat("Turn");
//             float targetTurn = 0f;
            
//             // Mathf.Lerp로 현재 Turn 값을 0으로 부드럽게 변화시킴
//             float smoothedTurn = Mathf.Lerp(currentTurn, targetTurn, Time.deltaTime * 5f);  // 5f로 부드러운 전환

//             anim.SetFloat("Turn", smoothedTurn);

//             // 회전값을 부드럽게 0으로 전환
//             tr.rotation = Quaternion.Lerp(tr.rotation, Quaternion.identity, Time.deltaTime * 5f);
//         }
//     }
// }
    // void TransRotation()
    // {
    //     Transform tr = controllerableCharacter.transform;
        
    //     Ray ray = mainCam.ScreenPointToRay(Input.mousePosition);
    //     if (Physics.Raycast(ray, out RaycastHit hit))
    //     {
    //         Debug.Log(1);
    //         Vector3 lookDir = hit.point - tr.position;
    //         lookDir.y = 0;

    //         if (lookDir.magnitude > 0.1f)
    //         {
    //             // anim.SetIKRotationWeight(1);
    //             Quaternion targetRotation = Quaternion.LookRotation(lookDir);
                
    //             // tr.rotation = Quaternion.Lerp(tr.rotation, targetRotation, Time.deltaTime * 10f);
    //             tr.rotation = targetRotation;

    //         }
    //     }

    // }
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
    string run = "Running";
    string walk = "Walking";
    void Running()
    {
        applySpeed = runSpeed;
        GenNoise = true;
        applyNoise = runNoise;
        anim.SetBool(run,true);
    }


    public void RunningCancel()
    {
        GenNoise = false;
        applySpeed = walkSpeed;
        applyNoise = 0;
        anim.SetBool(run,false);
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
