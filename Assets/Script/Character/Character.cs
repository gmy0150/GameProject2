using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Character : MonoBehaviour
{
    [Header("�⺻�ӵ�")]
    public float MoveSpeed;
    protected float applyspeed;

    public abstract void Action();

    public abstract float ReturnSpeed();

}
