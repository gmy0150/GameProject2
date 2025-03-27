using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public abstract class Character : MonoBehaviour
{
    [Header("기본속도")]
    public float MoveSpeed;
    public float RunSpeed;
    protected float applyspeed;
    protected float applyNoise;
    public abstract void Action();

    public abstract float ReturnSpeed();
    public abstract float ReturnNoise();
    public abstract bool GetNoise();
    public abstract void MakeNoise(GameObject obj, float radius, float stepsize);



}
