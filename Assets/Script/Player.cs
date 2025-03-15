using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Character
{
    IController controller;
    public float RunSpeed;
    public float CrouchSpeed;
    protected float applyspeed;
    bool isCrouch,isRun;

    public override void Action()
    {
        
    }

    // Start is called before the first frame update
    void Start()
    {
        IController KeyboardControll = new KeyboardController();
        KeyboardControll.OnPosessed(this);
        this.controller = KeyboardControll;
        applyspeed = MoveSpeed;
    }


    // Update is called once per frame
    void Update()
    {
        if (controller != null)
        {
            controller.Tick(Time.deltaTime);
        }
        TryRun();
        TryCrouch();
        


    }
    public void TransSpeed(float speed)
    {
        applyspeed = speed;
    }
    public override float ReturnSpeed()
    {
        return applyspeed;
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
        isRun = true;
        applyspeed = RunSpeed;

    }
    void RunningCancel()
    {
        isRun = false;
        applyspeed = MoveSpeed;
    }
    void TryCrouch()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            Crouch();
        }
    }
    void Crouch()
    {
        isCrouch = !isCrouch;
        if (isCrouch)
        {
            applyspeed = CrouchSpeed;
        }
        else
        {
            applyspeed = MoveSpeed; 
        }
    }
}
