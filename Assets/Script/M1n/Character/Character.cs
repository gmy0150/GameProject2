using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public abstract class Character : MonoBehaviour
{
    [Header("�⺻�ӵ�")]
    public float MoveSpeed;
    public float RunSpeed;
    protected float applyspeed;
    protected float applyNoise;

    public abstract void MakeNoise(GameObject obj, float radius, float stepsize);



}
